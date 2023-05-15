// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.EventAwardMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
  public class EventAwardMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static EventAwardInfo[] m_eventAward;
    private static Dictionary<int, List<EventAwardInfo>> m_EventAwards;
    private static ThreadSafeRandom rand = new ThreadSafeRandom();

    public static void CreateEventAward(eEventType DateId)
    {
    }

    public static EventAwardInfo CreateSearchGoodsAward(eEventType DataId)
    {
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      int num1 = 1;
      int maxRound = ThreadSafeRandom.NextStatic(eventAward.Select<EventAwardInfo, int>((Func<EventAwardInfo, int>) (s => s.Random)).Max());
      List<EventAwardInfo> list = eventAward.Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.Random >= maxRound)).ToList<EventAwardInfo>();
      int num2 = list.Count<EventAwardInfo>();
      if (num2 > 0)
      {
        int count = num1 > num2 ? num2 : num1;
        foreach (int randomUnrepeat in EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
        {
          EventAwardInfo eventAwardInfo = list[randomUnrepeat];
          eventAwardInfoList.Add(eventAwardInfo);
        }
      }
      foreach (EventAwardInfo eventAwardInfo in eventAwardInfoList)
      {
        if (ItemMgr.FindItemTemplate(eventAwardInfo.TemplateID) != null)
          return eventAwardInfo;
      }
      return (EventAwardInfo) null;
    }
        public static List<NewChickenBoxItemInfo> GetNewChickenBoxAward(eEventType DataId)
        {
            List<NewChickenBoxItemInfo> list = new List<NewChickenBoxItemInfo>();
            List<EventAwardInfo> list2 = new List<EventAwardInfo>();
            List<EventAwardInfo> source = EventAwardMgr.FindEventAward(DataId);
            int num = 1;
            int maxRound = ThreadSafeRandom.NextStatic((from s in source
                                                        select s.Random).Max());
            List<EventAwardInfo> list3 = (from s in source
                                          where s.Random >= maxRound
                                          select s).ToList<EventAwardInfo>();
            int num2 = list3.Count<EventAwardInfo>();
            if (num2 > 0)
            {
                num = ((num > num2) ? num2 : num);
                int[] randomUnrepeatArray = EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, num);
                int[] array = randomUnrepeatArray;
                for (int i = 0; i < array.Length; i++)
                {
                    int index = array[i];
                    EventAwardInfo item = list3[index];
                    list2.Add(item);
                }
            }
            foreach (EventAwardInfo current in list2)
            {
                NewChickenBoxItemInfo newChickenBoxItemInfo = new NewChickenBoxItemInfo();
                newChickenBoxItemInfo.TemplateID = current.TemplateID;
                newChickenBoxItemInfo.IsBinds = current.IsBinds;
                newChickenBoxItemInfo.ValidDate = current.ValidDate;
                newChickenBoxItemInfo.Count = current.Count;
                newChickenBoxItemInfo.StrengthenLevel = current.StrengthenLevel;
                newChickenBoxItemInfo.AttackCompose = 0;
                newChickenBoxItemInfo.DefendCompose = 0;
                newChickenBoxItemInfo.AgilityCompose = 0;
                newChickenBoxItemInfo.LuckCompose = 0;
                ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(current.TemplateID);
                newChickenBoxItemInfo.Quality = ((itemTemplateInfo == null) ? 2 : itemTemplateInfo.Quality);
                newChickenBoxItemInfo.IsSelected = false;
                newChickenBoxItemInfo.IsSeeded = false;
                list.Add(newChickenBoxItemInfo);
            }
            return list;
        }

        public static List<EventAwardInfo> FindEventAward(eEventType DataId) => EventAwardMgr.m_EventAwards.ContainsKey((int) DataId) ? EventAwardMgr.m_EventAwards[(int) DataId] : (List<EventAwardInfo>) null;

    public static List<SqlDataProvider.Data.ItemInfo> GetAllEventAwardAward(
      eEventType DataId)
    {
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (EventAwardInfo eventAwardInfo in eventAward)
      {
        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventAwardInfo.TemplateID), eventAwardInfo.Count, 105);
        fromTemplate.IsBinds = eventAwardInfo.IsBinds;
        fromTemplate.ValidDate = eventAwardInfo.ValidDate;
        itemInfoList.Add(fromTemplate);
      }
      return itemInfoList;
    }

    public static List<EventAwardInfo> GetBoGuBoxAward(int type)
    {
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(eEventType.BOGU_AVEDTURE);
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      foreach (EventAwardInfo eventAwardInfo in eventAward)
      {
        if (eventAwardInfo.Random == type)
          eventAwardInfoList.Add(eventAwardInfo);
      }
      return eventAwardInfoList;
    }

    public static List<EventAwardInfo> GetDiceAward(eEventType DataId)
    {
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      int num1 = 1;
      int maxRound = ThreadSafeRandom.NextStatic(eventAward.Select<EventAwardInfo, int>((Func<EventAwardInfo, int>) (s => s.Random)).Max());
      List<EventAwardInfo> list = eventAward.Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.Random >= maxRound)).ToList<EventAwardInfo>();
      int num2 = list.Count<EventAwardInfo>();
      if (num2 > 0)
      {
        int count = num1 > num2 ? num2 : num1;
        foreach (int randomUnrepeat in EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
        {
          EventAwardInfo eventAwardInfo = list[randomUnrepeat];
          eventAwardInfoList.Add(eventAwardInfo);
        }
      }
      return eventAwardInfoList;
    }

    public static List<CardInfo> GetFightFootballTimeAward(eEventType DataId)
    {
      List<CardInfo> cardInfoList = new List<CardInfo>();
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      int num1 = 1;
      int maxRound = ThreadSafeRandom.NextStatic(eventAward.Select<EventAwardInfo, int>((Func<EventAwardInfo, int>) (s => s.Random)).Max());
      List<EventAwardInfo> list = eventAward.Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.Random >= maxRound)).ToList<EventAwardInfo>();
      int num2 = list.Count<EventAwardInfo>();
      if (num2 > 0)
      {
        int count = num1 > num2 ? num2 : num1;
        foreach (int randomUnrepeat in EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
        {
          EventAwardInfo eventAwardInfo = list[randomUnrepeat];
          eventAwardInfoList.Add(eventAwardInfo);
        }
      }
      foreach (EventAwardInfo eventAwardInfo in eventAwardInfoList)
      {
        CardInfo cardInfo = new CardInfo()
        {
          templateID = eventAwardInfo.TemplateID,
          count = eventAwardInfo.Count
        };
        cardInfoList.Add(cardInfo);
      }
      return cardInfoList;
    }

    public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
    {
      int[] numArray = new int[count];
      for (int index1 = 0; index1 < count; ++index1)
      {
        int num1 = EventAwardMgr.rand.Next(minValue, maxValue + 1);
        int num2 = 0;
        for (int index2 = 0; index2 < index1; ++index2)
        {
          if (numArray[index2] == num1)
            ++num2;
        }
        if (num2 == 0)
          numArray[index1] = num1;
        else
          --index1;
      }
      return numArray;
    }

    public static bool Init() => EventAwardMgr.ReLoad();

    public static EventAwardInfo[] LoadEventAwardDb()
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
        return produceBussiness.GetEventAwardInfos();
    }

    public static Dictionary<int, List<EventAwardInfo>> LoadEventAwards(
      EventAwardInfo[] EventAwards)
    {
      Dictionary<int, List<EventAwardInfo>> dictionary = new Dictionary<int, List<EventAwardInfo>>();
      for (int index = 0; index < EventAwards.Length; ++index)
      {
        EventAwardInfo info = EventAwards[index];
        if (!dictionary.Keys.Contains<int>(info.ActivityType))
        {
          IEnumerable<EventAwardInfo> source = ((IEnumerable<EventAwardInfo>) EventAwards).Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.ActivityType == info.ActivityType));
          dictionary.Add(info.ActivityType, source.ToList<EventAwardInfo>());
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        EventAwardInfo[] EventAwards = EventAwardMgr.LoadEventAwardDb();
        Dictionary<int, List<EventAwardInfo>> dictionary = EventAwardMgr.LoadEventAwards(EventAwards);
        if (EventAwards != null)
        {
          Interlocked.Exchange<EventAwardInfo[]>(ref EventAwardMgr.m_eventAward, EventAwards);
          Interlocked.Exchange<Dictionary<int, List<EventAwardInfo>>>(ref EventAwardMgr.m_EventAwards, dictionary);
        }
      }
      catch (Exception ex)
      {
        if (EventAwardMgr.log.IsErrorEnabled)
          EventAwardMgr.log.Error((object) nameof (ReLoad), ex);
        return false;
      }
      return true;
    }
  }
}
