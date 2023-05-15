﻿
// Type: Game.Server.Buffer.ConsortionAddEffectTurnBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
    public class ConsortionAddEffectTurnBuffer : AbstractBuffer
  {
    public ConsortionAddEffectTurnBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      ConsortionAddEffectTurnBuffer ofType = player.BufferList.GetOfType(typeof (ConsortionAddEffectTurnBuffer)) as ConsortionAddEffectTurnBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.m_info.ValidDate;
        ofType.Info.TemplateID = this.m_info.TemplateID;
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
