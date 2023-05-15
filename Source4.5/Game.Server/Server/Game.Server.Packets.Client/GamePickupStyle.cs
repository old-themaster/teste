﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GamePickupStyle
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(211, "Change style of room (mode) 0 = pvp, 1 = guild")]
  public class GamePickupStyle : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host && !client.Player.CurrentRoom.IsPlaying)
      {
        int num = packet.ReadInt();
        client.Player.CurrentRoom.GameStyle = num;
        GSPacketIn pkg = client.Player.Out.SendRoomType(client.Player, client.Player.CurrentRoom);
        client.Player.CurrentRoom.SendToAll(pkg, client.Player);
      }
      return 0;
    }
  }
}
