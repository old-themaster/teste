
// Type: Game.Server.Buffer.OfferMultipleBuffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Buffer
{
    public class OfferMultipleBuffer : AbstractBuffer
  {
    public OfferMultipleBuffer(BufferInfo info)
      : base(info)
    {
    }

    public override void Start(GamePlayer player)
    {
      OfferMultipleBuffer ofType = player.BufferList.GetOfType(typeof (OfferMultipleBuffer)) as OfferMultipleBuffer;
      if (ofType != null)
      {
        ofType.Info.ValidDate += this.Info.ValidDate;
        player.BufferList.UpdateBuffer((AbstractBuffer) ofType);
      }
      else
      {
        base.Start(player);
        player.OfferAddPlus *= (double) this.Info.Value;
      }
    }

    public override void Stop()
    {
      this.m_player.OfferAddPlus /= (double) this.m_info.Value;
      base.Stop();
    }
  }
}
