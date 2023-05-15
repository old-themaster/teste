// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.StoreItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(79, "储存物品")]
  public class StoreItemHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.ConsortiaID == 0)
        return 1;
      int num1 = (int) packet.ReadByte();
            int num2 = packet.ReadInt();
      packet.ReadInt();
      if (num1 == 0 && num2 < 31)
        return 1;
      if (ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID) != null)
      {
        PlayerInventory consortiaBag = client.Player.ConsortiaBag;
        client.Player.GetInventory((eBageType) num1);
      }
      return 0;
    }
  }
}
