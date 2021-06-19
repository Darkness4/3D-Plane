using Godot;

public class Player : KinematicBody
{
    [Export] private readonly float _acceleration = 0.1f;
    [Export] private readonly float _yawSpeed = 0.01f;  // y
    [Export] private readonly float _rollSpeed = 0.05f;  // z
    [Export] private readonly float _pitchSpeed = 0.05f;  // x

    private Vector3 _velocity;
    private float _speed = -100;
    private bool _isIncrement = true;

    private Spatial[] _guns = null!;
    private Timer _gunCooldownTimer = null!;
    private Timer _speedCooldownTimer = null!;
    private GlobalSignals _globalSignals = null!;
    private Camera _firstPersonCamera = null!;
    private Camera _thirdPersonCamera = null!;

    public override void _Ready()
    {
        _guns = new[]
        {
            GetNode<Spatial>("Gun0"),
            GetNode<Spatial>("Gun1"),
        };
        _gunCooldownTimer = GetNode<Timer>("GunCooldownTimer");
        _speedCooldownTimer = GetNode<Timer>("SpeedCooldownTimer");
        _globalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
        _firstPersonCamera = GetNode<Camera>("FirstPersonCamera");
        _thirdPersonCamera = GetNode<Camera>("ThirdPersonCamera");
        Visible = _thirdPersonCamera.Current;
    }

    public override void _PhysicsProcess(float delta)
    {
        Move();
    }

    private void Move()
    {
        var inputVector = new Vector3
        {
            z = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            y = Input.GetActionStrength("yaw_right") - Input.GetActionStrength("yaw_left"),
            x = Input.GetActionStrength("ui_up") - Input.GetActionStrength("ui_down"),
        };

        var inputVelocity = inputVector.Normalized();

        _velocity = _velocity.LinearInterpolate(inputVelocity, _acceleration);
        GlobalRotate(Transform.basis.x.Normalized(), -_velocity.x  * _pitchSpeed);
        GlobalRotate(Transform.basis.z.Normalized(), -_velocity.z  * _rollSpeed);
        GlobalRotate(Transform.basis.y.Normalized(), -_velocity.y  * _yawSpeed);

        MoveAndSlide(Transform.basis.z * _speed);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("switch_camera"))
        {
            _firstPersonCamera.Current = !_firstPersonCamera.Current;
            _thirdPersonCamera.Current = !_firstPersonCamera.Current;
            Visible = _thirdPersonCamera.Current;
        }

        if (@event.IsActionPressed("ui_accept"))
        {
            _gunCooldownTimer.Start();
        }

        if (@event.IsActionReleased("ui_accept"))
        {
            _gunCooldownTimer.Stop();
        }

        if (@event.IsActionPressed("speed_up"))
        {
            _isIncrement = true;
            _speedCooldownTimer.Start();
        }

        if (@event.IsActionReleased("speed_up"))
        {
            _speedCooldownTimer.Stop();
        }

        if (@event.IsActionPressed("speed_down"))
        {
            _isIncrement = false;
            _speedCooldownTimer.Start();
        }

        if (@event.IsActionReleased("speed_down"))
        {
            _speedCooldownTimer.Stop();
        }
    }

    private void _on_GunCooldownTimer_timeout()
    {
        foreach (var gun in _guns)
        {
            _globalSignals.EmitSignal(nameof(GlobalSignals.BulletFired), gun.GlobalTransform);
        }
    }

    private void _on_SpeedCooldownTimer_timeout()
    {
        GD.Print(_speed);
        switch (_isIncrement)
        {
            case true when _speed > -100:
                _speed--;
                break;
            case false when _speed < -10:
                _speed++;
                break;
        }
    }
}
