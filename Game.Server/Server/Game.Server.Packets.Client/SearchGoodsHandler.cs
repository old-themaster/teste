// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SearchGoodsHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(98, "客户端日记")]
  public class SearchGoodsHandler : IPacketHandler
  {
    private readonly int[] mapID = new int[3]{ 1, 2, 3 };
    private static ThreadSafeRandom rand = new ThreadSafeRandom();

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte index = packet.ReadByte();
      if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
        RoomMgr.KickPlayer(client.Player.CurrentRoom, index);
      return 0;
    }
  }
}
