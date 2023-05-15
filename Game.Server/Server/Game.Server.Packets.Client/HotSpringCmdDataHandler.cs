// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringCmdDataHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(191, "礼堂数据")]
  public class HotSpringCmdDataHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentHotSpringRoom != null)
        client.Player.CurrentHotSpringRoom.ProcessData(client.Player, packet);
      return 0;
    }
  }
}
