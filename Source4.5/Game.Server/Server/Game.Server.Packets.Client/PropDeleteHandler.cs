// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PropDeleteHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(75, "删除道具")]
  public class PropDeleteHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int place = packet.ReadInt();
      client.Player.FightBag.RemoveItemAt(place);
      return 0;
    }
  }
}
