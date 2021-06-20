using Godot;
using System;

public class ObstacleSpawner : Node
{
    [Export] private readonly PackedScene _obstacleFactory = null!;
    [Export] private readonly int _count = 10;
    private Node _main = null!;

    public override void _Ready()
    {
        _main = GetTree().CurrentScene;
        CallDeferred(nameof(Spawn));
    }

    private void Spawn()
    {
        for (var i = 0; i < _count; i++)
        {
            var obstacle = _obstacleFactory.Instance() as Spatial;
            _main.AddChild(obstacle);
        }
    }
}
