﻿
// Type: Game.Server.Buffer.CaddyGoodsBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
    public class CaddyGoodsBuffer : AbstractBuffer
  {
    public CaddyGoodsBuffer(BufferInfo buffer)
      : base(buffer)
    {
    }

    public override void Start(GamePlayer player)
    {
      CaddyGoodsBuffer ofType = player.BufferList.GetOfType(typeof (CaddyGoodsBuffer)) as CaddyGoodsBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
        base.Start(player);
    }

    public override void Stop()
    {
      base.Stop();
    }
  }
}