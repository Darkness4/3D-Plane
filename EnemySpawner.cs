using System;
using Godot;

public partial class EnemySpawner : Node3D
{
  private readonly Random _randomizer = new();
  [Export] private PackedScene _unitFactory = null!;
  private Node _main = null!;

  public override void _Ready()
  {
    _main = GetTree().CurrentScene;
  }

  private void Spawn()
  {
    var enemy = _unitFactory.Instantiate<Node3D>();
    _main.AddChild(enemy);
    enemy!.Transform = new Transform3D(enemy.Transform.Basis, Transform.Origin + new Vector3
    {
      X = _randomizer.Next(-15, 15),
      Y = _randomizer.Next(-10, 10)
    });
  }

  // ReSharper disable once UnusedMember.Local
  private void OnTimerTimeout()
  {
    Spawn();
  }
}
