// Decompiled with JetBrains decompiler
// Type: Game.Server.WonderFul.UserWonderFulActivityManager
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.WonderFul
{
    public class UserWonderFulActivityManager
  {
    private GamePlayer Player;
    private Dictionary<string, GmActiveConditionInfo> StrengthenActives = new Dictionary<string, GmActiveConditionInfo>();
    private Dictionary<string, GmActiveConditionInfo> PayActives = new Dictionary<string, GmActiveConditionInfo>();

    public UserWonderFulActivityManager(GamePlayer player)
    {
      this.Player = player;
     // this.Init();
    }

   /* public void Init()
    {
      foreach (GmActivityInfo gmActivityInfo in GmActivityMgr.GmActivityInfos.FindAll((Predicate<GmActivityInfo>) (a => a.endTime > DateTime.Now && a.beginTime <= DateTime.Now)))
      {
        switch (gmActivityInfo.activityType)
        {
          case 0:
            using (List<GmGiftInfo>.Enumerator enumerator = GmActivityMgr.FindGmGift(gmActivityInfo.activityId).GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GmGiftInfo current = enumerator.Current;
                this.PayActives.Add(current.giftbagId, GmActivityMgr.FindGmActiveCondition(current.giftbagId).First<GmActiveConditionInfo>());
              }
              break;
            }
          case 8:
            using (List<GmGiftInfo>.Enumerator enumerator = GmActivityMgr.FindGmGift(gmActivityInfo.activityId).GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GmGiftInfo current = enumerator.Current;
                this.StrengthenActives.Add(current.giftbagId, GmActivityMgr.FindGmActiveCondition(current.giftbagId).First<GmActiveConditionInfo>());
              }
              break;
            }
        }
      }
      this.Player.AddFund += new GamePlayer.PlayerFundHandler(this.Player_AddMedal);
      this.Player.ItemStrengthen += new GamePlayer.PlayerItemStrengthenEventHandle(this.Player_ItemStrengthen);
    }*/

    private void Player_AddMedal(int count, int type)
    {
      if (type != 4)
        return;
      foreach (KeyValuePair<string, GmActiveConditionInfo> payActive in this.PayActives)
      {
        KeyValuePair<string, GmActiveConditionInfo> act = payActive;
        UserGmActivityCondition activityCondition = this.Player.GmActivityConditions.Find((Predicate<UserGmActivityCondition>) (t => t.GiftBagID == act.Key));
        if (activityCondition.StatusID <= 0 || activityCondition.StatusValue != 0)
        {
          activityCondition.StatusValue += count;
          activityCondition.StatusID = act.Value.conditionValue;
        }
      }
    }

    private void Player_ItemStrengthen(int categoryID, int level)
    {
      foreach (KeyValuePair<string, GmActiveConditionInfo> strengthenActive in this.StrengthenActives)
      {
        KeyValuePair<string, GmActiveConditionInfo> act = strengthenActive;
        UserGmActivityCondition activityCondition = this.Player.GmActivityConditions.Find((Predicate<UserGmActivityCondition>) (t => t.GiftBagID == act.Key));
        if ((activityCondition.StatusID <= 0 || activityCondition.StatusValue != 0) && act.Value.conditionValue == level)
        {
          activityCondition.StatusValue = level;
          activityCondition.StatusID = act.Value.conditionValue;
        }
      }
    }
  }
}
