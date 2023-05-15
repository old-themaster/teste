// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserSendMailHandler
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B480679-DF24-46B7-834E-821AA9A4FB3F
// Assembly location: C:\Users\Anderson\Desktop\Source 4.2\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using Game.Server.Statics;
using SqlDataProvider.Data;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(116, "发送邮件")]
    public class UserSendMailHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn packet1 = new GSPacketIn((short)116, client.Player.PlayerCharacter.ID);
            if (client.Player.PlayerCharacter.Gold < 100)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserSendMailHandler.GoldNotEnought"));
                packet1.WriteBoolean(false);
                client.Out.SendTCP(packet1);
                return 1;
            }
            string translateId1 = "UserSendMailHandler.Success";
            eMessageType type1 = eMessageType.GM_NOTICE;
            ItemInfo itemInfo1 = (ItemInfo)null;
            string nickName = packet.ReadString();
            string str1 = packet.ReadString();
            string str2 = packet.ReadString();
            bool flag = packet.ReadBoolean();
            int num1 = packet.ReadInt();
            int num2 = packet.ReadInt();
            eBageType bagType = (eBageType)packet.ReadByte();
            int place = packet.ReadInt();
            int count1 = packet.ReadInt();
            if (client.Player.IsLimitMoney(num2))
            {
                packet1.WriteBoolean(false);
                client.Out.SendTCP(packet1);
                return 1;
            }
            int num3 = GameProperties.LimitLevel(0);
            if (place != -1 && client.Player.PlayerCharacter.Grade < num3)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserSendMailHandler.Msg4", (object)num3));
                packet1.WriteBoolean(false);
                client.Out.SendTCP(packet1);
                return 0;
            }
            if (bagType == eBageType.EquipBag && place != -1 && place < client.Player.EquipBag.BeginSlot)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserSendMailHandler.WrongPlace"));
                packet1.WriteBoolean(false);
                client.Out.SendTCP(packet1);
                return 0;
            }
            if ((num2 != 0 || place != -1) && client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                packet1.WriteBoolean(false);
                client.Out.SendTCP(packet1);
                return 1;
            }
            ItemInfo cloneItem = (ItemInfo)null;
            int num4;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                GamePlayer byPlayerNickName = WorldMgr.GetClientByPlayerNickName(nickName);
                PlayerInfo playerInfo = byPlayerNickName != null ? byPlayerNickName.PlayerCharacter : playerBussiness.GetUserSingleByNickName(nickName);
                GSPacketIn gsPacketIn;
                if (playerInfo == null || string.IsNullOrEmpty(nickName))
                {
                    eMessageType type2 = eMessageType.BIGBUGLE_NOTICE;
                    string translateId2 = "UserSendMailHandler.Failed2";
                    packet1.WriteBoolean(false);
                    gsPacketIn = client.Out.SendMessage(type2, LanguageMgr.GetTranslation(translateId2));
                    client.Out.SendTCP(packet1);
                    return 0;
                }
                if (playerInfo.Grade < num3)
                {
                    client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserSendMailHandler.Msg3", (object)num3));
                    packet1.WriteBoolean(false);
                    client.Out.SendTCP(packet1);
                    num4 = 0;
                }
                else
                {
                    if (playerInfo.NickName == client.Player.PlayerCharacter.NickName)
                    {
                        string translateId2 = "UserSendMailHandler.Failed1";
                        packet1.WriteBoolean(false);
                        gsPacketIn = client.Out.SendMessage(type1, LanguageMgr.GetTranslation(translateId2));
                        client.Out.SendTCP(packet1);
                        return 0;
                    }
                    MailInfo mail = new MailInfo()
                    {
                        SenderID = client.Player.PlayerCharacter.ID,
                        Sender = client.Player.PlayerCharacter.NickName,
                        ReceiverID = playerInfo.ID,
                        Receiver = playerInfo.NickName,
                        IsExist = true,
                        Gold = 0,
                        Money = 0,
                        Title = str1,
                        Content = str2
                    };
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(LanguageMgr.GetTranslation("UserSendMailHandler.AnnexRemark"));
                    int num5 = 0;
                    if (place != -1)
                    {
                        itemInfo1 = client.Player.GetItemAt(bagType, place);
                        if (itemInfo1 == null)
                        {
                            client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserSendMailHandler.Msg1"));
                            num4 = 0;
                            return num4;
                        }
                        if (!itemInfo1.IsBinds && itemInfo1 != null)
                        {
                            cloneItem = ItemInfo.CloneFromTemplate(itemInfo1.Template, itemInfo1);
                            ItemInfo itemInfo2 = ItemInfo.CloneFromTemplate(itemInfo1.Template, itemInfo1);
                            if (itemInfo2.ItemID == 0)
                                playerBussiness.AddGoods(itemInfo2);
                            mail.Annex1Name = itemInfo2.Template.Name;
                            mail.Annex1 = itemInfo2.ItemID.ToString();
                            ++num5;
                            stringBuilder.Append(num5);
                            stringBuilder.Append("、");
                            stringBuilder.Append(mail.Annex1Name);
                            stringBuilder.Append("x");
                            stringBuilder.Append(itemInfo2.Count);
                            stringBuilder.Append(";");
                        }
                        else
                        {
                            num4 = 0;
                            return num4;
                        }
                    }
                    if (!flag)
                    {
                        mail.Type = 1;
                        if (client.Player.PlayerCharacter.Money >= num2 && num2 > 0)
                        {
                            mail.Money = num2;
                            LogMgr.LogMoneyAdd(LogMoneyType.Mail, LogMoneyType.Mail_Send, client.Player.PlayerCharacter.ID, num2, client.Player.PlayerCharacter.Money, 0, 0, 0, "", "", "");
                            client.Player.RemoveMoney(num2);
                            int num6 = num5 + 1;
                            stringBuilder.Append(num6);
                            stringBuilder.Append("、");
                            stringBuilder.Append(LanguageMgr.GetTranslation("UserSendMailHandler.Money"));
                            stringBuilder.Append(num2);
                            stringBuilder.Append(";");
                        }
                    }
                    else
                    {
                        if (num2 <= 0 || string.IsNullOrEmpty(mail.Annex1) && string.IsNullOrEmpty(mail.Annex2) && string.IsNullOrEmpty(mail.Annex3) && string.IsNullOrEmpty(mail.Annex4))
                        {
                            num4 = 1;
                            return num4;
                        }
                        mail.ValidDate = num1 == 1 ? 1 : 6;
                        mail.Type = 101;
                        if (num2 > 0)
                        {
                            mail.Money = num2;
                            int num6 = num5 + 1;
                            stringBuilder.Append(num6);
                            stringBuilder.Append("、");
                            stringBuilder.Append(LanguageMgr.GetTranslation("UserSendMailHandler.PayMoney"));
                            stringBuilder.Append(num2);
                            stringBuilder.Append(";");
                        }
                    }
                    if (stringBuilder.Length > 1)
                        mail.AnnexRemark = stringBuilder.ToString();
                    if (playerBussiness.SendMail(mail))
                    {
                        client.Player.RemoveGold(100);
                        if (itemInfo1 != null)
                        {
                            int count2 = itemInfo1.Count - count1;
                            client.Player.RemoveItem(itemInfo1);
                            if (count2 > 0)
                            {
                                cloneItem.Count = count2;
                                client.Player.AddTemplate(cloneItem, bagType, count2, eGameView.RouletteTypeGet);
                            }
                            client.Player.OnSendGiftmail(itemInfo1.TemplateID, count1);
                        }
                    }
                    packet1.WriteBoolean(true);
                    if (byPlayerNickName != null)
                        client.Player.Out.SendMailResponse(playerInfo.ID, eMailRespose.Receiver);
                    client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Send);
                    gsPacketIn = client.Out.SendMessage(type1, LanguageMgr.GetTranslation(translateId1));
                    client.Out.SendTCP(packet1);
                    return 0;
                }
            }
            return num4;
        }
    }
}
