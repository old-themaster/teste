// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.GmActivityMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8FC3C43-9CA5-4357-A48E-153B305FFC14
// Assembly location: C:\server\fight\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bussiness.Managers
{
    public class GmActivityMgr
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static List<GmActivityInfo> GmActivityInfos;
        public static List<GmGiftInfo> GmGiftInfos;
        public static List<GmActiveConditionInfo> GmActiveConditionInfos;
        public static List<GmActiveRewardInfo> GmActiveRewardInfos;

        public static List<GmActiveConditionInfo> FindGmActiveCondition(
          string giftId)
        {
            return GmActivityMgr.GmActiveConditionInfos.FindAll((Predicate<GmActiveConditionInfo>)(q => q.giftbagId == giftId));
        }

        public static List<GmActiveRewardInfo> FindGmActiveReward(string giftId) => GmActivityMgr.GmActiveRewardInfos.FindAll((Predicate<GmActiveRewardInfo>)(q => q.giftId == giftId));

        public static GmActivityInfo FindGmActivity(string activityId) => GmActivityMgr.GmActivityInfos.Find((Predicate<GmActivityInfo>)(q => q.activityId == activityId));

        public static List<GmGiftInfo> FindGmGift(string activeId) => GmActivityMgr.GmGiftInfos.FindAll((Predicate<GmGiftInfo>)(q => q.activityId == activeId));

        public static bool Init() => GmActivityMgr.ReLoad();

      /*  public static void LoadGmActivityDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                GmActivityMgr.GmActivityInfos = ((IEnumerable<GmActivityInfo>)produceBussiness.GetAllGmActivity()).ToList<GmActivityInfo>();
        }

        public static void LoadGmGiftDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                GmActivityMgr.GmGiftInfos = ((IEnumerable<GmGiftInfo>)produceBussiness.GetAllGmGift()).ToList<GmGiftInfo>();
        }

        public static void LoadGmActiveConditionDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                GmActivityMgr.GmActiveConditionInfos = ((IEnumerable<GmActiveConditionInfo>)produceBussiness.GetAllGmActiveCondition()).ToList<GmActiveConditionInfo>();
        }

        public static void LoadGmActiveRewardDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                GmActivityMgr.GmActiveRewardInfos = ((IEnumerable<GmActiveRewardInfo>)produceBussiness.GetAllGmActiveReward()).ToList<GmActiveRewardInfo>();
        }*/

        public static bool ReLoad()
        {
            try
            {
               /* GmActivityMgr.LoadGmActivityDb();
                GmActivityMgr.LoadGmGiftDb();
                GmActivityMgr.LoadGmActiveConditionDb();
                GmActivityMgr.LoadGmActiveRewardDb();*/
            }
            catch (Exception ex)
            {
                if (GmActivityMgr.Log.IsErrorEnabled)
                    GmActivityMgr.Log.Error((object)"ReLoad GmActivity", ex);
                return false;
            }
            return true;
        }
    }
}
