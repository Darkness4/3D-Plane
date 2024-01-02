using Godot;
using System;

public partial class RandomStaticBody : StaticBody3D
{
  private readonly Random _randomizer = new();

  public override void _Ready()
  {
    var basis = Transform.Basis
        .Rotated(Transform.Basis.X, (float)((_randomizer.NextDouble() * 2 - 1) * Math.PI))
        .Rotated(Transform.Basis.Y, (float)((_randomizer.NextDouble() * 2 - 1) * Math.PI))
        .Rotated(Transform.Basis.Z, (float)((_randomizer.NextDouble() * 2 - 1) * Math.PI));
    var origin = new Vector3
    {
      X = (float)(_randomizer.NextDouble() * 2 - 1) * 200,
      Y = (float)(_randomizer.NextDouble() * 2 - 1) * 200,
      Z = (float)(_randomizer.NextDouble() * 2 - 1) * 200
    };
    Transform = new Transform3D(basis, origin);
  }
}
