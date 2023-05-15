// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ChangeColorShellTimeOverHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(206, "场景用户离开")]
  public class ChangeColorShellTimeOverHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = (int) packet.ReadByte();
      packet.ReadInt();
      return 0;
    }
  }
}
