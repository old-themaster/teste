// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuickBuyGoldBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(126, "场景用户离开")]
  public class QuickBuyGoldBoxHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      packet.ReadBoolean();
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 1;
      }
      ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(1123301);
      ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID);
      int num2 = num1 * shopItemInfoById.AValue1;
      if (client.Player.MoneyDirect(num2, false))
      {
        int point = 0;
        int gold = 0;
        int giftToken = 0;
        int medal = 0;
        int Ascension = 0;
        int exp = 0;
        List<ItemInfo> itemInfos = new List<ItemInfo>();
        ItemBoxMgr.CreateItemBox(itemTemplate.TemplateID, itemInfos, ref gold, ref point, ref giftToken, ref medal, ref Ascension, ref exp);
        int num3 = num1 * gold;
        client.Player.AddGold(num3);
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Você ganhou " + (object) num3 + " moedas de ouro."));
      }
      return 0;
    }
  }
}
