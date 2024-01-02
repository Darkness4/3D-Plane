using Godot;

/// <summary>
///     Manages all in-game bullet.
/// </summary>
public partial class BulletManager : Node
{
  public override void _Ready()
  {
    GetNode<GlobalSignals>("/root/GlobalSignals").Connect(
        nameof(GlobalSignals.BulletFired),
        new Callable(this, MethodName.SpawnBullet)
    );
  }

  [Export] private PackedScene _bulletFactory = null!;

  /// <summary>
  ///     Spawn a <c>Bullet</c>
  /// </summary>
  public void SpawnBullet(
      Transform3D transform
  )
  {
    var bullet = _bulletFactory.Instantiate<Bullet>();
    AddChild(bullet);
    bullet.Initialize(transform);
  }
}
