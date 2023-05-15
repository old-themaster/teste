﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.PlayerLoadingAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;

namespace Game.Server.RingStation.Action
{
  public class PlayerLoadingAction : BaseAction
  {
    private int m_loading;

    public PlayerLoadingAction(int state, int delay)
      : base(delay, 0)
      => this.m_loading = state;

    protected override void ExecuteImp(RingStationGamePlayer player, long tick)
    {
      if (this.m_loading > 100)
        this.m_loading = 100;
      player.SendLoadingComplete(this.m_loading);
      if (this.m_loading < 100)
      {
        Random random = new Random();
        player.AddAction((IAction) new PlayerLoadingAction(this.m_loading + random.Next(20, 40), 1000));
      }
      this.Finish(tick);
    }
  }
}
