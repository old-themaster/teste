﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryInfoDeleteHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(234, "撤消征婚信息")]
  public class MarryInfoDeleteHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      string translation = LanguageMgr.GetTranslation("MarryInfoDeleteHandler.Fail");
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        if (playerBussiness.DeleteMarryInfo(num, client.Player.PlayerCharacter.ID, ref translation))
          client.Out.SendAuctionRefresh((AuctionInfo) null, num, false, (ItemInfo) null);
        client.Out.SendMessage(eMessageType.GM_NOTICE, translation);
      }
      return 0;
    }
  }
}
