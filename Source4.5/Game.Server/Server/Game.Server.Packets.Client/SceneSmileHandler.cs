﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SceneSmileHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(20, "用户场景表情")]
  public class SceneSmileHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      packet.ClientID = client.Player.PlayerCharacter.ID;
      if (client.Player.CurrentRoom != null)
        client.Player.CurrentRoom.SendToAll(packet);
      else if (client.Player.CurrentMarryRoom != null)
        client.Player.CurrentMarryRoom.SendToAllForScene(packet, client.Player.MarryMap);
      else if (client.Player.CurrentHotSpringRoom != null)
        client.Player.CurrentHotSpringRoom.SendToAll(packet);
      else
        RoomMgr.WaitingRoom.method_0(packet);
      return 1;
    }
  }
}
