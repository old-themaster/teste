// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.CardResetHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll
using Game.Bussiness;
using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(196, "防沉迷系统开关")]
  public class CardResetHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      int slot = packet.ReadInt();
      RandomSafe randomSafe = new RandomSafe();
      CardInventory cardBag = client.Player.CardBag;
      if (slot < 5)
        return 0;
      UsersCardInfo itemAt = cardBag.GetItemAt(slot);
      if (itemAt == null)
        return 0;
      if (num == 0)
      {
        CardUpdateConditionInfo cardUpdateCondition = CardMgr.GetCardUpdateCondition(itemAt.Level);
        if (cardUpdateCondition != null && cardUpdateCondition.ResetCardCount > 0)
        {
          bool flag = false;
          if (itemAt.Count >= cardUpdateCondition.ResetCardCount)
          {
            flag = true;
            itemAt.Count -= cardUpdateCondition.ResetCardCount;
            cardBag.UpdateCard(itemAt);
          }
          else if (client.Player.RemoveMoney(cardUpdateCondition.ResetMoney) > 0)
            flag = true;
          if (flag)
          {
            int[] numArray = new int[4];
            int max1 = randomSafe.NextSmallValue(6, 50);
            numArray[0] = randomSafe.NextSmallValue(5, max1);
            int max2 = randomSafe.NextSmallValue(6, 50);
            numArray[1] = randomSafe.NextSmallValue(5, max2);
            int max3 = randomSafe.NextSmallValue(6, 50);
            numArray[2] = randomSafe.NextSmallValue(5, max3);
            int max4 = randomSafe.NextSmallValue(6, 50);
            numArray[3] = randomSafe.NextSmallValue(5, max4);
            if (client.Player.CardResetTempProp.ContainsKey(itemAt.TemplateID))
              client.Player.CardResetTempProp[itemAt.TemplateID] = numArray;
            else
              client.Player.CardResetTempProp.Add(itemAt.TemplateID, numArray);
            GSPacketIn pkg = new GSPacketIn((short) 196);
            pkg.WriteInt(4);
            pkg.WriteInt(itemAt.Attack + numArray[0]);
            pkg.WriteInt(itemAt.Defence + numArray[1]);
            pkg.WriteInt(itemAt.Agility + numArray[2]);
            pkg.WriteInt(itemAt.Luck + numArray[3]);
            client.SendTCP(pkg);
          }
          else
            client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.CardReset.Msg2"));
        }
        else
          client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.CardReset.Msg3"));
      }
      else if (client.Player.CardResetTempProp.ContainsKey(itemAt.TemplateID))
      {
        int[] numArray = client.Player.CardResetTempProp[itemAt.TemplateID];
        itemAt.AttackReset = numArray[0];
        itemAt.DefenceReset = numArray[1];
        itemAt.AgilityReset = numArray[2];
        itemAt.LuckReset = numArray[3];
        UsersCardInfo cardEquip = cardBag.GetCardEquip(itemAt.TemplateID);
        if (cardEquip != null)
        {
          cardEquip.CopyProp(itemAt);
          cardBag.UpdateCard(cardEquip);
          client.Player.EquipBag.UpdatePlayerProperties();
        }
        cardBag.UpdateCard(itemAt);
        client.Player.CardResetTempProp.Remove(itemAt.TemplateID);
        client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.CardReset.Msg1"));
      }
      return 0;
    }
  }
}
