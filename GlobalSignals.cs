using System;
using Godot;

public class GlobalSignals : Node
{
    /// <summary>
    ///     A bullet has been fired with the following parameters.
    /// </summary>
    [Signal]
    public delegate void BulletFired(Transform transform);
}
