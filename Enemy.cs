using System;
using Godot;

public partial class Enemy : CharacterBody3D
{
  private readonly Random _randomizer = new();
  private Timer _killTimer = null!;

  public override void _Ready()
  {
    _killTimer = GetNode<Timer>("KillTimer");
    _killTimer.Start();
  }

  public override void _PhysicsProcess(double delta)
  {
    Move();
  }

  private void Move()
  {
    Velocity = new Vector3
    {
      Z = _randomizer.Next(20, 50)
    };
    MoveAndSlide();
    if (Transform.Origin.Z > 10) QueueFree();
  }

  /// <summary>
  ///   <c>Enemy</c> has timed out.
  /// </summary>
  private void OnKillTimerTimeout()
  {
    QueueFree();
  }
}
