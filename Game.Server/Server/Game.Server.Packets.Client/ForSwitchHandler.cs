// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ForSwitchHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(225, "场景用户离开")]
  public class ForSwitchHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn pkg = new GSPacketIn((short) 225, client.Player.PlayerCharacter.ID);
      client.SendTCP(pkg);
      return 0;
    }
  }
}
