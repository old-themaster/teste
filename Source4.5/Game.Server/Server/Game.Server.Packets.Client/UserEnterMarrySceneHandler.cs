﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserEnterMarrySceneHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;
using Game.Server.SceneMarryRooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(240, "Player enter marry scene.")]
  public class UserEnterMarrySceneHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn packet1 = new GSPacketIn((short) 240, client.Player.PlayerCharacter.ID);
      if (WorldMgr.MarryScene.AddPlayer(client.Player))
        packet1.WriteBoolean(true);
      else
        packet1.WriteBoolean(false);
      client.Out.SendTCP(packet1);
      if (client.Player.CurrentMarryRoom == null)
      {
        foreach (MarryRoom room in MarryRoomMgr.GetAllMarryRoom())
          client.Player.Out.SendMarryRoomInfo(client.Player, room);
      }
      return 0;
    }
  }
}
