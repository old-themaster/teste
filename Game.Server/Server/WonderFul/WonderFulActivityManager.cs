// Decompiled with JetBrains decompiler
// Type: Game.Server.WonderFul.WonderFulActivityManager
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Server.WonderFul
{
  public class WonderFulActivityManager
  {
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly object Locker = new object();

    public static void WonderFulActivityInit(GamePlayer player, int type = 3)
    {
      GSPacketIn pkg = new GSPacketIn((short) 405, player.PlayerCharacter.ID);
      pkg.WriteInt(type);
      if (type <= 0)
        return;
      try
      {
        List<GmActivityInfo> all1 = GmActivityMgr.GmActivityInfos.FindAll((Predicate<GmActivityInfo>) (q => q.beginTime < DateTime.Now && q.endTime > DateTime.Now));
        pkg.WriteInt(all1.Count);
        foreach (GmActivityInfo gmActivityInfo in all1)
        {
          GmActivityInfo active = gmActivityInfo;
          pkg.WriteString(active.activityId);
          List<GmGiftInfo> gmGift = GmActivityMgr.FindGmGift(active.activityId);
          List<UserGmActivityCondition> activityConditionList = new List<UserGmActivityCondition>();
          List<UserGmActivityReward> gmActivityRewardList = new List<UserGmActivityReward>();
          foreach (GmGiftInfo gmGiftInfo in gmGift)
          {
            GmGiftInfo gift = gmGiftInfo;
            List<UserGmActivityCondition> all2 = player.GmActivityConditions.FindAll((Predicate<UserGmActivityCondition>) (q => q.GiftBagID == gift.giftbagId));
            int day = 0;
            if (all2.Count == 0)
            {
              foreach (UserGmActivityCondition activityCondition in GmActivityMgr.FindGmActiveCondition(gift.giftbagId).Select<GmActiveConditionInfo, UserGmActivityCondition>((Func<GmActiveConditionInfo, UserGmActivityCondition>) (conditionInfo => new UserGmActivityCondition()
              {
                UserID = player.PlayerId,
                ActivityID = active.activityId,
                GiftBagID = conditionInfo.giftbagId,
                StatusID = active.activityType == 60 ? day++ : 0,
                StatusValue = 0
              })))
              {
                activityConditionList.Add(activityCondition);
                player.GmActivityConditions.Add(activityCondition);
              }
            }
            else
              activityConditionList.AddRange((IEnumerable<UserGmActivityCondition>) all2);
            List<UserGmActivityReward> all3 = player.GmActivityRewards.FindAll((Predicate<UserGmActivityReward>) (q => q.GiftBagID == gift.giftbagId));
            if (all3.Count == 0)
            {
              foreach (UserGmActivityReward gmActivityReward in GmActivityMgr.FindGmActiveReward(gift.giftbagId).Select<GmActiveRewardInfo, UserGmActivityReward>((Func<GmActiveRewardInfo, UserGmActivityReward>) (rewardInfo => new UserGmActivityReward()
              {
                UserID = player.PlayerId,
                ActivityID = active.activityId,
                GiftBagID = rewardInfo.giftId,
                Times = 0
              })))
              {
                gmActivityRewardList.Add(gmActivityReward);
                player.GmActivityRewards.Add(gmActivityReward);
              }
            }
            else
              gmActivityRewardList.AddRange((IEnumerable<UserGmActivityReward>) all3);
          }
          pkg.WriteInt(activityConditionList.Count);
          foreach (UserGmActivityCondition activityCondition in activityConditionList)
          {
            pkg.WriteInt(activityCondition.StatusID);
            pkg.WriteInt(activityCondition.StatusValue);
          }
          pkg.WriteInt(gmActivityRewardList.Count);
          foreach (UserGmActivityReward gmActivityReward in gmActivityRewardList)
          {
            pkg.WriteString(gmActivityReward.GiftBagID);
            pkg.WriteInt(gmActivityReward.Times);
            pkg.WriteInt(GmActivityMgr.FindGmActiveReward(gmActivityReward.GiftBagID)[0].allGiftGetTimes);
          }
        }
        player.SendTCP(pkg);
      }
      catch (Exception ex)
      {
        if (!WonderFulActivityManager.Log.IsErrorEnabled)
          return;
        WonderFulActivityManager.Log.Error((object) "WonderFulActivityInit error:", ex);
      }
    }

    public static void SendWonderFulReward(GamePlayer player, GSPacketIn packet)
    {
      try
      {
        lock (WonderFulActivityManager.Locker)
        {
          for (int index1 = 0; index1 < packet.ReadInt(); ++index1)
          {
            string activityId = packet.ReadString();
            int num1 = packet.ReadInt();
            GmActivityInfo gmActivity = GmActivityMgr.FindGmActivity(activityId);
            for (int index2 = 0; index2 < packet.ReadInt(); ++index2)
            {
              string[] strArray1 = packet.ReadString().Split(',');
              string giftId = strArray1[0];
              packet.ReadInt();
              List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
              if (gmActivity != null && !(gmActivity.beginTime > DateTime.Now) && !(gmActivity.endTime < DateTime.Now) && GmActivityMgr.FindGmGift(activityId).Count != 0)
              {
                List<GmActiveRewardInfo> gmActiveReward = GmActivityMgr.FindGmActiveReward(giftId);
                if (gmActiveReward.Count != 0)
                {
                  List<GmActiveConditionInfo> gmActiveCondition = GmActivityMgr.FindGmActiveCondition(giftId);
                  UserGmActivityReward gmActivityReward = player.GmActivityRewards.Find((Predicate<UserGmActivityReward>) (a => a.ActivityID == activityId && a.GiftBagID == giftId));
                  if (gmActivityReward != null)
                  {
                    switch (gmActivity.activityType)
                    {
                      case 0:
                        if (gmActiveCondition.Count != 0 && gmActivity.activityChildType == 6 && gmActivityReward.Times <= 0)
                        {
                          UserGmActivityCondition activityCondition = player.GmActivityConditions.Find((Predicate<UserGmActivityCondition>) (a => a.ActivityID == activityId));
                          if (activityCondition != null && activityCondition.StatusValue >= gmActiveCondition[0].conditionValue)
                          {
                            foreach (GmActiveRewardInfo activeRewardInfo in gmActiveReward)
                            {
                              SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(activeRewardInfo.templateId), 1, 102);
                              if (fromTemplate != null)
                              {
                                fromTemplate.IsBinds = activeRewardInfo.isBind == 1;
                                fromTemplate.Count = activeRewardInfo.count;
                                fromTemplate.ValidDate = activeRewardInfo.validDate;
                                string[] strArray2 = activeRewardInfo.property.Split(',');
                                fromTemplate.StrengthenLevel = int.Parse(strArray2[0]);
                                fromTemplate.AttackCompose = int.Parse(strArray2[1]);
                                fromTemplate.DefendCompose = int.Parse(strArray2[2]);
                                fromTemplate.AgilityCompose = int.Parse(strArray2[3]);
                                fromTemplate.LuckCompose = int.Parse(strArray2[4]);
                                items.Add(fromTemplate);
                              }
                            }

                            if (player.SendItemsToMail(items, eMailType.Manage, LanguageMgr.GetTranslation("WonderFulActivity.Mail.Title"), LanguageMgr.GetTranslation("WonderFulActivity.Mail.Content", (object) gmActivity.activityName)))
                            {
                              player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                              player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.GetReward.Success"));
                              gmActivityReward.Times = 1;
                              break;
                            }
                            continue;
                          }
                          continue;
                        }
                        continue;
                      case 2:
                        if (gmActivity.activityChildType == 0)
                        {
                          foreach (GmActiveRewardInfo activeRewardInfo in gmActiveReward.FindAll((Predicate<GmActiveRewardInfo>) (r => r.rewardType == 0)))
                          {
                            if (player.GetItemCount(activeRewardInfo.templateId) < activeRewardInfo.count)
                              return;
                            player.RemoveTemplate(activeRewardInfo.templateId, activeRewardInfo.count);
                          }
                          foreach (GmActiveRewardInfo activeRewardInfo in gmActiveReward.FindAll((Predicate<GmActiveRewardInfo>) (r => r.rewardType == 1)))
                          {
                            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(activeRewardInfo.templateId), 1, 102);
                            if (fromTemplate != null)
                            {
                              fromTemplate.IsBinds = activeRewardInfo.isBind == 1;
                              fromTemplate.Count = activeRewardInfo.count;
                              fromTemplate.ValidDate = activeRewardInfo.validDate;
                              string[] strArray3 = activeRewardInfo.property.Split(',');
                              fromTemplate.StrengthenLevel = int.Parse(strArray3[0]);
                              fromTemplate.AttackCompose = int.Parse(strArray3[1]);
                              fromTemplate.DefendCompose = int.Parse(strArray3[2]);
                              fromTemplate.AgilityCompose = int.Parse(strArray3[3]);
                              fromTemplate.LuckCompose = int.Parse(strArray3[4]);
                              items.Add(fromTemplate);
                            }
                          }
                          if (player.SendItemsToMail(items, eMailType.Manage, LanguageMgr.GetTranslation("WonderFulActivity.Mail.Title"), LanguageMgr.GetTranslation("WonderFulActivity.Mail.Content", (object) gmActivity.activityName)))
                          {
                            player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                            player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.GetReward.Success"));
                            break;
                          }
                          continue;
                        }
                        break;
                      case 8:
                        if (gmActiveCondition.Count != 0 && gmActivity.activityChildType == 0)
                        {
                          UserGmActivityCondition activityCondition = player.GmActivityConditions.Find((Predicate<UserGmActivityCondition>) (a => a.ActivityID == activityId && a.GiftBagID == giftId));
                          if (activityCondition != null && activityCondition.StatusID >= gmActiveCondition.First<GmActiveConditionInfo>().conditionValue && activityCondition.StatusValue != 0)
                          {
                            foreach (GmActiveRewardInfo activeRewardInfo in gmActiveReward)
                            {
                              SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(activeRewardInfo.templateId), 1, 102);
                              if (fromTemplate != null)
                              {
                                fromTemplate.IsBinds = activeRewardInfo.isBind == 1;
                                fromTemplate.Count = activeRewardInfo.count;
                                fromTemplate.ValidDate = activeRewardInfo.validDate;
                                string[] strArray4 = activeRewardInfo.property.Split(',');
                                fromTemplate.StrengthenLevel = int.Parse(strArray4[0]);
                                fromTemplate.AttackCompose = int.Parse(strArray4[1]);
                                fromTemplate.DefendCompose = int.Parse(strArray4[2]);
                                fromTemplate.AgilityCompose = int.Parse(strArray4[3]);
                                fromTemplate.LuckCompose = int.Parse(strArray4[4]);
                                items.Add(fromTemplate);
                              }
                            }
                            if (player.SendItemsToMail(items, eMailType.Manage, LanguageMgr.GetTranslation("WonderFulActivity.Mail.Title"), LanguageMgr.GetTranslation("WonderFulActivity.Mail.Content", (object) gmActivity.activityName)))
                            {
                              player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                              player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.GetReward.Success"));
                              activityCondition.StatusValue = 0;
                              break;
                            }
                            continue;
                          }
                          continue;
                        }
                        continue;
                      case 19:
                        bool flag = int.Parse(strArray1[1]) != -8;
                        GmActiveConditionInfo activeConditionInfo1 = gmActiveCondition.Find((Predicate<GmActiveConditionInfo>) (c => c.conditionIndex == 1));
                        GmActiveConditionInfo activeConditionInfo2 = gmActiveCondition.Find((Predicate<GmActiveConditionInfo>) (c => c.conditionIndex == 2));
                        GmActiveConditionInfo activeConditionInfo3 = gmActiveCondition.Find((Predicate<GmActiveConditionInfo>) (c => c.conditionIndex == 3));
                        gmActiveCondition.Find((Predicate<GmActiveConditionInfo>) (c => c.conditionIndex == 4));
                        GmActiveConditionInfo activeConditionInfo4 = gmActiveCondition.Find((Predicate<GmActiveConditionInfo>) (c => c.conditionIndex == 5));
                        GmActiveConditionInfo activeConditionInfo5 = gmActiveCondition.Find((Predicate<GmActiveConditionInfo>) (c => c.conditionIndex == 100));
                        int num2 = activeConditionInfo3.conditionValue * num1;
                        GmActiveRewardInfo activeRewardInfo1 = gmActiveReward[0];
                        if (flag && activeConditionInfo2.conditionValue == -9)
                        {
                          if (player.PlayerCharacter.DDTMoney < num2)
                          {
                            player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoGiftToken"));
                            continue;
                          }
                        }
                        else if (player.PlayerCharacter.Money < num2)
                        {
                          player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoMoney"));
                          continue;
                        }
                        if (gmActivity.activityChildType == 2)
                        {
                          if (activeConditionInfo4.conditionValue > 0)
                          {
                            if (player.PlayerCharacter.typeVIP == (byte) 0)
                            {
                              player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoVIP"));
                              continue;
                            }
                            if (player.PlayerCharacter.VIPLevel < activeConditionInfo4.conditionValue)
                            {
                              player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.VIPLimit", (object) activeConditionInfo4.conditionValue));
                              continue;
                            }
                          }
                        }
                        else if (gmActivity.activityChildType == 3)
                        {
                          if (gmActivityReward.Times >= activeConditionInfo1.conditionValue)
                          {
                            player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.IndividualLimit"));
                            continue;
                          }
                          if (gmActivityReward.Times + num1 > activeConditionInfo1.conditionValue)
                          {
                            num1 = activeConditionInfo1.conditionValue - gmActivityReward.Times;
                            int num3 = activeConditionInfo3.conditionValue * num1;
                            if (flag && activeConditionInfo2.conditionValue == -9)
                            {
                              if (player.PlayerCharacter.DDTMoney < num3)
                              {
                                player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoGiftToken"));
                                continue;
                              }
                            }
                            else if (player.PlayerCharacter.Money < num3)
                            {
                              player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoMoney"));
                              continue;
                            }
                          }
                        }
                        else if (gmActivity.activityChildType == 5)
                        {
                          if (activeRewardInfo1.allGiftGetTimes == activeConditionInfo5.conditionValue)
                          {
                            player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.ServerLimit"));
                            continue;
                          }
                          if (activeRewardInfo1.allGiftGetTimes + num1 > activeConditionInfo5.conditionValue)
                          {
                            num1 = activeConditionInfo5.conditionValue - activeRewardInfo1.allGiftGetTimes;
                            int num4 = activeConditionInfo3.conditionValue * num1;
                            if (flag && activeConditionInfo2.conditionValue == -9)
                            {
                              if (player.PlayerCharacter.DDTMoney < num4)
                              {
                                player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoGiftToken"));
                                continue;
                              }
                            }
                            else if (player.PlayerCharacter.Money < num4)
                            {
                              player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.PanicBuying.NoMoney"));
                              continue;
                            }
                          }
                        }
                        for (int index3 = 0; index3 < num1; ++index3)
                        {
                          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(activeRewardInfo1.templateId), 1, 102);
                          if (fromTemplate != null)
                          {
                            fromTemplate.IsBinds = activeRewardInfo1.isBind == 1;
                            fromTemplate.Count = ItemMgr.FindItemTemplate(activeRewardInfo1.templateId).MaxCount == 1 ? activeRewardInfo1.count : activeRewardInfo1.count * num1;
                            fromTemplate.ValidDate = activeRewardInfo1.validDate;
                            string[] strArray5 = activeRewardInfo1.property.Split(',');
                            fromTemplate.StrengthenLevel = int.Parse(strArray5[0]);
                            fromTemplate.AttackCompose = int.Parse(strArray5[1]);
                            fromTemplate.DefendCompose = int.Parse(strArray5[2]);
                            fromTemplate.AgilityCompose = int.Parse(strArray5[3]);
                            fromTemplate.LuckCompose = int.Parse(strArray5[4]);
                            items.Add(fromTemplate);
                            if (ItemMgr.FindItemTemplate(activeRewardInfo1.templateId).MaxCount > 1)
                              break;
                          }
                          else
                            break;
                        }
                        if (flag && activeConditionInfo2.conditionValue == -9)
                        {
                          if (player.GiftTokenDirect(activeConditionInfo3.conditionValue))
                          {
                            if (!player.SendItemsToMail(items, eMailType.Manage, LanguageMgr.GetTranslation("WonderFulActivity.Mail.Title"), LanguageMgr.GetTranslation("WonderFulActivity.Mail.Content")))
                            {
                              player.AddGiftToken(activeConditionInfo3.conditionValue);
                              continue;
                            }
                            player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                            if (gmActivity.activityChildType == 3)
                              gmActivityReward.Times += num1;
                            else if (gmActivity.activityChildType == 5)
                              activeRewardInfo1.allGiftGetTimes += num1;
                            player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.GetReward.Success"));
                            WonderFulActivityManager.WonderFulActivityInit(player);
                            break;
                          }
                          break;
                        }
                        if (player.MoneyDirect(activeConditionInfo3.conditionValue))
                        {
                          if (!player.SendItemsToMail(items, eMailType.Manage, LanguageMgr.GetTranslation("WonderFulActivity.Mail.Title"), LanguageMgr.GetTranslation("WonderFulActivity.Mail.Content")))
                          {
                            player.AddMoney(activeConditionInfo3.conditionValue);
                            continue;
                          }
                          player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                          if (gmActivity.activityChildType == 3)
                            gmActivityReward.Times += num1;
                          else if (gmActivity.activityChildType == 5)
                            activeRewardInfo1.allGiftGetTimes += num1;
                          player.SendMessage(LanguageMgr.GetTranslation("WonderfulActivity.GetReward.Success"));
                          WonderFulActivityManager.WonderFulActivityInit(player);
                        }
                        break;
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (!WonderFulActivityManager.Log.IsErrorEnabled)
          return;
        WonderFulActivityManager.Log.Error((object) "SendWonderFulReward error:", ex);
      }
    }
  }
}
