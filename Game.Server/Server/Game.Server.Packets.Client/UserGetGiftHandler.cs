// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserGetGiftHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(218, "领取奖品")]
  public class UserGetGiftHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      UserGiftInfo[] allGifts = (UserGiftInfo[]) null;
      PlayerInfo player = client.Player.PlayerCharacter;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        allGifts = playerBussiness.GetAllUserReceivedGifts(num);
        if (player.ID != num)
        {
          GamePlayer playerById = WorldMgr.GetPlayerById(num);
          player = playerById == null ? playerBussiness.GetUserSingleByUserID(num) : playerById.PlayerCharacter;
        }
      }
      if (allGifts != null && player != null)
        client.Out.SendGetUserGift(player, allGifts);
      return 0;
    }
  }
}
