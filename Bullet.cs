using System;
using Godot;

public class Bullet : KinematicBody
{
    [Export] private readonly float _spread = 0.005f;

    private readonly Random _randomizer = new();
    private Timer _killTimer = null!;
    private const float Gravity = 0.1f;
    private Vector3 _downVelocity = Vector3.Zero;
    private static readonly Vector3 HalfDown = new() { y = -45f};

    public override void _Ready()
    {
        _killTimer = GetNode<Timer>("KillTimer")!;
        _killTimer.Start();
    }

    public override void _PhysicsProcess(float delta)
    {
        Move();
    }

    private void Move()
    {
        _downVelocity = _downVelocity.LinearInterpolate(HalfDown, Gravity);
        MoveAndSlide(GlobalTransform.basis.z * -500 + _downVelocity);
    }

    public void Initialize(Transform transform)
    {
        transform.basis = transform.basis
            .Rotated(transform.basis.x, (float) ((_randomizer.NextDouble() * 2 - 1) * _spread))
            .Rotated(transform.basis.y, (float) ((_randomizer.NextDouble() * 2 - 1) * _spread));
        Transform = transform;
    }

    private void _on_Area_body_entered(Node body)
    {
        if (body.IsInGroup("Enemies"))
        {
            body.QueueFree();
        }
        QueueFree();
    }

    /// <summary>
    ///     <c>Bullet</c> has timed out.
    /// </summary>
    private void _on_KillTimer_timeout()
    {
        QueueFree();
    }
}
