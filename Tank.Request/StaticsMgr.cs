// Decompiled with JetBrains decompiler
// Type: Tank.Request.StaticsMgr
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using Tank.Request.CelebList;

namespace Tank.Request
{
  public static class StaticsMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static object _locker = new object();
    private static List<string> _list = new List<string>();
    private static int RegCount = 0;
    private static Timer _timer;
    private static int pid;
    private static int did;
    private static int sid;
    private static string _path;
    private static long _interval;
    private static int CelebBuildDay;
    public static string CurrentPath;

    public static void Setup()
    {
      StaticsMgr.CurrentPath = HttpContext.Current.Server.MapPath("~");
      StaticsMgr.CelebBuildDay = DateTime.Now.Day;
      StaticsMgr.pid = int.Parse(ConfigurationManager.AppSettings["PID"]);
      StaticsMgr.did = int.Parse(ConfigurationManager.AppSettings["DID"]);
      StaticsMgr.sid = int.Parse(ConfigurationManager.AppSettings["SID"]);
      StaticsMgr._path = ConfigurationManager.AppSettings["LogPath"];
      StaticsMgr._interval = (long) (int.Parse(ConfigurationManager.AppSettings["LogInterval"]) * 60 * 1000);
      StaticsMgr._timer = new Timer(new TimerCallback(StaticsMgr.OnTimer), (object) null, 0L, StaticsMgr._interval);
    }

    private static void OnTimer(object state)
    {
      try
      {
        lock (StaticsMgr._locker)
        {
          if (StaticsMgr._list.Count > 0)
          {
            using (FileStream fileStream = File.Open(string.Format("{0}\\payment-{1:D2}{2:D2}{3:D2}-{4:yyyyMMdd}.log", (object) StaticsMgr._path, (object) StaticsMgr.pid, (object) StaticsMgr.did, (object) StaticsMgr.sid, (object) DateTime.Now), FileMode.Append))
            {
              using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
              {
                while (StaticsMgr._list.Count != 0)
                {
                  streamWriter.WriteLine(StaticsMgr._list[0]);
                  StaticsMgr._list.RemoveAt(0);
                }
              }
            }
          }
          if (StaticsMgr.RegCount > 0)
          {
            using (FileStream fileStream = File.Open(string.Format("{0}\\reg-{1:D2}{2:D2}{3:D2}-{4:yyyyMMdd}.log", (object) StaticsMgr._path, (object) StaticsMgr.pid, (object) StaticsMgr.did, (object) StaticsMgr.sid, (object) DateTime.Now), FileMode.Append))
            {
              using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
              {
                string str = string.Format("{0},{1},{2},{3},{4}", (object) StaticsMgr.pid, (object) StaticsMgr.did, (object) StaticsMgr.sid, (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (object) StaticsMgr.RegCount);
                streamWriter.WriteLine(str);
                StaticsMgr.RegCount = 0;
              }
            }
          }
          int celebBuildDay = StaticsMgr.CelebBuildDay;
          DateTime now = DateTime.Now;
          int day = now.Day;
          if (celebBuildDay == day)
            return;
          now = DateTime.Now;
          if (now.Hour <= 2)
            return;
          now = DateTime.Now;
          if (now.Hour >= 6)
            return;
          now = DateTime.Now;
          StaticsMgr.CelebBuildDay = now.Day;
          StringBuilder stringBuilder = new StringBuilder();
          try
          {
            stringBuilder.Append(CelebByGpList.Build());
            stringBuilder.Append(CelebByDayGPList.Build());
            stringBuilder.Append(CelebByWeekGPList.Build());
            stringBuilder.Append(CelebByOfferList.Build());
            stringBuilder.Append(CelebByDayOfferList.Build());
            stringBuilder.Append(CelebByWeekOfferList.Build());
            stringBuilder.Append(CelebByDayFightPowerList.Build());
            stringBuilder.Append(CelebByConsortiaRiches.Build());
            stringBuilder.Append(CelebByConsortiaDayRiches.Build());
            stringBuilder.Append(CelebByConsortiaWeekRiches.Build());
            stringBuilder.Append(CelebByConsortiaHonor.Build());
            stringBuilder.Append(CelebByConsortiaDayHonor.Build());
            stringBuilder.Append(CelebByConsortiaWeekHonor.Build());
            stringBuilder.Append(CelebByConsortiaLevel.Build());
            stringBuilder.Append(CelebByDayBestEquip.Build());
            stringBuilder.Append(celebbyconsortiafightpower.Build());
            stringBuilder.Append(celebbyweekleaguescore.Build());
            ILog log = StaticsMgr.log;
            now = DateTime.Now;
            string str = "Complete auto update Celeb in " + now.ToString();
            log.Info((object) str);
          }
          catch (Exception ex)
          {
            stringBuilder.Append("CelebByList is Error!");
            StaticsMgr.log.Error((object) stringBuilder.ToString(), ex);
          }
        }
      }
      catch (Exception ex)
      {
        StaticsMgr.log.Error((object) "Save log error", ex);
      }
    }

    public static void Log(
      DateTime dt,
      string username,
      bool sex,
      int money,
      string payway,
      Decimal needMoney)
    {
      string str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (object) StaticsMgr.pid, (object) StaticsMgr.did, (object) StaticsMgr.sid, (object) dt.ToString("yyyy-MM-dd HH:mm:ss"), (object) username, (object) (sex ? 1 : 0), (object) money, (object) payway, (object) needMoney);
      lock (StaticsMgr._locker)
        StaticsMgr._list.Add(str);
    }

    public static void RegCountAdd()
    {
      lock (StaticsMgr._locker)
        ++StaticsMgr.RegCount;
    }

    public static void Stop()
    {
      StaticsMgr._timer.Dispose();
      StaticsMgr.OnTimer((object) null);
    }
  }
}
