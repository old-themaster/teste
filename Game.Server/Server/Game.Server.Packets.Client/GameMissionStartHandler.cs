using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(82, "游戏开始")]
  public class GameMissionStartHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      BaseRoom currentRoom = client.Player.CurrentRoom;
      if (flag && currentRoom != null)
        RoomMgr.StartGameMission(currentRoom);
      else
        client.Player.SendMessage("Erro no sistema. pronto:" + flag.ToString() + " |" + currentRoom?.ToString() == null ? "roomNULL" : currentRoom.RoomId.ToString());
      return 0;
    }
  }
}
