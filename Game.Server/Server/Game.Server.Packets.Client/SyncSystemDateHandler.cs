// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SyncSystemDateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(5, "同步系统数据")]
  public class SyncSystemDateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      packet.ClearContext();
      packet.WriteDateTime(DateTime.Now);
      client.Out.SendTCP(packet);
      return 0;
    }
  }
}
