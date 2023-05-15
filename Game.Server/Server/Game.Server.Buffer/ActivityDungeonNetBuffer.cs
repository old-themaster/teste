﻿
// Type: Game.Server.Buffer.ActivityDungeonNetBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
    public class ActivityDungeonNetBuffer : AbstractBuffer
  {
    public ActivityDungeonNetBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      ActivityDungeonNetBuffer ofType = player.BufferList.GetOfType(typeof (ActivityDungeonNetBuffer)) as ActivityDungeonNetBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate = this.Info.ValidDate;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
        player.UpdateFightBuff(this.Info);
      }
      else
      {
        base.Start(player);
        player.FightBuffs.Add(this.Info);
      }
    }

    public override void Stop()
    {
      this.m_player.FightBuffs.Remove(this.Info);
      base.Stop();
    }
  }
}
