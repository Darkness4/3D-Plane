using Godot;

/// <summary>
///     Manages all in-game bullet.
/// </summary>
public class BulletManager : Node
{
    public override void _Ready()
    {
        GetNode<GlobalSignals>("/root/GlobalSignals").Connect(
            nameof(GlobalSignals.BulletFired),
            this,
            nameof(SpawnBullet)
        );
    }

    [Export] private readonly PackedScene _bulletFactory = null!;

    /// <summary>
    ///     Spawn a <c>Bullet</c>
    /// </summary>
    public void SpawnBullet(
        Transform transform
    )
    {
        var bullet = (_bulletFactory.Instance() as Bullet)!;
        AddChild(bullet);
        bullet.Initialize(transform);
    }
}
