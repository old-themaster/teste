// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ChangeDesignationHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(34, "场景用户离开")]
  public class ChangeDesignationHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      client.Player.PlayerCharacter.IsShowConsortia = flag;
      return 0;
    }
  }
}
