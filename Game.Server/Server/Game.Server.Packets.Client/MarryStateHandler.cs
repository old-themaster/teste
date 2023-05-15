// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryStateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
    [PacketHandler(251, "当前场景状态")]
  internal class MarryStateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadInt())
      {
        case 0:
          if (client.Player.IsInMarryRoom)
          {
            if (client.Player.MarryMap != 1)
            {
              if (client.Player.MarryMap == 2)
              {
                client.Player.X = 800;
                client.Player.Y = 763;
              }
            }
            else
            {
              client.Player.X = 646;
              client.Player.Y = 1241;
            }
            foreach (GamePlayer allPlayer in client.Player.CurrentMarryRoom.GetAllPlayers())
            {
              if (allPlayer != client.Player && allPlayer.MarryMap == client.Player.MarryMap)
              {
                allPlayer.Out.SendPlayerEnterMarryRoom(client.Player);
                client.Player.Out.SendPlayerEnterMarryRoom(allPlayer);
              }
            }
            break;
          }
          break;
        case 1:
          RoomMgr.EnterWaitingRoom(client.Player);
          break;
      }
      return 0;
    }
  }
}
