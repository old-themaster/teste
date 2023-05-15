using Game.Base.Packets;
using Game.Server.Rooms;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(109, "房间列表")]
  public class GameRoomListHandler : IPacketHandler
  {
    public const int numberRoomInPage = 100;

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      packet.ReadInt();
      packet.ReadInt();
      int num1 = packet.ReadInt();
      if (num1 < 0)
        num1 = 0;
      int num2 = num1 + 1;
      BaseRoom[] rooms = RoomMgr.Rooms;
      List<BaseRoom> room = new List<BaseRoom>();
      int num3 = 0;
      for (int index = 0; index < rooms.Length; ++index)
      {
        if (!rooms[index].IsEmpty)
        {
          ++num3;
          if (num3 < num2 * 100 && num3 > (num2 - 1) * 100)
            room.Add(rooms[index]);
          else if (num3 > num2 * 100)
            break;
        }
      }
      if (room.Count > 0)
        client.Out.SendUpdateRoomList(room);
      return 0;
    }
  }
}
