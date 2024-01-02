using Godot;

public partial class Player : CharacterBody3D
{
  private const float _acceleration = 0.1f;
  private const float _yawSpeed = 1f;  // y
  private const float _rollSpeed = 2f;  // z
  private const float _pitchSpeed = 2f;  // x
  private const float _maxSpeed = 4000f;
  private const float _minSpeed = 1000f;

  private Vector3 _velocity;
  private float _speed = -_maxSpeed;
  private bool _isIncrement = true;
  private int _ammo = 960;
  private Vector3 _thirdPersonCameraOriginalOrigin;
  private Basis _thirdPersonCameraOriginalBasis;

  private Node3D[] _guns = null!;
  private Timer _gunCooldownTimer = null!;
  private Timer _speedCooldownTimer = null!;
  private GlobalSignals _globalSignals = null!;
  private Camera3D _firstPersonCamera = null!;
  private Camera3D _thirdPersonCamera = null!;
  private MeshInstance3D _playerMesh = null!;

  public override void _Ready()
  {
    _guns = new[]
    {
            GetNode<Node3D>("Gun0"),
            GetNode<Node3D>("Gun1"),
        };
    _gunCooldownTimer = GetNode<Timer>("GunCooldownTimer");
    _speedCooldownTimer = GetNode<Timer>("SpeedCooldownTimer");
    _globalSignals = GetNode<GlobalSignals>("/root/GlobalSignals");
    _firstPersonCamera = GetNode<Camera3D>("FirstPersonCamera");
    _thirdPersonCamera = GetNode<Camera3D>("ThirdPersonCamera");
    _playerMesh = GetNode<MeshInstance3D>("PlayerMesh");
    _playerMesh.Visible = _thirdPersonCamera.Current;
    _thirdPersonCameraOriginalOrigin = _thirdPersonCamera.Transform.Origin;
    _thirdPersonCameraOriginalBasis = _thirdPersonCamera.Transform.Basis;
  }

  public override void _PhysicsProcess(double delta)
  {
    Move(delta);
  }

  private void Move(double delta)
  {
    var inputVelocity = new Vector3
    {
      Z = Input.GetAxis("roll_left", "roll_right"),
      Y = Input.GetAxis("yaw_left", "yaw_right"),
      X = Input.GetAxis("pitch_up", "pitch_down")
    }.Normalized();

    _velocity = _velocity.Lerp(inputVelocity, _acceleration);

    // Rotate the ship
    GlobalRotate(Transform.Basis.X.Normalized(), -_velocity.X * _pitchSpeed * (float)delta);
    GlobalRotate(Transform.Basis.Z.Normalized(), -_velocity.Z * _rollSpeed * (float)delta);
    GlobalRotate(Transform.Basis.Y.Normalized(), -_velocity.Y * _yawSpeed * (float)delta);
    TransformCamera();

    // Move forward
    Velocity = GlobalTransform.Basis.Z.Normalized() * _speed * (float)delta;
    MoveAndSlide();
  }

  private void TransformCamera()
  {
    _thirdPersonCamera.Transform = new Transform3D(
        _thirdPersonCameraOriginalBasis.Rotated(_thirdPersonCameraOriginalBasis.Z.Normalized(), -_velocity.Z * 0.5f),
        _thirdPersonCameraOriginalOrigin + new Vector3
        {
          X = _velocity.Y * 2,
          Y = -_velocity.X * 2,
        }
    );
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("switch_camera"))
    {
      _firstPersonCamera.Current = !_firstPersonCamera.Current;
      _thirdPersonCamera.Current = !_firstPersonCamera.Current;
      _playerMesh.Visible = _thirdPersonCamera.Current;
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

  private void OnGunCooldownTimerTimeout()
  {
    if (_ammo > 0)
    {
      _ammo -= _guns.Length;
      foreach (var gun in _guns)
      {
        _globalSignals.EmitSignal(nameof(GlobalSignals.BulletFired), gun.GlobalTransform);
      }
    }
    else
    {
      _globalSignals.EmitSignal(nameof(GlobalSignals.NoMoreBullet));
    }
  }

  private void OnSpeedCooldownTimerTimeout()
  {
    GD.Print(_speed);
    if (_isIncrement && _speed > -_maxSpeed)
    {
      _speed -= 100f;
    }
    else if (!_isIncrement && _speed < -_minSpeed)
    {
      _speed += 100f;
    }
  }
}
