// Decompiled with JetBrains decompiler
// Type: YbxMgr.GMActiveMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using EntityDatabase.PlayerModels;
using EntityDatabase.ServerModels;
using Game.Server.GameObjects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace YbxMgr
{
  public static class GMActiveMgr
  {
    public static List<GM_Activity> _GM_Activitys;
    public static List<GM_Active_Condition> _GM_ActiveConditions;
    public static List<GM_Active_Reward> _GM_ActiveRewards;
    public static List<GM_Gift> _GM_Gifts;
    public static List<Ybx_85_NapXuTuan> _NapXuTuan;
    public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static bool Init()
    {
      try
      {
        ServerData serverData = new ServerData();
        PlayerData playerData = new PlayerData();
        GMActiveMgr._GM_Activitys = serverData.GM_Activity.ToList<GM_Activity>();
        GMActiveMgr._GM_ActiveConditions = serverData.GM_Active_Condition.ToList<GM_Active_Condition>();
        GMActiveMgr._GM_ActiveRewards = serverData.GM_Active_Reward.ToList<GM_Active_Reward>();
        GMActiveMgr._GM_Gifts = serverData.GM_Gift.ToList<GM_Gift>();
        GMActiveMgr._NapXuTuan = playerData.Ybx_85_NapXuTuan.OrderByDescending<Ybx_85_NapXuTuan, int>((Expression<Func<Ybx_85_NapXuTuan, int>>) (p => p.consume)).ToList<Ybx_85_NapXuTuan>();
        return true;
      }
      catch (Exception ex)
      {
        if (GMActiveMgr.log.IsErrorEnabled)
          GMActiveMgr.log.Error((object) nameof (Init), ex);
        return false;
      }
    }

    public static void CreateActiveForPlayer(
      int UserID,
      string _activeId,
      List<Ybx_88_GMActive> UserPlayer)
    {
      foreach (var data in GMActiveMgr._GM_Gifts.Where<GM_Gift>((Func<GM_Gift, bool>) (p => p.activityId == _activeId)).Select(m => new
      {
        giftbagId = m.giftbagId,
        giftbagOrder = m.giftbagOrder
      }).Distinct().OrderBy(p => p.giftbagOrder))
      {
        int num = 0;
        if (data.giftbagId == "6c161378-5ea5-e9b1-a441-be551df307e6")
          num = GMActiveMgr.getstatusID();
        Ybx_88_GMActive ybx88GmActive = new Ybx_88_GMActive()
        {
          UserID = UserID,
          activeId = _activeId,
          giftid = data.giftbagId,
          statusID = num,
          statusValue = 0,
          times = 0,
          allGiftGetTimes = 0
        };
        UserPlayer.Add(ybx88GmActive);
      }
    }

    public static void intItInfo(int UserID, string _activeId, List<Ybx_88_GMActive> UserPlayer)
    {
      IOrderedEnumerable<\u003C\u003Ef__AnonymousType4<string, int>> orderedEnumerable = GMActiveMgr._GM_Gifts.Where<GM_Gift>((Func<GM_Gift, bool>) (p => p.activityId == _activeId)).Select(m => new
      {
        giftbagId = m.giftbagId,
        giftbagOrder = m.giftbagOrder
      }).Distinct().OrderBy(p => p.giftbagOrder);
      int num1 = 0;
      foreach (var data in orderedEnumerable)
      {
        var item = data;
        Ybx_88_GMActive ybx88GmActive1 = UserPlayer.Where<Ybx_88_GMActive>((Func<Ybx_88_GMActive, bool>) (p => p.giftid == item.giftbagId)).FirstOrDefault<Ybx_88_GMActive>();
        if (ybx88GmActive1 == null)
        {
          int num2 = 0;
          if (item.giftbagId == "6c161378-5ea5-e9b1-a441-be551df307e6")
            num2 = GMActiveMgr.getstatusID();
          Ybx_88_GMActive ybx88GmActive2 = new Ybx_88_GMActive()
          {
            UserID = UserID,
            activeId = _activeId,
            giftid = item.giftbagId,
            statusID = num2,
            statusValue = num1,
            times = 0,
            allGiftGetTimes = 0
          };
          UserPlayer.Add(ybx88GmActive2);
        }
        else if (num1 == 0)
          num1 = ybx88GmActive1.statusValue;
      }
    }

    private static int getstatusID() => 10000 * DateTime.Now.Year + 100 * DateTime.Now.Month + DateTime.Now.Day;

    public static void AddTieuXu(GamePlayer player, int value)
    {
      string activeId = "2ae8558d-8080-20ed-5b40-69ee92ebfc2c";
      if (GMActiveMgr._GM_Activitys.Where<GM_Activity>((Func<GM_Activity, bool>) (p => p.activityId == activeId)).FirstOrDefault<GM_Activity>().IsActive)
      {
        List<Ybx_88_GMActive> list = player.Actives.GMActive.Where<Ybx_88_GMActive>((Func<Ybx_88_GMActive, bool>) (p => p.activeId == activeId && p.UserID == player.PlayerId)).ToList<Ybx_88_GMActive>();
        if (list.Count == 0)
        {
          GMActiveMgr.CreateActiveForPlayer(player.PlayerId, activeId, player.Actives.GMActive);
          list = player.Actives.GMActive.Where<Ybx_88_GMActive>((Func<Ybx_88_GMActive, bool>) (p => p.activeId == activeId)).ToList<Ybx_88_GMActive>();
        }
        foreach (Ybx_88_GMActive ybx88GmActive in list)
          ybx88GmActive.statusValue += value;
      }
      int Score = new Random().Next(1, 50);
      TeamMgr.AddSelfActive(player, Score, 2);
    }
  }
}
