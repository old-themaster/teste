// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.RateMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class RateMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock = new ReaderWriterLock();
    private static ArrayList m_RateInfos = new ArrayList();

    public static float GetRate(eRateType eType)
    {
      float num = 1f;
      RateMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        RateInfo rateInfoWithType = RateMgr.GetRateInfoWithType((int) eType);
        if (rateInfoWithType == null)
          return num;
        if ((double) rateInfoWithType.Rate == 0.0)
          return 1f;
        if (RateMgr.IsValid(rateInfoWithType))
          num = rateInfoWithType.Rate;
      }
      catch
      {
      }
      finally
      {
        RateMgr.m_lock.ReleaseReaderLock();
      }
      return num;
    }

    private static RateInfo GetRateInfoWithType(int type)
    {
      foreach (RateInfo rateInfo in RateMgr.m_RateInfos)
      {
        if (rateInfo.Type == type)
          return rateInfo;
      }
      return (RateInfo) null;
    }

    public static bool Init(GameServerConfig config)
    {
      RateMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
          RateMgr.m_RateInfos = serviceBussiness.GetRate(config.ServerID);
        return true;
      }
      catch (Exception ex)
      {
        if (RateMgr.log.IsErrorEnabled)
          RateMgr.log.Error((object) nameof (RateMgr), ex);
        return false;
      }
      finally
      {
        RateMgr.m_lock.ReleaseWriterLock();
      }
    }

    private static bool IsValid(RateInfo _RateInfo)
    {
      DateTime beginDay = _RateInfo.BeginDay;
      DateTime endDay = _RateInfo.EndDay;
      DateTime dateTime1 = _RateInfo.BeginDay;
      int year1 = dateTime1.Year;
      dateTime1 = DateTime.Now;
      int year2 = dateTime1.Year;
      if (year1 <= year2)
      {
        DateTime dateTime2 = DateTime.Now;
        int year3 = dateTime2.Year;
        dateTime2 = _RateInfo.EndDay;
        int year4 = dateTime2.Year;
        if (year3 <= year4)
        {
          dateTime2 = _RateInfo.BeginDay;
          int dayOfYear1 = dateTime2.DayOfYear;
          dateTime2 = DateTime.Now;
          int dayOfYear2 = dateTime2.DayOfYear;
          if (dayOfYear1 <= dayOfYear2)
          {
            dateTime2 = DateTime.Now;
            int dayOfYear3 = dateTime2.DayOfYear;
            dateTime2 = _RateInfo.EndDay;
            int dayOfYear4 = dateTime2.DayOfYear;
            if (dayOfYear3 <= dayOfYear4)
            {
              dateTime2 = _RateInfo.BeginTime;
              TimeSpan timeOfDay1 = dateTime2.TimeOfDay;
              dateTime2 = DateTime.Now;
              TimeSpan timeOfDay2 = dateTime2.TimeOfDay;
              if (timeOfDay1 <= timeOfDay2)
              {
                dateTime2 = DateTime.Now;
                TimeSpan timeOfDay3 = dateTime2.TimeOfDay;
                dateTime2 = _RateInfo.EndTime;
                TimeSpan timeOfDay4 = dateTime2.TimeOfDay;
                return timeOfDay3 <= timeOfDay4;
              }
            }
          }
        }
      }
      return false;
    }

    public static bool ReLoad() => RateMgr.Init(GameServer.Instance.Configuration);
  }
}
