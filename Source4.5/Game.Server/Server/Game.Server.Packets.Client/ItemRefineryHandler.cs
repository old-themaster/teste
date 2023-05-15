// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemRefineryHandler
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B480679-DF24-46B7-834E-821AA9A4FB3F
// Assembly location: C:\Users\Anderson\Desktop\Source 4.2\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
  [PacketHandler(210, "物品炼化")]
  public class ItemRefineryHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn packet1 = packet.Clone();
      packet1.ClearContext();
      bool isbind = false;
      int OpertionType = packet.ReadInt();
      int num1 = packet.ReadInt();
      List<ItemInfo> Items = new List<ItemInfo>();
      List<ItemInfo> itemInfoList = new List<ItemInfo>();
      List<eBageType> eBageTypeList = new List<eBageType>();
      StringBuilder stringBuilder1 = new StringBuilder();
      int defaultprobability = 25;
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.locked"));
        return 1;
      }
      for (int index = 0; index < num1; ++index)
      {
        eBageType bagType = (eBageType) packet.ReadInt();
        int place = packet.ReadInt();
        ItemInfo itemAt = client.Player.GetItemAt(bagType, place);
        if (itemAt != null)
        {
          if (!Items.Contains(itemAt))
          {
            if (itemAt.IsBinds)
              isbind = true;
            stringBuilder1.Append(itemAt.ItemID.ToString() + ":" + itemAt.TemplateID.ToString() + ",");
            Items.Add(itemAt);
            eBageTypeList.Add(bagType);
          }
          else
          {
            client.Out.SendMessage(eMessageType.GM_NOTICE, "Bad Input");
            return 1;
          }
        }
      }
      eBageType bagType1 = (eBageType) packet.ReadInt();
      int place1 = packet.ReadInt();
      ItemInfo itemAt1 = client.Player.GetItemAt(bagType1, place1);
      int num2;
      if (itemAt1 != null)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        num2 = itemAt1.ItemID;
        string str1 = num2.ToString();
        num2 = itemAt1.TemplateID;
        string str2 = num2.ToString();
        string str3 = str1 + ":" + str2 + ",";
        stringBuilder2.Append(str3);
      }
      eBageType bagType2 = (eBageType) packet.ReadInt();
      int place2 = packet.ReadInt();
      ItemInfo itemAt2 = client.Player.GetItemAt(bagType2, place2);
      bool Luck = itemAt2 != null;
      if (num1 != 4 || itemAt1 == null)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemRefineryHandler.ItemNotEnough"));
        return 1;
      }
      bool result = false;
      bool IsFormula = false;
      if (OpertionType == 0)
      {
        ItemTemplateInfo itemTemplateInfo = RefineryMgr.Refinery(client.Player, Items, itemAt1, Luck, OpertionType, ref result, ref defaultprobability, ref IsFormula);
        if (itemTemplateInfo != null)
          client.Out.SendRefineryPreview(client.Player, itemTemplateInfo.TemplateID, isbind, itemAt1);
        return 0;
      }
      int num3 = 10000;
      if (client.Player.PlayerCharacter.Gold > num3)
      {
        client.Player.RemoveGold(num3);
        ItemTemplateInfo itemTemplateInfo = RefineryMgr.Refinery(client.Player, Items, itemAt1, Luck, OpertionType, ref result, ref defaultprobability, ref IsFormula);
        if (itemTemplateInfo != null & IsFormula & result)
        {
          stringBuilder1.Append("Success");
          if (true)
          {
            ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(itemTemplateInfo, 1, 114);
            if (fromTemplate != null)
            {
              client.Player.OnItemMelt(itemAt1.Template.CategoryID);
              fromTemplate.IsBinds = isbind;
              AbstractInventory itemInventory = (AbstractInventory) client.Player.GetItemInventory(itemTemplateInfo);
              if (!itemInventory.AddItem(fromTemplate, itemInventory.BeginSlot))
              {
                stringBuilder1.Append("NoPlace");
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(fromTemplate.GetBagName()) + LanguageMgr.GetTranslation("ItemFusionHandler.NoPlace"));
              }
              packet1.WriteByte((byte) 0);
              num2 = itemAt1.Count--;
              client.Player.UpdateItem(itemAt1);
            }
          }
          else
            stringBuilder1.Append("false");
        }
        else
          packet1.WriteByte((byte) 1);
        if (Luck)
        {
          num2 = itemAt2.Count--;
          client.Player.UpdateItem(itemAt2);
        }
        for (int index = 0; index < Items.Count; ++index)
        {
          client.Player.UpdateItem(Items[index]);
          if (Items[index].Count <= 0)
            client.Player.RemoveItem(Items[index]);
        }
        client.Player.RemoveItem(Items[Items.Count - 1]);
        client.Player.Out.SendTCP(packet1);
      }
      else
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemRefineryHandler.NoGold"));
      return 1;
    }
  }
}
