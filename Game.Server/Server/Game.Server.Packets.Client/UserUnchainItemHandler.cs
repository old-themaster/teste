// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserUnchainItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(47, "解除物品")]
  public class UserUnchainItemHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom == null || !client.Player.CurrentRoom.IsPlaying)
      {
        int fromSlot = packet.ReadInt();
        int firstEmptySlot = client.Player.EquipBag.FindFirstEmptySlot(31);
        client.Player.EquipBag.MoveItem(fromSlot, firstEmptySlot, 0);
      }
      return 0;
    }
  }
}
