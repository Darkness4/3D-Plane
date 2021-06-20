using Godot;
using System;

public class RandomStaticBody : StaticBody
{
    private readonly Random _randomizer = new();

    public override void _Ready()
    {
        var basis = Transform.basis
            .Rotated(Transform.basis.x, (float) ((_randomizer.NextDouble() * 2 - 1) * Math.PI))
            .Rotated(Transform.basis.y, (float) ((_randomizer.NextDouble() * 2 - 1) * Math.PI))
            .Rotated(Transform.basis.z, (float) ((_randomizer.NextDouble() * 2 - 1) * Math.PI));
        var origin = new Vector3
        {
            x = (float) (_randomizer.NextDouble() * 2 - 1) * 200,
            y = (float) (_randomizer.NextDouble() * 2 - 1) * 200,
            z = (float) (_randomizer.NextDouble() * 2 - 1) * 200
        };
        Transform = new Transform(basis, origin);
    }
}
