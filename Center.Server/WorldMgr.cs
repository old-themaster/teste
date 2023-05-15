// Decompiled with JetBrains decompiler
// Type: Center.Server.WorldMgr
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\WONDERTANK vReZero\Emulator\Center\Center.Server.dll
using Bussiness;
using Bussiness.Managers;
using Center.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Center.Server
{
  public class WorldMgr
  {
    private static object _syncStop = new object();
    public static DateTime begin_time;
    public static string[] bossResourceId = new string[4]
    {
      "1",
      "2",
      "2",
      "4"
    };
    public static long current_blood = 0;
    public static int currentPVE_ID;
    public static DateTime end_time;
    public static int fight_time;
    public static bool fightOver;
    public static bool IsLeagueOpen;
    public static DateTime LeagueOpenTime;
    public static bool IsBattleGoundOpen;
    public static DateTime BattleGoundOpenTime;
    public static DateTime FightFootballTime;
    public static bool IsFightFootballTime;
    public static bool CanSendLightriddleAward;
    public static bool CanSendLuckyStarAward;
    private static int LuckStarCountDown;
    private static Dictionary<string, RankingLightriddleInfo> m_lightriddleRankList;
    private static Dictionary<int, LuckStarRewardRecordInfo> m_luckStarRewardRecordInfo;
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<string, RankingPersonInfo> m_rankList;
    public static readonly long MAX_BLOOD = 2100000000;
    public static string[] name = new string[4]
    {
      "Chefão",
      "Chefe do Mundo",
      "Conde Drácula",
      "Capitão do Futebol"
    };
    public static List<string> NotceList = new List<string>();
    public static int[] Pve_Id = new int[4]
    {
      1243,
      30001,
      30002,
      30004
    };

    public static bool roomClose;
    private static readonly int worldbossTime = 60;
    public static bool worldOpen;

    public static bool CheckName(string NickName) => WorldMgr.m_rankList.Keys.Contains<string>(NickName);

    public static RankingPersonInfo GetSingleRank(string name) => WorldMgr.m_rankList[name];

    public static bool LoadNotice(string path)
    {
      string str1 = path + WorldMgr.SystemNoticeFile;
      if (!File.Exists(str1))
      {
        WorldMgr.log.Error((object) ("SystemNotice file : " + str1 + " not found !"));
      }
      else
      {
        try
        {
          foreach (XElement node in XDocument.Load(str1).Root.Nodes())
          {
            try
            {
              int.Parse(node.Attribute((XName) "id").Value);
              string str2 = node.Attribute((XName) "notice").Value;
              WorldMgr.NotceList.Add(str2);
            }
            catch (Exception ex)
            {
              WorldMgr.log.Error((object) "BattleMgr setup error:", ex);
            }
          }
        }
        catch (Exception ex)
        {
          WorldMgr.log.Error((object) "BattleMgr setup error:", ex);
        }
      }
      WorldMgr.log.InfoFormat("Total {0} syterm notice loaded.", (object) WorldMgr.NotceList.Count);
      return true;
    }

    public static void ReduceBlood(int value)
    {
      if (WorldMgr.current_blood <= 0L)
        return;
      WorldMgr.current_blood -= (long) value;
    }

    public static List<RankingPersonInfo> SelectTopTen()
    {
      List<RankingPersonInfo> rankingPersonInfoList = new List<RankingPersonInfo>();
      foreach (KeyValuePair<string, RankingPersonInfo> keyValuePair in (IEnumerable<KeyValuePair<string, RankingPersonInfo>>) WorldMgr.m_rankList.OrderByDescending<KeyValuePair<string, RankingPersonInfo>, int>((Func<KeyValuePair<string, RankingPersonInfo>, int>) (pair => pair.Value.Damage)))
      {
        if (rankingPersonInfoList.Count == 10)
          return rankingPersonInfoList;
        rankingPersonInfoList.Add(keyValuePair.Value);
      }
      return rankingPersonInfoList;
    }
        public static List<LuckStarRewardRecordInfo> GetAllLuckyStarRank()
        {
            List<LuckStarRewardRecordInfo> list = new List<LuckStarRewardRecordInfo>();
            foreach (LuckStarRewardRecordInfo info in m_luckStarRewardRecordInfo.Values)
            {
                list.Add(info);
            }
            return list;
        }
        public static void SetupWorldBoss(int id)
    {
      WorldMgr.current_blood = WorldMgr.MAX_BLOOD;
      WorldMgr.begin_time = DateTime.Now;
      WorldMgr.end_time = WorldMgr.begin_time.AddDays(1.0);
      WorldMgr.fight_time = WorldMgr.worldbossTime - WorldMgr.begin_time.Minute;
      WorldMgr.fightOver = false;
      WorldMgr.roomClose = false;
      WorldMgr.currentPVE_ID = id;
      WorldMgr.worldOpen = true;
    }

        public static void WorldBossFightOver()
        {
            WorldMgr.fightOver = true;
        }
        public static void WorldBossRoomClose()
        {
            WorldMgr.roomClose = true;
        }
        public static void UpdateFightTime()
        {
            if (!WorldMgr.fightOver)
            {
                WorldMgr.fight_time = WorldMgr.worldbossTime - WorldMgr.begin_time.Minute;
            }
        }
        public static void WorldBossClose()
        {
            WorldMgr.worldOpen = false;
        }
        public static void WorldBossClearRank()
        {
            WorldMgr.m_rankList.Clear();
        }

        public static bool Start()
    {
      try
      {
        WorldMgr.CanSendLightriddleAward = true;
        WorldMgr.CanSendLuckyStarAward = true;
        WorldMgr.m_lightriddleRankList = new Dictionary<string, RankingLightriddleInfo>();
        WorldMgr.m_luckStarRewardRecordInfo = new Dictionary<int, LuckStarRewardRecordInfo>();
        WorldMgr.m_rankList = new Dictionary<string, RankingPersonInfo>();
        WorldMgr.current_blood = WorldMgr.MAX_BLOOD;
        WorldMgr.begin_time = DateTime.Now;
        WorldMgr.LeagueOpenTime = DateTime.Now;
        WorldMgr.BattleGoundOpenTime = DateTime.Now;
        WorldMgr.FightFootballTime = DateTime.Now;
        WorldMgr.ResetLuckStar();
        WorldMgr.end_time = WorldMgr.begin_time.AddDays(1.0);
        WorldMgr.fightOver = true;
        WorldMgr.roomClose = true;
        WorldMgr.worldOpen = false;
        WorldMgr.IsLeagueOpen = false;
        WorldMgr.IsBattleGoundOpen = false;
        WorldMgr.IsFightFootballTime = false;
       return WorldMgr.LoadNotice("");
      }
      catch (Exception ex)
      {
        WorldMgr.log.ErrorFormat("Load server list from db failed:{0}", (object) ex);
        return false;
      }
    }
        public static void ResetLightriddleRank()
        {
            WorldMgr.m_lightriddleRankList.Clear();
        }
        public static void ResetLuckStar()
        {
            WorldMgr.m_luckStarRewardRecordInfo.Clear();
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                playerBussiness.ResetLuckStarRank();
            }
            WorldMgr.LuckStarCountDown = Math.Abs(60 - DateTime.Now.Minute - 30);
        }

        public static void SavekyStarToDatabase()
        {
            List<LuckStarRewardRecordInfo> allLuckyStarRank = GetAllLuckyStarRank();
            using (PlayerBussiness bussiness = new PlayerBussiness())
            {
                foreach (LuckStarRewardRecordInfo info in allLuckyStarRank)
                {
                  //  bussiness.SaveLuckStarRankInfo(info);
                }
            }
        }
        public static void UpdateLuckStarRewardRecord(int PlayerID, string nickName, int TemplateID, int Count, int isVip)
        {
            if (m_luckStarRewardRecordInfo.Keys.Contains<int>(PlayerID))
            {
                LuckStarRewardRecordInfo local1 = m_luckStarRewardRecordInfo[PlayerID];
                local1.useStarNum++;
            }
            else
            {
                LuckStarRewardRecordInfo info = new LuckStarRewardRecordInfo
                {
                    PlayerID = PlayerID,
                    nickName = nickName,
                    useStarNum = 1,
                    TemplateID = TemplateID,
                    Count = Count,
                    isVip = isVip
                };
                m_luckStarRewardRecordInfo.Add(PlayerID, info);
            }
        }

        public static void SaveLuckyStarRewardRecord()
        {
            int hour = DateTime.Now.Hour;
            if (LuckStarCountDown > 0)
            {
                LuckStarCountDown--;
            }
            if (LuckStarCountDown == 0)
            {
                if (CanSendLuckyStarAward)
                {
                    SavekyStarToDatabase();
                    LuckStarCountDown = 30;
                }
                else
                {
                    LuckStarCountDown = -1;
                }
            }
        }
        public static void SendLuckyStarTopTenAward()
        {
            int minUseNum = GameProperties.MinUseNum;
            using (PlayerBussiness bussiness = new PlayerBussiness())
            {
                foreach (LuckStarRewardRecordInfo info in bussiness.GetLuckStarTopTenRank(minUseNum))
                {
                    string format = "Ph?n thu?ng h?ng {0} sobrenome?t d?ng Como pode m?n";
                    List<LuckyStartToptenAwardInfo> luckyStartAwardByRank = WorldEventMgr.GetLuckyStartAwardByRank(info.rank);
                    List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
                    foreach (LuckyStartToptenAwardInfo info2 in luckyStartAwardByRank)
                    {
                        SqlDataProvider.Data.ItemInfo item = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info2.TemplateID), 1, 0x69);
                        item.IsBinds = info2.IsBinds;
                        item.ValidDate = info2.Validate;
                        item.Count = info2.Count;
                        item.IsUsed = true;
                        item.BeginDate = DateTime.Now;
                        infos.Add(item);
                    }
                    format = string.Format(format, info.rank);
                    WorldEventMgr.SendItemsToMail(infos, info.PlayerID, info.nickName, format);
                    EntertamentModeMgr.MailNotice(info.PlayerID);
                }
            }
            CanSendLuckyStarAward = false;
        }

       /* public static void SendLightriddleTopEightAward()
        {
            List<RankingLightriddleInfo> list = WorldMgr.SelectTopEight();
            foreach (RankingLightriddleInfo current in list)
            {
                string text = "Classificação de recompensa {0} atividade completa";
                List<LuckyStartToptenAwardInfo> lanternriddlesAwardByRank = WorldEventMgr.GetLanternriddlesAwardByRank(current.Rank);
                List<ItemInfo> list2 = new List<ItemInfo>();
                foreach (LuckyStartToptenAwardInfo current2 in lanternriddlesAwardByRank)
                {
                    ItemTemplateInfo goods = ItemMgr.FindItemTemplate(current2.TemplateID);
                    ItemInfo itemInfo = ItemInfo.CreateFromTemplate(goods, 1, 105);
                    itemInfo.IsBinds = current2.IsBinds;
                    itemInfo.ValidDate = current2.Validate;
                    itemInfo.Count = current2.Count;
                    list2.Add(itemInfo);
                }
                text = string.Format(text, current.Rank);
                WorldEventMgr.SendItemsToMail(list2, current.PlayerId, current.NickName, text);
                EntertamentModeMgr.MailNotice(list2.PlayerId);
            }
            WorldMgr.CanSendLightriddleAward = false;
        }*/

        public static void SendLightriddleTopEightAward()
        {
            foreach (RankingLightriddleInfo info in SelectTopEight())
            {
                string format = "Classificação de recompensa {0} atividade completa";
                List<LuckyStartToptenAwardInfo> lanternriddlesAwardByRank = WorldEventMgr.GetLanternriddlesAwardByRank(info.Rank);
                List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
                foreach (LuckyStartToptenAwardInfo info2 in lanternriddlesAwardByRank)
                {
                    SqlDataProvider.Data.ItemInfo item = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info2.TemplateID), 1, 0x69);
                    item.IsBinds = info2.IsBinds;
                    item.ValidDate = info2.Validate;
                    item.Count = info2.Count;
                    item.IsUsed = true;
                    item.BeginDate = DateTime.Now;
                    infos.Add(item);
                }
                format = string.Format(format, info.Rank);
                WorldEventMgr.SendItemsToMail(infos, info.PlayerId, info.NickName, format);
                EntertamentModeMgr.MailNotice(info.PlayerId);
            }
            CanSendLightriddleAward = false;
        }
        public static List<RankingLightriddleInfo> SelectTopEight()
        {
            List<RankingLightriddleInfo> list = new List<RankingLightriddleInfo>();
            IOrderedEnumerable<KeyValuePair<string, RankingLightriddleInfo>> orderedEnumerable =
                from pair in WorldMgr.m_lightriddleRankList
                where pair.Value.Integer > 1
                orderby pair.Value.Integer descending
                select pair;
            foreach (KeyValuePair<string, RankingLightriddleInfo> current in orderedEnumerable)
            {
                if (list.Count == 8)
                {
                    break;
                }
                list.Add(current.Value);
            }
            return list;
        }
        public static void UpdateRank(int damage, int honor, string nickName)
    {
      if (WorldMgr.m_rankList.Keys.Contains<string>(nickName))
      {
        WorldMgr.m_rankList[nickName].Damage += damage;
        WorldMgr.m_rankList[nickName].Honor += honor;
      }
      else
      {
        RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
        {
          ID = WorldMgr.m_rankList.Count + 1,
          Name = nickName,
          Damage = damage,
          Honor = honor
        };
        WorldMgr.m_rankList.Add(nickName, rankingPersonInfo);
      }
    }
  
    private static string SystemNoticeFile => ConfigurationManager.AppSettings["SystemNoticePath"];
  }
}
