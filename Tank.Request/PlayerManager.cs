// Decompiled with JetBrains decompiler
// Type: Tank.Request.PlayerManager
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace Tank.Request
{
  public class PlayerManager
  {
    private static Dictionary<string, PlayerManager.PlayerData> m_players = new Dictionary<string, PlayerManager.PlayerData>();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static object sys_obj = new object();
    private static int m_timeout = 30;
    private static Timer m_timer;

    public static void Setup()
    {
      PlayerManager.m_timeout = int.Parse(ConfigurationManager.AppSettings["LoginSessionTimeOut"]);
      PlayerManager.m_timer = new Timer(new TimerCallback(PlayerManager.CheckTimerCallback), (object) null, 0, 60000);
    }

    protected static bool CheckTimeOut(DateTime dt) => (DateTime.Now - dt).TotalMinutes > (double) PlayerManager.m_timeout;

    private static void CheckTimerCallback(object state)
    {
      lock (PlayerManager.sys_obj)
      {
        List<string> stringList = new List<string>();
        foreach (PlayerManager.PlayerData playerData in PlayerManager.m_players.Values)
        {
          if (PlayerManager.CheckTimeOut(playerData.Date))
            stringList.Add(playerData.Name);
        }
        foreach (string key in stringList)
          PlayerManager.m_players.Remove(key);
      }
    }

    public static void Add(string name, string pass)
    {
      lock (PlayerManager.sys_obj)
      {
        if (PlayerManager.m_players.ContainsKey(name))
        {
          PlayerManager.m_players[name].Name = name;
          PlayerManager.m_players[name].Pass = pass;
          PlayerManager.m_players[name].Date = DateTime.Now;
          PlayerManager.m_players[name].Count = 0;
        }
        else
          PlayerManager.m_players.Add(name, new PlayerManager.PlayerData()
          {
            Name = name,
            Pass = pass,
            Date = DateTime.Now
          });
      }
    }

    public static bool Login(string name, string pass)
    {
      lock (PlayerManager.sys_obj)
      {
        if (PlayerManager.m_players.ContainsKey(name))
          PlayerManager.log.Error((object) (name + "|" + PlayerManager.m_players[name].Pass));
        else
          PlayerManager.log.Error((object) ("NOHAVE " + name));
        if (!PlayerManager.m_players.ContainsKey(name) || !(PlayerManager.m_players[name].Pass == pass))
          return false;
        PlayerManager.PlayerData player = PlayerManager.m_players[name];
        if (player.Pass == pass && !PlayerManager.CheckTimeOut(player.Date))
          return true;
        PlayerManager.log.Error((object) (name + "|timeout:" + (object) PlayerManager.m_players[name].Date));
        return false;
      }
    }

    public static bool Update(string name, string pass)
    {
      lock (PlayerManager.sys_obj)
      {
        if (PlayerManager.m_players.ContainsKey(name))
        {
          PlayerManager.m_players[name].Pass = pass;
          ++PlayerManager.m_players[name].Count;
          return true;
        }
      }
      return false;
    }

    public static bool Remove(string name)
    {
      lock (PlayerManager.sys_obj)
        return PlayerManager.m_players.Remove(name);
    }

    public static bool GetByUserIsFirst(string name)
    {
      lock (PlayerManager.sys_obj)
      {
        if (PlayerManager.m_players.ContainsKey(name))
          return PlayerManager.m_players[name].Count == 0;
      }
      return false;
    }

    private class PlayerData
    {
      public string Name;
      public string Pass;
      public DateTime Date;
      public int Count;
    }
  }
}
