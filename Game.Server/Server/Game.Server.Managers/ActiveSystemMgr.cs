// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.ActiveSystemMgr
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F406BD-7233-42D4-AF91-73FA12644876
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\DLL\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
    public class ActiveSystemMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ReaderWriterLock m_lock;
        private static ThreadSafeRandom rand;
        private static Dictionary<int, ActiveSystemInfo> m_activeSystem = new Dictionary<int, ActiveSystemInfo>();
        private static Dictionary<int, ActivitySystemItemInfo> m_activySystemItem = new Dictionary<int, ActivitySystemItemInfo>();
        private static Dictionary<int, LanternriddlesInfo> m_lanternriddlesInfo = new Dictionary<int, LanternriddlesInfo>();
        private static List<LuckStarRewardRecordInfo> m_recordList = new List<LuckStarRewardRecordInfo>();
        protected static Timer m_scanRank;
        protected static Timer m_statusScanTimer;
        protected static Timer m_lanternriddlesScanTimer;
        private static bool m_sendOpenToClient;
        private static bool m_sendCloseToClient;
        private static bool m_lanternriddlesOpen;
        private static int m_periodType;
        private static int m_boatCompleteExp;
        private static int m_reduceToemUpGrace;
        private static bool m_IsReset;
        private static bool m_IsSendAward;
        private static bool m_IsBattleGoundOpen;
        private static bool m_IsLeagueOpen;
        private static bool m_IsFightFootballTime;
        private static bool m_x2Exp;
        private static bool m_x3Exp;
        private static int m_luckStarCountDown;

        public static List<LuckStarRewardRecordInfo> RecordList => ActiveSystemMgr.m_recordList;

        public static bool LanternriddlesOpen => ActiveSystemMgr.m_lanternriddlesOpen;

        public static int periodType => ActiveSystemMgr.m_periodType;

        public static int boatCompleteExp => ActiveSystemMgr.m_boatCompleteExp;

        public static int ReduceToemUpGrace => ActiveSystemMgr.m_reduceToemUpGrace;

        public static bool IsBattleGoundOpen
        {
            get => ActiveSystemMgr.m_IsBattleGoundOpen;
            set => ActiveSystemMgr.m_IsBattleGoundOpen = value;
        }

        public static bool IsLeagueOpen
        {
            get => ActiveSystemMgr.m_IsLeagueOpen;
            set => ActiveSystemMgr.m_IsLeagueOpen = value;
        }

        public static bool IsFightFootballTime
        {
            get => ActiveSystemMgr.m_IsFightFootballTime;
            set => ActiveSystemMgr.m_IsFightFootballTime = value;
        }


        public static DateTime EndDate => DateTime.Now.AddMilliseconds((double)(GameProperties.LightRiddleAnswerTime * 1000));

        public static bool Init()
        {
            bool flag;
            try
            {
                ActiveSystemMgr.m_lock = new ReaderWriterLock();
                ActiveSystemMgr.rand = new ThreadSafeRandom();
                ActiveSystemMgr.m_IsBattleGoundOpen = false;
                ActiveSystemMgr.m_IsLeagueOpen = false;
                ActiveSystemMgr.m_IsFightFootballTime = false;
                ActiveSystemMgr.m_lanternriddlesOpen = false;
                ActiveSystemMgr.m_sendOpenToClient = true;
                ActiveSystemMgr.m_sendCloseToClient = true;
                ActiveSystemMgr.m_luckStarCountDown = Math.Abs(60 - DateTime.Now.Minute - 30);
                ActiveSystemMgr.Setup();
                flag = ActiveSystemMgr.LoadSystermInfo();
            }
            catch (Exception ex)
            {
                if (ActiveSystemMgr.log.IsErrorEnabled)
                    ActiveSystemMgr.log.Error((object)nameof(ActiveSystemMgr), ex);
                flag = false;
            }
            return flag;
        }

        public static void AddOrUpdateLanternriddles(int playerID, LanternriddlesInfo Lanternriddles)
        {
            Lanternriddles.QuestViews = LightriddleQuestMgr.Get30LightriddleQuest();
            if (!ActiveSystemMgr.m_lanternriddlesInfo.ContainsKey(playerID))
                ActiveSystemMgr.m_lanternriddlesInfo.Add(playerID, Lanternriddles);
            else
                ActiveSystemMgr.m_lanternriddlesInfo[playerID] = Lanternriddles;
        }

        public static LanternriddlesInfo EnterLanternriddles(int playerID)
        {
            if (ActiveSystemMgr.m_lanternriddlesInfo.ContainsKey(playerID))
                return ActiveSystemMgr.GetLanternriddlesInfo(playerID);
            LanternriddlesInfo lanternriddlesInfo = new LanternriddlesInfo();
            lanternriddlesInfo.PlayerID = playerID;
            lanternriddlesInfo.QuestionIndex = 1;
            lanternriddlesInfo.QuestionView = 30;
            lanternriddlesInfo.DoubleFreeCount = GameProperties.LightRiddleFreeComboNum;
            lanternriddlesInfo.DoublePrice = GameProperties.LightRiddleComboMoney;
            lanternriddlesInfo.HitFreeCount = GameProperties.LightRiddleFreeHitNum;
            lanternriddlesInfo.HitPrice = GameProperties.LightRiddleHitMoney;
            lanternriddlesInfo.QuestViews = LightriddleQuestMgr.Get30LightriddleQuest();
            lanternriddlesInfo.MyInteger = 0;
            lanternriddlesInfo.QuestionNum = 0;
            lanternriddlesInfo.Option = -1;
            lanternriddlesInfo.IsHint = false;
            lanternriddlesInfo.IsDouble = false;
            lanternriddlesInfo.EndDate = ActiveSystemMgr.EndDate;
            ActiveSystemMgr.m_lanternriddlesInfo.Add(playerID, lanternriddlesInfo);
            return lanternriddlesInfo;
        }

        public static LanternriddlesInfo GetLanternriddlesInfo(int playerID)
        {
            if (!ActiveSystemMgr.m_lanternriddlesInfo.ContainsKey(playerID))
                return ActiveSystemMgr.CreateNullLanternriddlesInfo(playerID);
            if (ActiveSystemMgr.m_lanternriddlesInfo[playerID].CanNextQuest)
            {
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].EndDate = ActiveSystemMgr.EndDate;
                if (ActiveSystemMgr.m_lanternriddlesInfo[playerID].QuestionIndex > 1)
                    ++ActiveSystemMgr.m_lanternriddlesInfo[playerID].QuestionIndex;
            }
            if (ActiveSystemMgr.m_lanternriddlesInfo[playerID].EndDate.Date < DateTime.Now.Date)
            {
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].QuestionIndex = 1;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].QuestViews = LightriddleQuestMgr.Get30LightriddleQuest();
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].DoubleFreeCount = GameProperties.LightRiddleFreeComboNum;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].DoublePrice = GameProperties.LightRiddleComboMoney;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].HitFreeCount = GameProperties.LightRiddleFreeHitNum;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].HitPrice = GameProperties.LightRiddleHitMoney;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].MyInteger = 0;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].QuestionNum = 0;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].Option = -1;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].IsHint = false;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].IsDouble = false;
                ActiveSystemMgr.m_lanternriddlesInfo[playerID].EndDate = ActiveSystemMgr.EndDate;
            }
            return ActiveSystemMgr.m_lanternriddlesInfo[playerID];
        }

        public static LanternriddlesInfo CreateNullLanternriddlesInfo(int playerID) => new LanternriddlesInfo()
        {
            PlayerID = playerID,
            QuestionIndex = 30,
            QuestionView = 30,
            DoubleFreeCount = GameProperties.LightRiddleFreeComboNum,
            DoublePrice = GameProperties.LightRiddleComboMoney,
            HitFreeCount = GameProperties.LightRiddleFreeHitNum,
            HitPrice = GameProperties.LightRiddleHitMoney,
            MyInteger = 0,
            QuestionNum = 0,
            Option = -1,
            IsHint = true,
            IsDouble = true,
            EndDate = DateTime.Now
        };

        public static LanternriddlesInfo GetLanternriddles(int playerID) => ActiveSystemMgr.m_lanternriddlesInfo.ContainsKey(playerID) ? ActiveSystemMgr.m_lanternriddlesInfo[playerID] : (LanternriddlesInfo)null;

        public static void LanternriddlesAnswer(int playerID, int option)
        {
            if (!ActiveSystemMgr.m_lanternriddlesInfo.ContainsKey(playerID))
                return;
            ActiveSystemMgr.m_lanternriddlesInfo[playerID].Option = option;
        }

        public static void SendTCP(GSPacketIn pkg, int playerID) => WorldMgr.GetPlayerById(playerID)?.SendTCP(pkg);

        private static bool CanOpenLanternriddles()
        {
            Convert.ToDateTime(GameProperties.LightRiddleBeginDate);
            return DateTime.Now.Date < Convert.ToDateTime(GameProperties.LightRiddleEndDate).Date;
        }

        public static void UpdateLuckStarRewardRecord(int PlayerID, string nickName, int TemplateID, int Count, int isVip)
        {

            AddRewardRecord(PlayerID, nickName, TemplateID, Count, isVip);
            GameServer.Instance.LoginServer.SendLuckStarRewardRecord(PlayerID, nickName, TemplateID, Count, isVip);
        }

        public static void AddRewardRecord(int PlayerID, string nickName, int TemplateID, int Count, int isVip)
        {
            LuckStarRewardRecordInfo item = new LuckStarRewardRecordInfo { };
            var lista = m_recordList.Where(s => s.PlayerID == PlayerID);
            if (lista != null)
            {
                item.PlayerID = PlayerID;
                item.nickName = nickName;
                item.useStarNum = 0;
                item.TemplateID = TemplateID;
                item.Count = Count;
                item.isVip = isVip;

            }
            else
            {
                item.PlayerID = PlayerID;
                item.nickName = nickName;
                item.useStarNum = 0;
                item.TemplateID = TemplateID;
                item.Count = Count;
                item.isVip = isVip;
            }

            m_recordList.Add(item);

            using (PlayerBussiness bussines = new PlayerBussiness())
            {
                bussines.SaveLuckStarRankInfo(PlayerID, nickName, 0, isVip);
            }
        }


        public static void UpdateIsLeagueOpen(bool open)
        {
            ActiveSystemMgr.m_IsLeagueOpen = open;
            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
            {
                if (allPlayer != null)
                {
                    if (open)
                        allPlayer.Out.SendLeagueNotice(allPlayer.PlayerCharacter.ID, allPlayer.BattleData.MatchInfo.restCount, allPlayer.BattleData.maxCount, (byte)1);
                    else
                        allPlayer.Out.SendLeagueNotice(allPlayer.PlayerCharacter.ID, allPlayer.BattleData.MatchInfo.restCount, allPlayer.BattleData.maxCount, (byte)2);
                }
            }
        }

        public static bool CanExchange() => DateTime.Now.DayOfWeek == DayOfWeek.Sunday && ActiveSystemMgr.m_periodType == 2;


        public static bool CanX2Exp()
        {
            int gp = CommunalActiveMgr.GetGP(5);
            return DateTime.Now.DayOfWeek == DayOfWeek.Friday && ActiveSystemMgr.boatCompleteExp >= gp;
        }

        public static bool CanX3Exp()
        {
            int gp = CommunalActiveMgr.GetGP(6);
            return DateTime.Now.DayOfWeek == DayOfWeek.Saturday && ActiveSystemMgr.boatCompleteExp >= gp;
        }

        public static void UpdateBoatExpFromDB(int exp)
        {
            ActiveSystemMgr.m_boatCompleteExp = 0;
            ActiveSystemMgr.m_boatCompleteExp += exp;
        }

        public static void UpdateBoatExp(int exp) => ActiveSystemMgr.m_boatCompleteExp += exp;

        public static int FindMyRank(int ID)
        {
            int dragonBoatMinScore = GameProperties.DragonBoatMinScore;
            return ActiveSystemMgr.m_activeSystem.ContainsKey(ID) && ActiveSystemMgr.m_activeSystem[ID].totalScore >= dragonBoatMinScore ? ActiveSystemMgr.m_activeSystem[ID].myRank : -1;
        }

        public static int FindAreaMyRank(int ID)
        {
            int boatAreaMinScore = GameProperties.DragonBoatAreaMinScore;
            return ActiveSystemMgr.m_activeSystem.ContainsKey(ID) && ActiveSystemMgr.m_activeSystem[ID].totalScore >= boatAreaMinScore ? ActiveSystemMgr.m_activeSystem[ID].myRank : -1;
        }

        public static List<ActiveSystemInfo> SelectTopTenCurrenServer(int condition)
        {
            List<ActiveSystemInfo> activeSystemInfoList = new List<ActiveSystemInfo>();
            foreach (KeyValuePair<int, ActiveSystemInfo> keyValuePair in (IEnumerable<KeyValuePair<int, ActiveSystemInfo>>)ActiveSystemMgr.m_activeSystem.Where<KeyValuePair<int, ActiveSystemInfo>>((Func<KeyValuePair<int, ActiveSystemInfo>, bool>)(pair => pair.Value.totalScore >= condition)).OrderByDescending<KeyValuePair<int, ActiveSystemInfo>, int>((Func<KeyValuePair<int, ActiveSystemInfo>, int>)(pair => pair.Value.totalScore)))
            {
                if (activeSystemInfoList.Count != 10)
                    activeSystemInfoList.Add(keyValuePair.Value);
                else
                    break;
            }
            return activeSystemInfoList;
        }

        public static List<ActiveSystemInfo> SelectTopTenAllServer(int condition)
        {
            List<ActiveSystemInfo> activeSystemInfoList = new List<ActiveSystemInfo>();
            foreach (KeyValuePair<int, ActiveSystemInfo> keyValuePair in (IEnumerable<KeyValuePair<int, ActiveSystemInfo>>)ActiveSystemMgr.m_activeSystem.Where<KeyValuePair<int, ActiveSystemInfo>>((Func<KeyValuePair<int, ActiveSystemInfo>, bool>)(pair => pair.Value.totalScore >= condition)).OrderByDescending<KeyValuePair<int, ActiveSystemInfo>, int>((Func<KeyValuePair<int, ActiveSystemInfo>, int>)(pair => pair.Value.totalScore)))
            {
                if (activeSystemInfoList.Count != 10)
                    activeSystemInfoList.Add(keyValuePair.Value);
                else
                    break;
            }
            return activeSystemInfoList;
        }
        public static void CheckPeriod()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)

            {
                DateTime now = DateTime.Now;
                if (now.DayOfWeek == DayOfWeek.Monday)
                {
                    if (!ActiveSystemMgr.m_IsReset)

                        ActiveSystemMgr.m_periodType = 1;
                    ActiveSystemMgr.m_reduceToemUpGrace = 0;
                }
                else
                {
                    now = DateTime.Now;
                    if (now.DayOfWeek == DayOfWeek.Friday && !ActiveSystemMgr.m_x2Exp)
                    {
                        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                        {
                            if (allPlayer != null)
                                allPlayer.CanX2Exp = true;
                        }
                        ActiveSystemMgr.m_x2Exp = true;
                    }
                    else
                    {
                        now = DateTime.Now;
                        if (now.DayOfWeek != DayOfWeek.Saturday || ActiveSystemMgr.m_x3Exp)
                            return;
                        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                        {
                            if (allPlayer != null)
                                allPlayer.CanX3Exp = true;
                        }
                        ActiveSystemMgr.m_x3Exp = true;
                        ActiveSystemMgr.m_x2Exp = false;
                    }
                }
            }
        }


        public static void Setup()
        {
            try
            {
                ActiveSystemMgr.m_periodType = 1;
                ActiveSystemMgr.m_boatCompleteExp = 0;
                ActiveSystemMgr.m_reduceToemUpGrace = 0;
                CommunalActiveInfo communalActive = CommunalActiveMgr.FindCommunalActive(1);
                if (communalActive != null)
                {
                    ActiveSystemMgr.m_IsSendAward = communalActive.IsSendAward;
                    ActiveSystemMgr.m_IsReset = communalActive.IsReset;
                }
                else
                {
                    ActiveSystemMgr.m_IsSendAward = true;
                    ActiveSystemMgr.m_IsReset = true;
                }
                ActiveSystemMgr.CheckPeriod();
                ActiveSystemMgr.BeginTimer();
            }
            catch (Exception ex)
            {
                if (!ActiveSystemMgr.log.IsErrorEnabled)
                    return;
                ActiveSystemMgr.log.Error((object)"ActiveSystemMgr Setup", ex);
            }
        }

        private static bool LoadSystermInfo()
        {
            try
            {
                ActiveSystemMgr.m_activeSystem = new Dictionary<int, ActiveSystemInfo>();
                ActiveSystemMgr.m_activySystemItem = new Dictionary<int, ActivitySystemItemInfo>();
                long num = 0;
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    foreach (ActiveSystemInfo activeSystemInfo in playerBussiness.GetAllActiveSystemData())
                    {
                        if (!ActiveSystemMgr.m_activeSystem.ContainsKey(activeSystemInfo.UserID))
                        {
                            num += (long)activeSystemInfo.totalScore;
                            ActiveSystemMgr.m_activeSystem.Add(activeSystemInfo.UserID, activeSystemInfo);
                        }
                    }
                    foreach (ActivitySystemItemInfo activitySystemItemInfo in playerBussiness.GetAllActivitySystemItem())
                    {
                        if (!ActiveSystemMgr.m_activySystemItem.ContainsKey(activitySystemItemInfo.ID))
                            ActiveSystemMgr.m_activySystemItem.Add(activitySystemItemInfo.ID, activitySystemItemInfo);
                    }
                    if (num > (long)int.MaxValue)
                        num = (long)int.MaxValue;
                    ActiveSystemMgr.UpdateBoatExpFromDB((int)num);
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ActiveSystemMgr.log.IsErrorEnabled)
                    ActiveSystemMgr.log.Error((object)nameof(ActiveSystemMgr), ex);
            }
            return false;
        }

        public static List<SqlDataProvider.Data.ItemInfo> GetPyramidAward(int layer)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            List<ActivitySystemItemInfo> activitySystemItemInfoList = new List<ActivitySystemItemInfo>();
            List<ActivitySystemItemInfo> systemItemByLayer = ActiveSystemMgr.FindActivitySystemItemByLayer(layer);
            int num1 = 1;
            int maxRound = ThreadSafeRandom.NextStatic(systemItemByLayer.Select<ActivitySystemItemInfo, int>((Func<ActivitySystemItemInfo, int>)(s => s.Random)).Max());
            List<ActivitySystemItemInfo> list = systemItemByLayer.Where<ActivitySystemItemInfo>((Func<ActivitySystemItemInfo, bool>)(s => s.Random >= maxRound)).ToList<ActivitySystemItemInfo>();
            int num2 = list.Count<ActivitySystemItemInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ActiveSystemMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ActivitySystemItemInfo activitySystemItemInfo = list[randomUnrepeat];
                    activitySystemItemInfoList.Add(activitySystemItemInfo);
                }
            }
            foreach (ActivitySystemItemInfo activitySystemItemInfo in activitySystemItemInfoList)
            {
                SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(activitySystemItemInfo.TemplateID), activitySystemItemInfo.Count, 102);
                fromTemplate.TemplateID = activitySystemItemInfo.TemplateID;
                fromTemplate.IsBinds = activitySystemItemInfo.IsBinds;
                fromTemplate.ValidDate = activitySystemItemInfo.ValidDate;
                fromTemplate.Count = activitySystemItemInfo.Count;
                fromTemplate.StrengthenLevel = activitySystemItemInfo.StrengthenLevel;
                fromTemplate.AttackCompose = 0;
                fromTemplate.DefendCompose = 0;
                fromTemplate.AgilityCompose = 0;
                fromTemplate.LuckCompose = 0;
                itemInfoList.Add(fromTemplate);
            }
            return itemInfoList;
        }

        public static List<ActivitySystemItemInfo> FindActivitySystemItemByLayer(
          int layer)
        {
            List<ActivitySystemItemInfo> activitySystemItemInfoList = new List<ActivitySystemItemInfo>();
            if (ActiveSystemMgr.m_activySystemItem != null)
            {
                foreach (ActivitySystemItemInfo activitySystemItemInfo in ActiveSystemMgr.m_activySystemItem.Values)
                {
                    if (activitySystemItemInfo.Quality == layer && activitySystemItemInfo.ActivityType == 8)
                        activitySystemItemInfoList.Add(activitySystemItemInfo);
                }
            }
            return activitySystemItemInfoList;
        }

        public static List<ActivitySystemItemInfo> FindGrowthPackage(int layer)
        {
            List<ActivitySystemItemInfo> list = new List<ActivitySystemItemInfo>();
            if (ActiveSystemMgr.m_activySystemItem != null)
            {
                foreach (ActivitySystemItemInfo current in ActiveSystemMgr.m_activySystemItem.Values)
                {
                    if (current.Quality == layer && current.ActivityType == 20)
                    {
                        list.Add(current);
                    }
                }
            }
            return list;
        }

        public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            int[] numArray = new int[count];
            for (int index1 = 0; index1 < count; ++index1)
            {
                int num1 = ThreadSafeRandom.NextStatic(minValue, maxValue + 1);
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

        public static void BeginTimer()
        {
            int num1 = 1800000;
            if (ActiveSystemMgr.m_scanRank == null)
                ActiveSystemMgr.m_scanRank = new Timer(new TimerCallback(ActiveSystemMgr.TimeCheck), (object)null, num1, num1);
            else
                ActiveSystemMgr.m_scanRank.Change(num1, num1);
            int num2 = 60000;
            if (ActiveSystemMgr.m_statusScanTimer == null)
                ActiveSystemMgr.m_statusScanTimer = new Timer(new TimerCallback(ActiveSystemMgr.StatusScan), (object)null, num2, num2);
            else
                ActiveSystemMgr.m_statusScanTimer.Change(num2, num2);
            int num3 = 60000;
            if (ActiveSystemMgr.m_lanternriddlesScanTimer == null)
                ActiveSystemMgr.m_lanternriddlesScanTimer = new Timer(new TimerCallback(ActiveSystemMgr.LanternriddlesScan), (object)null, num3, num3);
            else
                ActiveSystemMgr.m_lanternriddlesScanTimer.Change(num3, num3);
        }

        protected static void LanternriddlesScan(object sender)
        {
            try
            {
                ActiveSystemMgr.log.Info((object)"Begin Lanternriddles CheckPeriod....");
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                if (ActiveSystemMgr.CanOpenLanternriddles())
                {
                    int hour1 = DateTime.Now.Hour;
                    DateTime dateTime1 = Convert.ToDateTime(GameProperties.LightRiddleBeginTime);
                    DateTime dateTime2 = Convert.ToDateTime(GameProperties.LightRiddleEndTime);
                    int hour2 = dateTime1.Hour;
                    int hour3 = dateTime2.Hour;
                    if (hour1 >= hour2 && hour1 < hour3)
                    {
                        ActiveSystemMgr.m_lanternriddlesOpen = true;
                        if (ActiveSystemMgr.m_sendOpenToClient)
                        {
                            ActiveSystemMgr.LanternriddlesOpenClose();
                            ActiveSystemMgr.m_sendOpenToClient = false;
                        }
                    }
                    else
                    {
                        ActiveSystemMgr.m_lanternriddlesOpen = false;
                        if (hour1 >= hour3 && ActiveSystemMgr.m_sendCloseToClient)
                        {
                            ActiveSystemMgr.LanternriddlesOpenClose();
                            ActiveSystemMgr.m_sendCloseToClient = false;
                        }
                    }
                }
                Thread.CurrentThread.Priority = priority;
                int num = Environment.TickCount - tickCount;
                ActiveSystemMgr.log.Info((object)"End Lanternriddles CheckPeriod....");
            }
            catch (Exception ex)
            {
                ActiveSystemMgr.log.Error((object)"lanternriddlesScan ", ex);
            }
        }

        public static void LanternriddlesOpenClose()
        {
            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                allPlayer?.Out.SendLanternriddlesOpen(allPlayer.PlayerId, ActiveSystemMgr.m_lanternriddlesOpen);
        }

        protected static void StatusScan(object sender)
        {
            try
            {
                ActiveSystemMgr.log.Info((object)"Begin ActiveSystem CheckPeriod....");
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                ActiveSystemMgr.CheckPeriod();
                Thread.CurrentThread.Priority = priority;
                int num = Environment.TickCount - tickCount;
                ActiveSystemMgr.log.Info((object)"End ActiveSystem CheckPeriod....");
            }
            catch (Exception ex)
            {
                ActiveSystemMgr.log.Error((object)"StatusScan ", ex);
            }
        }

        protected static void TimeCheck(object sender)
        {
            try
            {
                ActiveSystemMgr.log.Info((object)"Begin ActiveSystem TimeCheck....");
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                ActiveSystemMgr.LoadSystermInfo();
                Thread.CurrentThread.Priority = priority;
                int num = Environment.TickCount - tickCount;
                ActiveSystemMgr.log.Info((object)"End ActiveSystem TimeCheck....");
            }
            catch (Exception ex)
            {
                ActiveSystemMgr.log.Error((object)"StatusScan ", ex);
            }
        }

        public static void StopAllTimer()
        {
            if (ActiveSystemMgr.m_scanRank != null)
            {
                ActiveSystemMgr.m_scanRank.Dispose();
                ActiveSystemMgr.m_scanRank = (Timer)null;
            }
            if (ActiveSystemMgr.m_lanternriddlesScanTimer != null)
            {
                ActiveSystemMgr.m_lanternriddlesScanTimer.Dispose();
                ActiveSystemMgr.m_lanternriddlesScanTimer = (Timer)null;
            }
            if (ActiveSystemMgr.m_statusScanTimer == null)
                return;
            ActiveSystemMgr.m_statusScanTimer.Dispose();
            ActiveSystemMgr.m_statusScanTimer = (Timer)null;
        }
    }
}
