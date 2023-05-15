﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.BaseAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.RingStation.Action
{
  public class BaseAction : IAction
  {
    private long m_finishDelay;
    private long m_finishTick;
    private long m_tick;

    public BaseAction(int delay)
      : this(delay, 0)
    {
    }

    public BaseAction(int delay, int finishDelay)
    {
      this.m_tick = TickHelper.GetTickCount() + (long) delay;
      this.m_finishDelay = (long) finishDelay;
      this.m_finishTick = long.MaxValue;
    }

    public void Execute(RingStationGamePlayer player, long tick)
    {
      if (this.m_tick > tick || this.m_finishTick != long.MaxValue)
        return;
      this.ExecuteImp(player, tick);
    }

    protected virtual void ExecuteImp(RingStationGamePlayer player, long tick) => this.Finish(tick);

    public void Finish(long tick) => this.m_finishTick = tick + this.m_finishDelay;

    public bool IsFinished(long tick) => this.m_finishTick <= tick;
  }
}
