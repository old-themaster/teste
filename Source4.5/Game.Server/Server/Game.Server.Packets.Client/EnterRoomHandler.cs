// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.EnterRoomHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(81, "游戏创建")]
  public class EnterRoomHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool isInvite = packet.ReadBoolean();
      int type = packet.ReadInt();
      int num = packet.ReadInt();
      int roomId = -1;
      string pwd = (string) null;
      if (num == -1)
      {
        roomId = packet.ReadInt();
        pwd = packet.ReadString();
      }
      switch (type)
      {
        case 1:
          type = 0;
          break;
        case 2:
          type = 4;
          break;
      }
      RoomMgr.EnterRoom(client.Player, roomId, pwd, type, isInvite);
      return 0;
    }
  }
}
