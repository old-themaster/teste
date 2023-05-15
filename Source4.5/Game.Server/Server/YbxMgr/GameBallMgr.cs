// Decompiled with JetBrains decompiler
// Type: YbxMgr.GameBallMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using EntityDatabase.PlayerModels;
using EntityDatabase.ServerModels;
using Game.Base.Packets;
using Game.Server;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace YbxMgr
{
  public static class GameBallMgr
  {
    public static List<Ybx_99_BallGame> BallGameData = new List<Ybx_99_BallGame>();
    public static List<NewYearRankLists> RankLists;
    public static List<NewYearPlayerRecords> PlayerRecords;
    public static List<Ts_NewyearPointReward> _NewyearPointRewards;
    private static Timer m_timer;
    public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static bool Init()
    {
      ServerData serverData = new ServerData();
      try
      {
        int num = 60000;
        GameBallMgr._NewyearPointRewards = serverData.Ts_NewyearPointReward.ToList<Ts_NewyearPointReward>();
        if (GameBallMgr.m_timer == null)
          GameBallMgr.m_timer = new Timer(new TimerCallback(GameBallMgr.SendToWorld), (object) null, num, num);
        else
          GameBallMgr.m_timer.Change(num, num);
        return true;
      }
      catch (Exception ex)
      {
        if (GameBallMgr.log.IsErrorEnabled)
          GameBallMgr.log.Error((object) nameof (Init), ex);
        return false;
      }
    }

    public static void SetData(GSPacketIn pkg)
    {
      GameBallMgr.RankLists = new List<NewYearRankLists>();
      int num1 = pkg.ReadInt();
      for (int index = 0; index < num1; ++index)
      {
        int num2 = pkg.ReadInt();
        string str1 = pkg.ReadString();
        int num3 = pkg.ReadInt();
        string str2 = pkg.ReadString();
        int num4 = pkg.ReadInt();
        int num5 = pkg.ReadInt();
        int num6 = pkg.ReadInt();
        GameBallMgr.RankLists.Add(new NewYearRankLists()
        {
          ID = num2,
          ServerId = num4,
          ServerName = str2,
          UserID = num5,
          NickName = str1,
          score = num3,
          rank = num6
        });
      }
      GameBallMgr.PlayerRecords = new List<NewYearPlayerRecords>();
      int num7 = pkg.ReadInt();
      for (int index = 0; index < num7; ++index)
        GameBallMgr.PlayerRecords.Add(new NewYearPlayerRecords()
        {
          NickName = pkg.ReadString(),
          score = pkg.ReadInt()
        });
      Console.WriteLine("Cap nhat dau truong mua he!");
    }

    private static void SendToWorld(object state) => GameServer.Instance.LoginServer.SendGetGameBallRank();
  }
}
