// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.FairBattleRewardMgr
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
  public class FairBattleRewardMgr
  {
    private static Dictionary<int, FairBattleRewardInfo> _fairBattleRewards;
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static FairBattleRewardInfo FindLevel(int Level)
    {
      FairBattleRewardMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (FairBattleRewardMgr._fairBattleRewards.ContainsKey(Level))
          return FairBattleRewardMgr._fairBattleRewards[Level];
      }
      catch
      {
      }
      finally
      {
        FairBattleRewardMgr.m_lock.ReleaseReaderLock();
      }
      return (FairBattleRewardInfo) null;
    }

    public static FairBattleRewardInfo GetBattleDataByPrestige(int Prestige)
    {
      for (int key = FairBattleRewardMgr._fairBattleRewards.Values.Count - 1; key >= 0; --key)
      {
        if (Prestige >= FairBattleRewardMgr._fairBattleRewards[key].Prestige)
          return FairBattleRewardMgr._fairBattleRewards[key];
      }
      return (FairBattleRewardInfo) null;
    }

    public static int GetGP(int level) => FairBattleRewardMgr.MaxLevel() > level && level > 0 ? FairBattleRewardMgr.FindLevel(level - 1).Prestige : 0;

    public static int GetLevel(int GP)
    {
      if (GP >= FairBattleRewardMgr.FindLevel(FairBattleRewardMgr.MaxLevel()).Prestige)
        return FairBattleRewardMgr.MaxLevel();
      for (int Level = 1; Level <= FairBattleRewardMgr.MaxLevel(); ++Level)
      {
        if (GP < FairBattleRewardMgr.FindLevel(Level).Prestige)
          return Level - 1 != 0 ? Level - 1 : 1;
      }
      return 1;
    }

    public static bool Init()
    {
      try
      {
        FairBattleRewardMgr.m_lock = new ReaderWriterLock();
        FairBattleRewardMgr._fairBattleRewards = new Dictionary<int, FairBattleRewardInfo>();
        FairBattleRewardMgr.rand = new ThreadSafeRandom();
        return FairBattleRewardMgr.LoadFairBattleReward(FairBattleRewardMgr._fairBattleRewards);
      }
      catch (Exception ex)
      {
        if (FairBattleRewardMgr.log.IsErrorEnabled)
          FairBattleRewardMgr.log.Error((object) nameof (FairBattleRewardMgr), ex);
        return false;
      }
    }

    private static bool LoadFairBattleReward(Dictionary<int, FairBattleRewardInfo> Level)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (FairBattleRewardInfo battleRewardInfo in produceBussiness.GetAllFairBattleReward())
        {
          if (!Level.ContainsKey(battleRewardInfo.Level))
            Level.Add(battleRewardInfo.Level, battleRewardInfo);
        }
      }
      return true;
    }

    public static int MaxLevel()
    {
      if (FairBattleRewardMgr._fairBattleRewards == null)
        FairBattleRewardMgr.Init();
      return FairBattleRewardMgr._fairBattleRewards.Values.Count;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, FairBattleRewardInfo> Level = new Dictionary<int, FairBattleRewardInfo>();
        if (FairBattleRewardMgr.LoadFairBattleReward(Level))
        {
          FairBattleRewardMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            FairBattleRewardMgr._fairBattleRewards = Level;
            return true;
          }
          catch
          {
          }
          finally
          {
            FairBattleRewardMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (FairBattleRewardMgr.log.IsErrorEnabled)
          FairBattleRewardMgr.log.Error((object) "FairBattleMgr", ex);
      }
      return false;
    }
  }
}
