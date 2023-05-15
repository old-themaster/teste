// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.PlayerUsePropAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.RingStation.Action
{
  public class PlayerUsePropAction : BaseAction
  {
    private int m_prop;

    public PlayerUsePropAction(int prop, int delay)
      : base(delay, 0)
      => this.m_prop = prop;

    protected override void ExecuteImp(RingStationGamePlayer player, long tick)
    {
      player.SendUseProp(this.m_prop);
      this.Finish(tick);
    }
  }
}
