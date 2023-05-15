// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.LevelMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class LevelMgr
  {
    private static Dictionary<int, LevelInfo> _levels;
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static LevelInfo FindLevel(int Grade)
    {
      LevelMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (LevelMgr._levels.ContainsKey(Grade))
          return LevelMgr._levels[Grade];
      }
      catch
      {
      }
      finally
      {
        LevelMgr.m_lock.ReleaseReaderLock();
      }
      return (LevelInfo) null;
    }

    public static int GetGP(int level) => LevelMgr.MaxLevel > level && level > 0 ? LevelMgr.FindLevel(level - 1).GP : 0;

    public static int GetLevel(int GP)
    {
      if (GP >= LevelMgr.FindLevel(LevelMgr.MaxLevel).GP)
        return LevelMgr.MaxLevel;
      for (int Grade = 1; Grade <= LevelMgr.MaxLevel; ++Grade)
      {
        if (GP < LevelMgr.FindLevel(Grade).GP)
          return Grade - 1 != 0 ? Grade - 1 : 1;
      }
      return 1;
    }

    public static int IncreaseGP(int level, int totalGP) => LevelMgr.MaxLevel > level && level > 0 ? level * 12 : 0;

    public static bool Init()
    {
      try
      {
        LevelMgr.m_lock = new ReaderWriterLock();
        LevelMgr._levels = new Dictionary<int, LevelInfo>();
        LevelMgr.rand = new ThreadSafeRandom();
        return LevelMgr.LoadLevel(LevelMgr._levels);
      }
      catch (Exception ex)
      {
        if (LevelMgr.log.IsErrorEnabled)
          LevelMgr.log.Error((object) nameof (LevelMgr), ex);
        return false;
      }
    }

    private static bool LoadLevel(Dictionary<int, LevelInfo> Level)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (LevelInfo levelInfo in produceBussiness.GetAllLevel())
        {
          if (!Level.ContainsKey(levelInfo.Grade))
            Level.Add(levelInfo.Grade, levelInfo);
        }
      }
      return true;
    }

    public static int ReduceGP(int level, int totalGP)
    {
      if (LevelMgr.MaxLevel > level && level > 0)
      {
        totalGP -= LevelMgr.FindLevel(level - 1).GP;
        if (totalGP >= level * 12)
          return level * 12;
        if (totalGP >= 0)
          return totalGP;
      }
      return 0;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, LevelInfo> Level = new Dictionary<int, LevelInfo>();
        if (LevelMgr.LoadLevel(Level))
        {
          LevelMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            LevelMgr._levels = Level;
            return true;
          }
          catch
          {
          }
          finally
          {
            LevelMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (LevelMgr.log.IsErrorEnabled)
          LevelMgr.log.Error((object) nameof (LevelMgr), ex);
      }
      return false;
    }

    public static int MaxLevel => LevelMgr._levels.Count;
  }
}
