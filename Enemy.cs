using System;
using Godot;

public class Enemy : KinematicBody
{
    private readonly Random _randomizer = new();
    private Timer _killTimer = null!;

    public override void _Ready()
    {
        _killTimer = GetNode<Timer>("KillTimer");
        _killTimer.Start();
    }

    public override void _PhysicsProcess(float delta)
    {
        Move();
    }

    private void Move()
    {
        MoveAndSlide(new Vector3
        {
            z = _randomizer.Next(20, 50)
        });
        if (Transform.origin.z > 10) QueueFree();
    }

    /// <summary>
    ///   <c>Enemy</c> has timed out.
    /// </summary>
    private void _on_KillTimer_timeout()
    {
        QueueFree();
    }
}
