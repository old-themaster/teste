

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
    [PacketHandler(156, "客户端日记")]
    public class BuyTransnationalGoodsHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int ID = packet.ReadInt();
            PyramidInfo pyramid = client.Player.Actives.Pyramid;
            int gold = 0;
            int money = 0;
            int offer = 0;
            int gifttoken = 0;
            int medal = 0;
            int Ascension = 0;
            int damageScore = 0;
            int petScore = 0;
            int iTemplateID = 0;
            int iCount = 0;
            int hardCurrency = 0;
            int LeagueMoney = 0;
            int useableScore = 0;
            eMessageType type = eMessageType.Normal;
            string translateId = "UserBuyItemHandler.Success";
            ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(ID);
            bool flag = false;
            if (shopItemInfoById != null && ShopMgr.IsOnShop(shopItemInfoById.ID) && shopItemInfoById.ShopID == 98)
                flag = true;
            if (!flag)
            {
                client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission"));
                return 1;
            }
            Dictionary<int, ItemInfo> dictionary = new Dictionary<int, ItemInfo>();
            ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID), 1, 102);
            if (shopItemInfoById.BuyType == 0)
                fromTemplate.ValidDate = shopItemInfoById.AUnit;
            else
                fromTemplate.Count = shopItemInfoById.AUnit;
            fromTemplate.IsBinds = true;
            if (!dictionary.Keys.Contains<int>(fromTemplate.TemplateID))
                dictionary.Add(fromTemplate.TemplateID, fromTemplate);
            else
                dictionary[fromTemplate.TemplateID].Count += fromTemplate.Count;
            ShopMgr.SetItemType(shopItemInfoById, 1, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref Ascension, ref hardCurrency, ref LeagueMoney, ref useableScore);
            if (dictionary.Values.Count == 0)
                return 1;
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }
            if (pyramid.totalPoint < damageScore)
            {
                client.Player.SendMessage("Não acumulação suficiente.");
                return 0;
            }
            pyramid.totalPoint -= damageScore;
            if (true)
            {
                string str1 = "";
                foreach (ItemInfo itemInfo in dictionary.Values)
                {
                    string str2 = str1;
                    int templateId;
                    string str3;
                    if (!(str1 == ""))
                    {
                        templateId = itemInfo.TemplateID;
                        str3 = "," + templateId.ToString();
                    }
                    else
                    {
                        templateId = itemInfo.TemplateID;
                        str3 = templateId.ToString();
                    }
                    str1 = str2 + str3;
                    if (fromTemplate.Template.MaxCount == 1)
                    {
                        for (int index = 0; index < fromTemplate.Count; ++index)
                        {
                            ItemInfo cloneItem = ItemInfo.CloneFromTemplate(fromTemplate.Template, fromTemplate);
                            cloneItem.Count = 1;
                            client.Player.AddTemplate(cloneItem);
                        }
                    }
                    else
                    {
                        int num = 0;
                        for (int index = 0; index < fromTemplate.Count; ++index)
                        {
                            if (num == fromTemplate.Template.MaxCount)
                            {
                                ItemInfo cloneItem = ItemInfo.CloneFromTemplate(fromTemplate.Template, fromTemplate);
                                cloneItem.Count = num;
                                client.Player.AddTemplate(cloneItem);
                                num = 0;
                            }
                            ++num;
                        }
                        if (num > 0)
                        {
                            ItemInfo cloneItem = ItemInfo.CloneFromTemplate(fromTemplate.Template, fromTemplate);
                            cloneItem.Count = num;
                            client.Player.AddTemplate(cloneItem);
                        }
                    }
                }
            }
            else
            {
                type = eMessageType.ERROR;
                translateId = "UserBuyItemHandler.FailByPermission";
            }
            client.Out.SendMessage(type, LanguageMgr.GetTranslation(translateId));
            GSPacketIn pkg = new GSPacketIn((short)145, client.Player.PlayerCharacter.ID);
            pkg.WriteByte((byte)2);
            pkg.WriteBoolean(pyramid.isPyramidStart);
            pkg.WriteInt(pyramid.totalPoint);
            pkg.WriteInt(pyramid.turnPoint);
            pkg.WriteInt(pyramid.pointRatio);
            pkg.WriteInt(pyramid.currentLayer);
            client.Player.SendTCP(pkg);
            return 0;
        }
    }
}
