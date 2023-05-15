// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryStatusHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(246, "请求结婚状态")]
  internal class MarryStatusHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(num);
      if (playerById != null)
      {
        client.Player.Out.SendPlayerMarryStatus(client.Player, playerById.PlayerCharacter.ID, playerById.PlayerCharacter.IsMarried);
      }
      else
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(num);
          client.Player.Out.SendPlayerMarryStatus(client.Player, userSingleByUserId.ID, userSingleByUserId.IsMarried);
        }
      }
      return 0;
    }
  }
}
