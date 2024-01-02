using System;
using Godot;

public partial class Bullet : CharacterBody3D
{
  [Export] private float _spread = 0.005f;

  private readonly Random _randomizer = new();
  private Timer _killTimer = null!;
  private const float Gravity = 0.1f;
  private Vector3 _downVelocity = Vector3.Zero;
  private static readonly Vector3 HalfDown = new() { Y = -45f };

  public override void _Ready()
  {
    _killTimer = GetNode<Timer>("KillTimer")!;
    _killTimer.Start();
  }

  public override void _PhysicsProcess(double delta)
  {
    Move();
  }

  private void Move()
  {
    _downVelocity = _downVelocity.Lerp(HalfDown, Gravity);
    Velocity = GlobalTransform.Basis.Z * -500 + _downVelocity;
    MoveAndSlide();
  }

  public void Initialize(Transform3D transform)
  {
    transform.Basis = transform.Basis
        .Rotated(transform.Basis.X, (float)((_randomizer.NextDouble() * 2 - 1) * _spread))
        .Rotated(transform.Basis.Y, (float)((_randomizer.NextDouble() * 2 - 1) * _spread));
    Transform = transform;
  }

  private void OnAreaBodyEntered(Node body)
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
  private void OnKillTimerTimeout()
  {
    QueueFree();
  }
}
