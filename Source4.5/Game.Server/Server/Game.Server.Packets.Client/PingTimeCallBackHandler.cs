// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PingTimeCallBackHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(4, "测试网络")]
  public class PingTimeCallBackHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      client.Player.PingTime = DateTime.Now.Ticks - client.Player.PingStart;
      return 0;
    }
  }
}
