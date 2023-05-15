// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.DefyAfficheHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(123, "场景用户离开")]
  public class DefyAfficheHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string str = packet.ReadString();
      int num = 500;
      if (client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock >= num)
      {
        client.Player.RemoveMoney(num);
        GSPacketIn packet1 = new GSPacketIn((short) 123);
        packet1.WriteString(str);
        GameServer.Instance.LoginServer.SendPacket(packet1);
        client.Player.LastChatTime = DateTime.Now;
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          packet1.ClientID = allPlayer.PlayerCharacter.ID;
          allPlayer.Out.SendTCP(packet1);
        }
        client.Player.OnPlayerDispatches();
      }
      else
        client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("UserBuyItemHandler.Money"));
      return 0;
    }
  }
}
