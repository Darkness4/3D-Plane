using Godot;
using System;

public partial class NoMoreBullets : Label
{
  public override void _Ready()
  {
    GetNode<GlobalSignals>("/root/GlobalSignals").Connect(
        nameof(GlobalSignals.NoMoreBullet),
        new Callable(this, MethodName.ShowMessage)
    );
  }

  private void ShowMessage()
  {
    Visible = true;
  }
}
