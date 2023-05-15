﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarrySceneChangeHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler(233, "结婚场景切换")]
  internal class MarrySceneChangeHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentMarryRoom == null || client.Player.MarryMap == 0)
        return 1;
      int num = packet.ReadInt();
      if (num == client.Player.MarryMap)
        return 1;
      GSPacketIn packet1 = new GSPacketIn((short) 244, client.Player.PlayerCharacter.ID);
      client.Player.CurrentMarryRoom.SendToPlayerExceptSelfForScene(packet1, client.Player);
      client.Player.MarryMap = num;
      switch (num)
      {
        case 1:
          client.Player.X = 514;
          client.Player.Y = 637;
          break;
        case 2:
          client.Player.X = 800;
          client.Player.Y = 763;
          break;
      }
      foreach (GamePlayer allPlayer in client.Player.CurrentMarryRoom.GetAllPlayers())
      {
        if (allPlayer != client.Player && allPlayer.MarryMap == client.Player.MarryMap)
        {
          allPlayer.Out.SendPlayerEnterMarryRoom(client.Player);
          client.Player.Out.SendPlayerEnterMarryRoom(allPlayer);
        }
      }
      return 0;
    }
  }
}
