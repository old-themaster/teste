using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Statics;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
  [PacketHandler(57, "赠送物品")]
  public class GoodsPresentHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int gold = 0;
      int money = 0;
      int offer = 0;
      int gifttoken = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      eMessageType type1 = eMessageType.GM_NOTICE;
      string translateId = "GoodsPresentHandler.Success";
      string str1 = packet.ReadString();
      string nickName = packet.ReadString();
      List<int> intList = new List<int>();
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        PlayerInfo singleByNickName = playerBussiness.GetUserSingleByNickName(nickName);
        if (singleByNickName != null)
        {
          List<ItemInfo> itemInfoList = new List<ItemInfo>();
          StringBuilder stringBuilder2 = new StringBuilder();
          int num1 = packet.ReadInt();
          for (int index = 0; index < num1; ++index)
          {
            int ID = packet.ReadInt();
            int type2 = packet.ReadInt();
            string str2 = packet.ReadString();
            string str3 = packet.ReadString();
            ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(ID);
            if (shopItemInfoById != null)
            {
              ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID), 1, 103);
              if (fromTemplate != null)
              {
                fromTemplate.Color = str2 == null ? "" : str2;
                fromTemplate.Skin = str3 == null ? "" : str3;
                if (fromTemplate != null)
                {
                  stringBuilder2.Append(type2);
                  stringBuilder2.Append(",");
                  itemInfoList.Add(fromTemplate);
                  intList = ItemInfo.SetItemType(shopItemInfoById, type2, ref gold, ref money, ref offer, ref gifttoken);
                }
              }
            }
          }
          if (itemInfoList.Count == 0)
            return 1;
          int count = client.Player.EquipBag.GetItems().Count;
          bool flag = true;
          for (int index = 0; index < intList.Count; index += 2)
          {
            if (client.Player.GetItemCount(intList[index]) < intList[index + 1])
              flag = false;
          }
          if (!flag)
          {
            type1 = eMessageType.BIGBUGLE_NOTICE;
            translateId = "UserBuyItemHandler.NoBuyItem";
            return 1;
          }
          if (gold <= client.Player.PlayerCharacter.Gold && money <= client.Player.PlayerCharacter.Money && offer <= client.Player.PlayerCharacter.Offer && gifttoken <= client.Player.PlayerCharacter.GiftToken)
          {
            stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
            client.Player.RemoveMoney(money);
            client.Player.RemoveGold(gold);
            client.Player.RemoveOffer(offer);
            client.Player.RemoveGiftToken(gifttoken);
            for (int index = 0; index < intList.Count; index += 2)
            {
              client.Player.RemoveTemplate(intList[index], intList[index + 1]);
              stringBuilder1.Append(intList[index].ToString() + ":");
            }
            string goodId = "";
            int num2 = 0;
            MailInfo mail = new MailInfo();
            StringBuilder stringBuilder3 = new StringBuilder();
            stringBuilder3.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark"));
            foreach (ItemInfo itemInfo in itemInfoList)
            {
              string str2 = goodId;
              int num3;
              string str3;
              if (!(goodId == ""))
              {
                num3 = itemInfo.TemplateID;
                str3 = "," + num3.ToString();
              }
              else
              {
                num3 = itemInfo.TemplateID;
                str3 = num3.ToString();
              }
              goodId = str2 + str3;
              itemInfo.UserID = 0;
              playerBussiness.AddGoods(itemInfo);
              ++num2;
              stringBuilder3.Append(num2);
              stringBuilder3.Append("、");
              stringBuilder3.Append(itemInfo.Template.Name);
              stringBuilder3.Append("x");
              stringBuilder3.Append(itemInfo.Count);
              stringBuilder3.Append(";");
              switch (num2 - 1)
              {
                case 0:
                  MailInfo mailInfo1 = mail;
                  num3 = itemInfo.ItemID;
                  string str4 = num3.ToString();
                  mailInfo1.Annex1 = str4;
                  mail.Annex1Name = itemInfo.Template.Name;
                  break;
                case 1:
                  MailInfo mailInfo2 = mail;
                  num3 = itemInfo.ItemID;
                  string str5 = num3.ToString();
                  mailInfo2.Annex2 = str5;
                  mail.Annex2Name = itemInfo.Template.Name;
                  break;
                case 2:
                  MailInfo mailInfo3 = mail;
                  num3 = itemInfo.ItemID;
                  string str6 = num3.ToString();
                  mailInfo3.Annex3 = str6;
                  mail.Annex3Name = itemInfo.Template.Name;
                  break;
                case 3:
                  MailInfo mailInfo4 = mail;
                  num3 = itemInfo.ItemID;
                  string str7 = num3.ToString();
                  mailInfo4.Annex4 = str7;
                  mail.Annex4Name = itemInfo.Template.Name;
                  break;
                case 4:
                  MailInfo mailInfo5 = mail;
                  num3 = itemInfo.ItemID;
                  string str8 = num3.ToString();
                  mailInfo5.Annex5 = str8;
                  mail.Annex5Name = itemInfo.Template.Name;
                  break;
              }
              if (num2 == 5)
              {
                num2 = 0;
                mail.AnnexRemark = stringBuilder3.ToString();
                stringBuilder3.Remove(0, stringBuilder3.Length);
                stringBuilder3.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark"));
                mail.Content = str1;
                mail.Gold = 0;
                mail.Money = 0;
                mail.Receiver = singleByNickName.NickName;
                mail.ReceiverID = singleByNickName.ID;
                mail.Sender = client.Player.PlayerCharacter.NickName;
                mail.SenderID = client.Player.PlayerCharacter.ID;
                mail.Title = mail.Sender + LanguageMgr.GetTranslation("GoodsPresentHandler.Content") + mail.Annex1Name + "]";
                mail.Type = 10;
                playerBussiness.SendMail(mail);
                mail.Revert();
              }
            }
            if (num2 > 0)
            {
              mail.AnnexRemark = stringBuilder3.ToString();
              mail.Content = str1;
              mail.Gold = 0;
              mail.Money = 0;
              mail.Receiver = singleByNickName.NickName;
              mail.ReceiverID = singleByNickName.ID;
              mail.Sender = client.Player.PlayerCharacter.NickName;
              mail.SenderID = client.Player.PlayerCharacter.ID;
              mail.Title = mail.Sender + LanguageMgr.GetTranslation("GoodsPresentHandler.Content") + mail.Annex1Name + "]";
              mail.Type = 10;
              playerBussiness.SendMail(mail);
            }
            LogMgr.LogMoneyAdd(LogMoneyType.Shop, LogMoneyType.Shop_Present, client.Player.PlayerCharacter.ID, money, client.Player.PlayerCharacter.Money, gold, gifttoken, offer, stringBuilder1.ToString(), goodId, stringBuilder2.ToString());
            client.Out.SendMailResponse(singleByNickName.ID, eMailRespose.Receiver);
            client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Send);
          }
          else
          {
            type1 = eMessageType.BIGBUGLE_NOTICE;
            translateId = "GoodsPresentHandler.NoMoney";
          }
        }
        else
        {
          type1 = eMessageType.BIGBUGLE_NOTICE;
          translateId = "GoodsPresentHandler.NoUser";
        }
      }
      client.Out.SendMessage(type1, LanguageMgr.GetTranslation(translateId));
      return 0;
    }
  }
}
