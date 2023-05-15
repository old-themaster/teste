// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.SubActiveMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
  public class SubActiveMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static Dictionary<int, SubActiveConditionInfo> m_SubActiveConditionInfo = new Dictionary<int, SubActiveConditionInfo>();
    public static Dictionary<int, List<SubActiveInfo>> m_SubActiveInfo = new Dictionary<int, List<SubActiveInfo>>();

    public static SubActiveConditionInfo GetSubActiveInfo(SqlDataProvider.Data.ItemInfo item)
    {
      foreach (List<SubActiveInfo> subActiveInfoList in SubActiveMgr.m_SubActiveInfo.Values)
      {
        foreach (SubActiveInfo info in subActiveInfoList)
        {
          if (SubActiveMgr.IsValid(info))
          {
            foreach (SubActiveConditionInfo activeConditionInfo in SubActiveMgr.m_SubActiveConditionInfo.Values)
            {
              if (info.ActiveID == activeConditionInfo.ActiveID && info.SubID == activeConditionInfo.SubID && activeConditionInfo.ConditionID == item.TemplateID)
              {
                switch (item.Template.CategoryID)
                {
                  case 1:
                  case 5:
                  case 7:
                    return item.StrengthenLevel == activeConditionInfo.Type || item.IsGold && item.StrengthenLevel + 100 == activeConditionInfo.Type ? activeConditionInfo : (SubActiveConditionInfo) null;
                  case 6:
                    return activeConditionInfo;
                  default:
                    return activeConditionInfo;
                }
              }
            }
          }
        }
      }
      return (SubActiveConditionInfo) null;
    }

    public static bool Init() => SubActiveMgr.ReLoad();

    public static bool IsValid(SubActiveInfo info)
    {
      DateTime startTime = info.StartTime;
      DateTime endTime = info.EndTime;
      DateTime dateTime = info.StartTime;
      long ticks1 = dateTime.Ticks;
      dateTime = DateTime.Now;
      long ticks2 = dateTime.Ticks;
      return ticks1 <= ticks2 && info.EndTime.Ticks >= DateTime.Now.Ticks;
    }

    public static Dictionary<int, SubActiveConditionInfo> LoadSubActiveConditionDb(
      Dictionary<int, List<SubActiveInfo>> conditions)
    {
      Dictionary<int, SubActiveConditionInfo> dictionary = new Dictionary<int, SubActiveConditionInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (int key in conditions.Keys)
        {
          foreach (SubActiveConditionInfo activeConditionInfo in produceBussiness.GetAllSubActiveCondition(key))
          {
            if (key == activeConditionInfo.ActiveID && !dictionary.ContainsKey(activeConditionInfo.ID))
              dictionary.Add(activeConditionInfo.ID, activeConditionInfo);
          }
        }
      }
      return dictionary;
    }

    public static Dictionary<int, List<SubActiveInfo>> LoadSubActiveDb()
    {
      Dictionary<int, List<SubActiveInfo>> dictionary = new Dictionary<int, List<SubActiveInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (SubActiveInfo subActiveInfo in produceBussiness.GetAllSubActive())
        {
          List<SubActiveInfo> subActiveInfoList = new List<SubActiveInfo>();
          if (!dictionary.ContainsKey(subActiveInfo.ActiveID))
          {
            subActiveInfoList.Add(subActiveInfo);
            dictionary.Add(subActiveInfo.ActiveID, subActiveInfoList);
          }
          else
            dictionary[subActiveInfo.ActiveID].Add(subActiveInfo);
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, List<SubActiveInfo>> conditions = SubActiveMgr.LoadSubActiveDb();
        Dictionary<int, SubActiveConditionInfo> dictionary = SubActiveMgr.LoadSubActiveConditionDb(conditions);
        if (conditions.Count > 0)
        {
          Interlocked.Exchange<Dictionary<int, List<SubActiveInfo>>>(ref SubActiveMgr.m_SubActiveInfo, conditions);
          Interlocked.Exchange<Dictionary<int, SubActiveConditionInfo>>(ref SubActiveMgr.m_SubActiveConditionInfo, dictionary);
        }
        return true;
      }
      catch (Exception ex)
      {
        SubActiveMgr.log.Error((object) "QuestMgr", ex);
      }
      return false;
    }
  }
}
