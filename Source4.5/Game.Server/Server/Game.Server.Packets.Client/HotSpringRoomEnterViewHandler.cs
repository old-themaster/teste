﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringRoomEnterViewHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler(201, "礼堂数据")]
  public class HotSpringRoomEnterViewHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentHotSpringRoom != null)
      {
        foreach (GamePlayer allPlayer in client.Player.CurrentHotSpringRoom.GetAllPlayers())
        {
          GSPacketIn pkg = new GSPacketIn((short) 198);
          pkg.WriteInt(allPlayer.PlayerCharacter.ID);
          pkg.WriteInt(allPlayer.PlayerCharacter.Grade);
          pkg.WriteInt(allPlayer.PlayerCharacter.Hide);
          pkg.WriteInt(allPlayer.PlayerCharacter.Repute);
          pkg.WriteString(allPlayer.PlayerCharacter.NickName);
          pkg.WriteByte(allPlayer.PlayerCharacter.typeVIP);
          pkg.WriteInt(allPlayer.PlayerCharacter.VIPLevel);
          pkg.WriteBoolean(allPlayer.PlayerCharacter.Sex);
          pkg.WriteString(allPlayer.PlayerCharacter.Style);
          pkg.WriteString(allPlayer.PlayerCharacter.Colors);
          pkg.WriteString(allPlayer.PlayerCharacter.Skin);
          pkg.WriteInt(allPlayer.Hot_X);
          pkg.WriteInt(allPlayer.Hot_Y);
          pkg.WriteInt(allPlayer.PlayerCharacter.FightPower);
          pkg.WriteInt(allPlayer.PlayerCharacter.Win);
          pkg.WriteInt(allPlayer.PlayerCharacter.Total);
          pkg.WriteInt(allPlayer.Hot_Direction);
          client.Player.SendTCP(pkg);
        }
      }
      return 0;
    }
  }
}
