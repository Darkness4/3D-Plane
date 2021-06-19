using System;
using Godot;

public class EnemySpawner : Spatial
{
    private readonly Random _randomizer = new();
    [Export] private readonly PackedScene _unitFactory = null!;
    private Node _main = null!;

    public override void _Ready()
    {
        _main = GetTree().CurrentScene;
    }

    private void Spawn()
    {
        var enemy = _unitFactory.Instance() as Spatial;
        _main.AddChild(enemy);
        enemy!.Transform = new Transform(enemy.Transform.basis, Transform.origin + new Vector3
        {
            x = _randomizer.Next(-15, 15),
            y = _randomizer.Next(-10, 10)
        });
    }

    // ReSharper disable once UnusedMember.Local
    private void _on_Timer_timeout()
    {
        Spawn();
    }
}
