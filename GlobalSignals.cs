using System;
using Godot;

public partial class GlobalSignals : Node
{
  /// <summary>
  ///     A bullet has been fired with the following parameters.
  /// </summary>
  [Signal]
  public delegate void BulletFiredEventHandler(Transform3D transform);

  [Signal]
  public delegate void NoMoreBulletEventHandler();
}
