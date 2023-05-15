// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserBuyItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(44, "购买物品")]
    public class UserBuyItemHandler : IPacketHandler
    {
        public static int countConnect = 0;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (UserBuyItemHandler.countConnect >= 3000)
            {
                client.Disconnect();
                return 0;
            }
            int gold = 0;
            int money = 0;
            int offer = 0;
            int gifttoken = 0;
            StringBuilder stringBuilder1 = new StringBuilder();
            eMessageType type1 = eMessageType.GM_NOTICE;
            string translateId1 = "UserBuyItemHandler.Success";
            GSPacketIn pkg = new GSPacketIn((short)44, client.Player.PlayerCharacter.ID);
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            List<bool> boolList = new List<bool>();
            List<int> intList1 = new List<int>();
            StringBuilder stringBuilder2 = new StringBuilder();
            bool isBinds = true;
            ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
            int num1 = packet.ReadInt();
            if (num1 > 0 && num1 <= 99)
            {
                List<string> stringList = new List<string>();
                for (int index1 = 0; index1 < num1; ++index1)
                {
                    int ID = packet.ReadInt();
                    int type2 = packet.ReadInt();
                    string str1 = packet.ReadString();
                    bool flag = packet.ReadBoolean();
                    string str2 = packet.ReadString();
                    int num2 = packet.ReadInt();
                    stringList.Add(ID.ToString());
                    ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(ID);
                    if (shopItemInfoById != null && ShopMgr.IsOnShop(shopItemInfoById.ID))
                    {
                        if (shopItemInfoById.ShopID != 2 && ShopMgr.CanBuy(shopItemInfoById.ShopID, consortiaInfo == null ? 1 : consortiaInfo.ShopLevel, ref isBinds, client.Player.PlayerCharacter.ConsortiaID, client.Player.PlayerCharacter.Riches))
                        {
                            if (shopItemInfoById.ShopID == 20)
                            {
                                if (!(client.Player.PlayerCharacter.ShopFinallyGottenTime.Date == DateTime.Now.Date) && WorldMgr.UpdateShopFreeCount(shopItemInfoById.ID, shopItemInfoById.LimitCount))
                                {
                                    List<ShopFreeCountInfo> allShopFreeCount = WorldMgr.GetAllShopFreeCount();
                                    client.Out.SendShopGoodsCountUpdate(allShopFreeCount);
                                    client.Player.PlayerCharacter.ShopFinallyGottenTime = DateTime.Now.Date;
                                    dictionary.Add(-9999, 1);
                                    string translation = LanguageMgr.GetTranslation("GameServer.FreeItem.Notice.Msg", client.Player.PlayerCharacter.NickName, shopItemInfoById.TemplateID);
                                    GSPacketIn packet2 = WorldMgr.SendSysNotice(eMessageType.ChatNormal, translation, 0, shopItemInfoById.TemplateID, null);
                                    GameServer.Instance.LoginServer.SendPacket(packet2);
                                }
                                else
                                {
                                    client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission2"));
                                    return 1;
                                }
                            }
                            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID), 1, 102);
                            if (shopItemInfoById.BuyType == 0)
                            {
                                if (1 == type2)
                                    fromTemplate.ValidDate = shopItemInfoById.AUnit;
                                if (2 == type2)
                                    fromTemplate.ValidDate = shopItemInfoById.BUnit;
                                if (3 == type2)
                                    fromTemplate.ValidDate = shopItemInfoById.CUnit;
                            }
                            else
                            {
                                if (1 == type2)
                                    fromTemplate.Count = shopItemInfoById.AUnit;
                                if (2 == type2)
                                    fromTemplate.Count = shopItemInfoById.BUnit;
                                if (3 == type2)
                                    fromTemplate.Count = shopItemInfoById.CUnit;
                            }

                            if (fromTemplate != null || shopItemInfoById != null)
                            {
                                fromTemplate.Color = str1 == null ? "" : str1;
                                fromTemplate.Skin = str2 == null ? "" : str2;
                                fromTemplate.IsBinds = isBinds || Convert.ToBoolean(shopItemInfoById.IsBind);
                                fromTemplate.IsBinds = true;
                                stringBuilder2.Append(type2);
                                stringBuilder2.Append(",");
                                itemInfoList.Add(fromTemplate);
                                boolList.Add(flag);
                                intList1.Add(num2);
                                List<int> intList2 = SqlDataProvider.Data.ItemInfo.SetItemType(shopItemInfoById, type2, ref gold, ref money, ref offer, ref gifttoken);
                                for (int index2 = 0; index2 < intList2.Count; index2 += 2)
                                {
                                    if (dictionary.ContainsKey(intList2[index2]))
                                        dictionary[intList2[index2]] += intList2[index2 + 1];
                                    else
                                        dictionary.Add(intList2[index2], intList2[index2 + 1]);
                                }
                            }
                        }
                        else
                        {
                            client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission"));
                            return 1;
                        }
                    }
                }
                if (itemInfoList.Count == 0)
                    return 1;
                if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
                {
                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                    return 1;
                }
                bool flag1 = true;
                foreach (KeyValuePair<int, int> keyValuePair in dictionary)
                {
                    if (keyValuePair.Key != -9999 && client.Player.GetTemplateCount(keyValuePair.Key) < keyValuePair.Value)
                        flag1 = false;
                }
                if (!flag1)
                {
                    string translateId2 = "UserBuyItemHandler.NoBuyItem";
                    client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation(translateId2));
                    return 1;
                }
                if (gold >= 0 && money >= 0 && (offer >= 0 && gifttoken >= 0) && (gold > 0 || money > 0 || (offer > 0 || gifttoken > 0) || dictionary.Count > 0))
                {
                    int num2 = client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock;
                    if (gold <= client.Player.PlayerCharacter.Gold && money <= client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock && (offer <= client.Player.PlayerCharacter.Offer && gifttoken <= client.Player.PlayerCharacter.GiftToken))
                    {
                        client.Player.RemoveMoney(money);
                        client.Player.RemoveGold(gold);
                        client.Player.RemoveOffer(offer);
                        client.Player.RemoveGiftToken(gifttoken);
                        int num3 = client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock;
                        string str1 = string.Format("money: {0} | gold: {1} | offter: {2} | gifttoken: {3} | moneyBefore: {4} | moneyAfter: {5}", (object)money, (object)gold, (object)offer, (object)gifttoken, (object)num2, (object)num3);
                        if (money > 0 && client.Player.Extra.CheckNoviceActiveOpen(NoviceActiveType.USE_MONEY_ACTIVE))
                            client.Player.Extra.UpdateEventCondition(3, money, true, 0);

                        foreach (KeyValuePair<int, int> keyValuePair in dictionary)
                        {
                            if (keyValuePair.Key != -9999)
                                client.Player.RemoveTemplateInShop(keyValuePair.Key, keyValuePair.Value);
                            stringBuilder1.Append(keyValuePair.Key.ToString() + ",");
                        }
                        if (dictionary.Count > 0)
                            client.Player.UpdateProperties();
                        string str2 = str1 + " | itemNeed: " + string.Join<int>(",", (IEnumerable<int>)dictionary.Keys.ToArray<int>());
                        string str3 = "";
                        int num4 = 0;
                        MailInfo mail = new MailInfo();
                        StringBuilder stringBuilder3 = new StringBuilder();
                        stringBuilder3.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark"));
                        for (int index = 0; index < itemInfoList.Count; ++index)
                        {
                            string str4 = str3;
                            int num5;
                            string str5;
                            if (!(str3 == ""))
                            {
                                num5 = itemInfoList[index].TemplateID;
                                str5 = "," + num5.ToString();
                            }
                            else
                                str5 = itemInfoList[index].TemplateID.ToString();
                            string str6 = str5;
                            str3 = str4 + str6;
                            if (client.Player.AddTemplate(itemInfoList[index], itemInfoList[index].Template.BagType, itemInfoList[index].Count, false))
                            {
                                if (boolList[index] && itemInfoList[index].CanEquip())
                                {
                                    int itemEpuipSlot = client.Player.EquipBag.FindItemEpuipSlot(itemInfoList[index].Template);
                                    if (itemEpuipSlot != 9 && itemEpuipSlot != 10 || intList1[index] != 9 && intList1[index] != 10)
                                    {
                                        if ((itemEpuipSlot == 7 || itemEpuipSlot == 8) && (intList1[index] == 7 || intList1[index] == 8))
                                            itemEpuipSlot = intList1[index];
                                    }
                                    else
                                        itemEpuipSlot = intList1[index];
                                    client.Player.EquipBag.MoveItem(itemInfoList[index].Place, itemEpuipSlot, 0);
                                    translateId1 = "UserBuyItemHandler.Save";
                                }
                            }
                            else
                            {
                                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                                {
                                    itemInfoList[index].UserID = 0;
                                    playerBussiness.AddGoods(itemInfoList[index]);
                                    ++num4;
                                    stringBuilder3.Append(num4);
                                    stringBuilder3.Append("、");
                                    stringBuilder3.Append(itemInfoList[index].Template.Name);
                                    stringBuilder3.Append("x");
                                    stringBuilder3.Append(itemInfoList[index].Count);
                                    stringBuilder3.Append(";");
                                    switch (num4 - 1)
                                    {
                                        case 0:
                                            MailInfo mailInfo1 = mail;
                                            num5 = itemInfoList[index].ItemID;
                                            string str7 = num5.ToString();
                                            mailInfo1.Annex1 = str7;
                                            mail.Annex1Name = itemInfoList[index].Template.Name;
                                            break;
                                        case 1:
                                            MailInfo mailInfo2 = mail;
                                            num5 = itemInfoList[index].ItemID;
                                            string str8 = num5.ToString();
                                            mailInfo2.Annex2 = str8;
                                            mail.Annex2Name = itemInfoList[index].Template.Name;
                                            break;
                                        case 2:
                                            MailInfo mailInfo3 = mail;
                                            num5 = itemInfoList[index].ItemID;
                                            string str9 = num5.ToString();
                                            mailInfo3.Annex3 = str9;
                                            mail.Annex3Name = itemInfoList[index].Template.Name;
                                            break;
                                        case 3:
                                            MailInfo mailInfo4 = mail;
                                            num5 = itemInfoList[index].ItemID;
                                            string str10 = num5.ToString();
                                            mailInfo4.Annex4 = str10;
                                            mail.Annex4Name = itemInfoList[index].Template.Name;
                                            break;
                                        case 4:
                                            MailInfo mailInfo5 = mail;
                                            num5 = itemInfoList[index].ItemID;
                                            string str11 = num5.ToString();
                                            mailInfo5.Annex5 = str11;
                                            mail.Annex5Name = itemInfoList[index].Template.Name;
                                            break;
                                    }
                                    if (num4 == 5)
                                    {
                                        num4 = 0;
                                        mail.AnnexRemark = stringBuilder3.ToString();
                                        stringBuilder3.Remove(0, stringBuilder3.Length);
                                        stringBuilder3.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark"));
                                        mail.Content = LanguageMgr.GetTranslation("UserBuyItemHandler.Title") + mail.Annex1Name + "]";
                                        mail.Gold = 0;
                                        mail.Money = 0;
                                        mail.Receiver = client.Player.PlayerCharacter.NickName;
                                        mail.ReceiverID = client.Player.PlayerCharacter.ID;
                                        mail.Sender = mail.Receiver;
                                        mail.SenderID = mail.ReceiverID;
                                        mail.Title = mail.Content;
                                        mail.Type = 8;
                                        playerBussiness.SendMail(mail);
                                        type1 = eMessageType.BIGBUGLE_NOTICE;
                                        translateId1 = "UserBuyItemHandler.Mail";
                                        mail.Revert();
                                    }
                                }
                            }
                        }
                        string content = str2 + " | listsBuy: " + str3;
                        if (num4 > 0)
                        {
                            using (PlayerBussiness playerBussiness = new PlayerBussiness())
                            {
                                mail.AnnexRemark = stringBuilder3.ToString();
                                mail.Content = LanguageMgr.GetTranslation("UserBuyItemHandler.Title") + mail.Annex1Name + "]";
                                mail.Gold = 0;
                                mail.Money = 0;
                                mail.Receiver = client.Player.PlayerCharacter.NickName;
                                mail.ReceiverID = client.Player.PlayerCharacter.ID;
                                mail.Sender = mail.Receiver;
                                mail.SenderID = mail.ReceiverID;
                                mail.Title = mail.Content;
                                mail.Type = 8;
                                playerBussiness.SendMail(mail);
                                type1 = eMessageType.BIGBUGLE_NOTICE;
                                translateId1 = "UserBuyItemHandler.Mail";
                            }
                        }
                        if (type1 == eMessageType.BIGBUGLE_NOTICE)
                            client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
                        client.Player.OnPaid(money, gold, offer, gifttoken, 0, 0, stringBuilder1.ToString());
                        client.Player.AddLog("Buy Shop", content);
                    }
                    else
                    {
                        if (gold > client.Player.PlayerCharacter.Gold)
                            translateId1 = "UserBuyItemHandler.NoGold";
                        if (offer > client.Player.PlayerCharacter.Offer)
                            translateId1 = "UserBuyItemHandler.NoOffer";
                        if (gifttoken > client.Player.PlayerCharacter.GiftToken)
                            translateId1 = "UserBuyItemHandler.GiftToken";
                        type1 = eMessageType.BIGBUGLE_NOTICE;
                    }
                    client.Out.SendMessage(type1, LanguageMgr.GetTranslation(translateId1));
                    pkg.WriteInt(1);
                    pkg.WriteInt(3);
                    client.Player.SendTCP(pkg);
                    return 0;
                }
                client.Player.SendMessage("Erro no sistema. O problema foi enviado para um administrador.");
                log.Error((object)("username: " + client.Player.PlayerCharacter.UserName + " - hack money down."));
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                foreach (GamePlayer gamePlayer in allPlayers)
                {
                    if (ComandosMgr.checkStaff(gamePlayer.PlayerCharacter.ID))
                    {
                        GSPacketIn gSPacketIn = WorldMgr.SendStaffHelp("O jogador: [" + client.Player.PlayerCharacter.NickName + "] está tentando usar HACK UseBuyItemHandler");
                        gamePlayer.SendTCP(gSPacketIn);
                    }
                }
                return 0;
            }
            client.Player.SendMessage("Erro no sistema. O problema foi enviado para um administrador.");
            log.Error((object)("username: " + client.Player.PlayerCharacter.UserName + " - hack money down (count: " + num1 + ")."));
            GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
            foreach (GamePlayer gamePlayer2 in allPlayers2)
            {
                if (ComandosMgr.checkStaff(gamePlayer2.PlayerCharacter.ID))
                {
                    GSPacketIn pkg2 = WorldMgr.SendStaffHelp("O jogador: [" + client.Player.PlayerCharacter.NickName + "] está tentando usar HACK UseBuyItemHandler");
                    gamePlayer2.SendTCP(pkg2);
                }
            }
            return 0;
        }
    }
}
