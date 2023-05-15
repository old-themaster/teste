// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ACActionHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [Obsolete("已经不用")]
  [PacketHandler(35, "user ac action")]
  public class ACActionHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet) => 1;
  }
}
