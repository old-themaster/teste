// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SwitchTeam
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(102, "Change Team")]
  public class SwitchTeam : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null)
      {
        int num = (int) packet.ReadByte();
      }
      return 0;
    }
  }
}
