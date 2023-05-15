﻿
// Type: Game.Server.Buffer.GuardBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
    public class GuardBuffer : AbstractBuffer
  {
    public GuardBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      GuardBuffer ofType = player.BufferList.GetOfType(typeof (GuardBuffer)) as GuardBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
        if (ofType.Info.ValidDate > 30)
          ofType.Info.ValidDate = 30;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
      {
        base.Start(player);
        player.PlayerCharacter.GuardAddPlus += this.Info.Value;
      }
    }

    public override void Stop()
    {
      this.m_player.PlayerCharacter.GuardAddPlus -= this.m_info.Value;
      base.Stop();
    }
  }
}
