// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GetPlayerCardHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(18, "场景用户离开")]
  public class GetPlayerCardHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(num);
      PlayerInfo player;
      List<UsersCardInfo> userCard;
      if (playerById != null)
      {
        player = playerById.PlayerCharacter;
        userCard = playerById.CardBag.GetCards(0, 4);
      }
      else
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          player = playerBussiness.GetUserSingleByUserID(num);
          userCard = playerBussiness.GetUserCardEuqip(num);
        }
      }
      if (userCard != null && player != null)
        client.Player.Out.SendUpdateCardData(player, userCard);
      return 0;
    }
  }
}
