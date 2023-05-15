// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemCompareHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(119, "物品比较")]
  public class ItemCompareHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (packet.ReadInt() != 2)
        return 0;
      int itemID = packet.ReadInt();
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(itemID);
        if (userItemSingle != null)
        {
          GSPacketIn packet1 = new GSPacketIn((short) 119, client.Player.PlayerCharacter.ID);
          packet1.WriteInt(userItemSingle.TemplateID);
          packet1.WriteInt(userItemSingle.ItemID);
          packet1.WriteInt(userItemSingle.StrengthenLevel);
          packet1.WriteInt(userItemSingle.AttackCompose);
          packet1.WriteInt(userItemSingle.AgilityCompose);
          packet1.WriteInt(userItemSingle.LuckCompose);
          packet1.WriteInt(userItemSingle.DefendCompose);
          packet1.WriteInt(userItemSingle.ValidDate);
          packet1.WriteBoolean(userItemSingle.IsBinds);
          packet1.WriteBoolean(userItemSingle.IsJudge);
          packet1.WriteBoolean(userItemSingle.IsUsed);
          if (userItemSingle.IsUsed)
            packet1.WriteString(userItemSingle.BeginDate.ToString());
          packet1.WriteInt(userItemSingle.Hole1);
          packet1.WriteInt(userItemSingle.Hole2);
          packet1.WriteInt(userItemSingle.Hole3);
          packet1.WriteInt(userItemSingle.Hole4);
          packet1.WriteInt(userItemSingle.Hole5);
          packet1.WriteInt(userItemSingle.Hole6);
          packet1.WriteString(userItemSingle.Template.Hole);
          client.Out.SendTCP(packet1);
        }
        return 1;
      }
    }
  }
}
