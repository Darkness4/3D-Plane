using Godot;
using System;

public partial class ObstacleSpawner : Node
{
  [Export] private PackedScene _obstacleFactory = null!;
  [Export] private int _count = 10;
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
      var obstacle = _obstacleFactory.Instantiate<Node3D>();
      _main.AddChild(obstacle);
    }
  }
}
