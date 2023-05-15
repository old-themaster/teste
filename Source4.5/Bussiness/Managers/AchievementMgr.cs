// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.AchievementMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Bussiness.Managers
{
  public static class AchievementMgr
  {
    private static Dictionary<int, AchievementInfo> m_achievement = new Dictionary<int, AchievementInfo>();
    private static Dictionary<int, List<AchievementConditionInfo>> m_achievementCondition = new Dictionary<int, List<AchievementConditionInfo>>();
    private static Dictionary<int, List<AchievementRewardInfo>> m_achievementReward = new Dictionary<int, List<AchievementRewardInfo>>();
    private static Dictionary<int, List<ItemRecordTypeInfo>> m_itemRecordType = new Dictionary<int, List<ItemRecordTypeInfo>>();
    private static Hashtable m_distinctCondition = new Hashtable();
    private static Hashtable m_ItemRecordTypeInfo = new Hashtable();

    public static Hashtable ItemRecordType => AchievementMgr.m_ItemRecordTypeInfo;

    public static Dictionary<int, AchievementInfo> Achievement => AchievementMgr.m_achievement;

    public static bool Init() => AchievementMgr.Reload();

    public static bool Reload()
    {
      try
      {
        AchievementMgr.LoadItemRecordTypeInfoDB();
        Dictionary<int, AchievementInfo> achievementInfos = AchievementMgr.LoadAchievementInfoDB();
        Dictionary<int, List<AchievementConditionInfo>> dictionary1 = AchievementMgr.LoadAchievementConditionInfoDB(achievementInfos);
        Dictionary<int, List<AchievementRewardInfo>> dictionary2 = AchievementMgr.LoadAchievementRewardInfoDB(achievementInfos);
        if (achievementInfos.Count > 0)
        {
          Interlocked.Exchange<Dictionary<int, AchievementInfo>>(ref AchievementMgr.m_achievement, achievementInfos);
          Interlocked.Exchange<Dictionary<int, List<AchievementConditionInfo>>>(ref AchievementMgr.m_achievementCondition, dictionary1);
          Interlocked.Exchange<Dictionary<int, List<AchievementRewardInfo>>>(ref AchievementMgr.m_achievementReward, dictionary2);
        }
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(string.Format("AchievementMgr {0}", (object) ex));
      }
      return false;
    }

    public static void LoadItemRecordTypeInfoDB()
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (AchievementConditionInfo achievementConditionInfo in produceBussiness.GetALlAchievementCondition())
        {
          if (!AchievementMgr.m_ItemRecordTypeInfo.Contains((object) achievementConditionInfo.CondictionType))
            AchievementMgr.m_ItemRecordTypeInfo.Add((object) achievementConditionInfo.CondictionType, (object) achievementConditionInfo.CondictionType);
        }
      }
    }

    public static Dictionary<int, AchievementInfo> LoadAchievementInfoDB()
    {
      Dictionary<int, AchievementInfo> dictionary = new Dictionary<int, AchievementInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (AchievementInfo achievementInfo in produceBussiness.GetALlAchievement())
        {
          if (!dictionary.ContainsKey(achievementInfo.ID))
            dictionary.Add(achievementInfo.ID, achievementInfo);
        }
      }
      return dictionary;
    }

    public static Dictionary<int, List<AchievementConditionInfo>> LoadAchievementConditionInfoDB(
      Dictionary<int, AchievementInfo> achievementInfos)
    {
      Dictionary<int, List<AchievementConditionInfo>> dictionary = new Dictionary<int, List<AchievementConditionInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        AchievementConditionInfo[] achievementCondition = produceBussiness.GetALlAchievementCondition();
        foreach (AchievementInfo achievementInfo1 in achievementInfos.Values)
        {
          AchievementInfo achievementInfo = achievementInfo1;
          IEnumerable<AchievementConditionInfo> source = ((IEnumerable<AchievementConditionInfo>) achievementCondition).Where<AchievementConditionInfo>((Func<AchievementConditionInfo, bool>) (s => s.AchievementID == achievementInfo.ID));
          dictionary.Add(achievementInfo.ID, source.ToList<AchievementConditionInfo>());
          if (source != null)
          {
            foreach (AchievementConditionInfo achievementConditionInfo in source)
            {
              if (!AchievementMgr.m_distinctCondition.Contains((object) achievementConditionInfo.CondictionType))
                AchievementMgr.m_distinctCondition.Add((object) achievementConditionInfo.CondictionType, (object) achievementConditionInfo.CondictionType);
            }
          }
        }
      }
      return dictionary;
    }

    public static Dictionary<int, List<AchievementRewardInfo>> LoadAchievementRewardInfoDB(
      Dictionary<int, AchievementInfo> achievementInfos)
    {
      Dictionary<int, List<AchievementRewardInfo>> dictionary = new Dictionary<int, List<AchievementRewardInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        AchievementRewardInfo[] achievementReward = produceBussiness.GetALlAchievementReward();
        foreach (AchievementInfo achievementInfo1 in achievementInfos.Values)
        {
          AchievementInfo achievementInfo = achievementInfo1;
          IEnumerable<AchievementRewardInfo> source = ((IEnumerable<AchievementRewardInfo>) achievementReward).Where<AchievementRewardInfo>((Func<AchievementRewardInfo, bool>) (s => s.AchievementID == achievementInfo.ID));
          dictionary.Add(achievementInfo.ID, source.ToList<AchievementRewardInfo>());
        }
      }
      return dictionary;
    }

    public static AchievementInfo GetSingleAchievement(int id) => !AchievementMgr.m_achievement.ContainsKey(id) ? (AchievementInfo) null : AchievementMgr.m_achievement[id];

    public static List<AchievementConditionInfo> GetAchievementCondition(
      AchievementInfo info) => !AchievementMgr.m_achievementCondition.ContainsKey(info.ID) ? (List<AchievementConditionInfo>) null : AchievementMgr.m_achievementCondition[info.ID];

    public static List<AchievementRewardInfo> GetAchievementReward(
      AchievementInfo info) => !AchievementMgr.m_achievementReward.ContainsKey(info.ID) ? (List<AchievementRewardInfo>) null : AchievementMgr.m_achievementReward[info.ID];
  }
}
