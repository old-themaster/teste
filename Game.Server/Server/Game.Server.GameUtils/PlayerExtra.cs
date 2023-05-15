using Bussiness;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Base.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
    public class PlayerExtra
  {
    protected Timer _onlineGameTimer;
    protected Timer _hotSpringTimer;
    private int[] buffData = new int[7]
    {
      1,
      2,
      3,
      4,
      5,
      6,
      7
    };
    private Dictionary<int, EventRewardProcessInfo> dictionary_0;
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public Dictionary<int, int> m_kingBlessInfo;
    private UsersExtraInfo m_Info;
    protected object m_lock = new object();
    protected GamePlayer m_player;
    private bool m_saveToDb;
    private List<EventAwardInfo> m_searchGoodItems;
    public int MapId = 1;
    private int[] positions = new int[34];
    private static ThreadSafeRandom rand = new ThreadSafeRandom();
    public readonly DateTime reChangeEnd;
    public readonly DateTime reChangeStart;
    public const int STRENGTH_ENCHANCE = 1;
    public readonly DateTime strengthenEnd;
    public readonly DateTime strengthenStart;
    public readonly DateTime upGradeEnd;
    public readonly DateTime upGradeStart;
    public readonly DateTime useMoneyEnd;
    public readonly DateTime useMoneyStart;

    public PlayerExtra(GamePlayer player, bool saveTodb)
    {
      this.m_player = player;
      this.m_kingBlessInfo = new Dictionary<int, int>();
      this.m_searchGoodItems = new List<EventAwardInfo>();
      this.m_saveToDb = saveTodb;
    }

    public void BeginHotSpringTimer()
    {
      int num = 60000;
      if (this._hotSpringTimer == null)
        this._hotSpringTimer = new Timer(new TimerCallback(this.HotSpringCheck), (object) null, num, num);
      else
        this._hotSpringTimer.Change(num, num);
    }

    public UsersExtraInfo CreateUserExtra(int UserID)
    {
      UsersExtraInfo usersExtraInfo = new UsersExtraInfo();
      usersExtraInfo.UserID = UserID;
      usersExtraInfo.LastTimeHotSpring = DateTime.Now;
      usersExtraInfo.LastFreeTimeHotSpring = DateTime.Now;
      usersExtraInfo.MinHotSpring = 60;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.AddDays(-1.0);
      usersExtraInfo.LeftRoutteCount = GameProperties.LeftRouterMaxDay;
      usersExtraInfo.LeftRoutteRate = 0.0f;
      return usersExtraInfo;
    }
        public bool CheckNoviceActiveOpen(NoviceActiveType activeType)
        {
            bool flag = false;
            switch (activeType)
            {
                case NoviceActiveType.GRADE_UP_ACTIVE:
                    return true;
                case NoviceActiveType.STRENGTHEN_WEAPON_ACTIVE:
                    return true;
                case NoviceActiveType.USE_MONEY_ACTIVE:
                    return true;
                case NoviceActiveType.RECHANGE_MONEY_ACTIVE:
                    return true;
                default:
                    return flag;
            }
        }
        public bool UseKingBless(int key)
        {
            object @lock;
            Monitor.Enter(@lock = this.m_lock);
            try
            {
                if (this.m_kingBlessInfo.ContainsKey(key) && this.m_kingBlessInfo[key] > 0)
                {
                    Dictionary<int, int> kingBlessInfo;
                    (kingBlessInfo = this.m_kingBlessInfo)[key] = kingBlessInfo[key] - 1;
                 //   this.Player.Out.SendKingBlessUpdateBuffData(this.Player.PlayerCharacter.ID, key, this.m_kingBlessInfo[key]);
                    return true;
                }
            }
            finally
            {
                Monitor.Exit(@lock);
            }
            return false;
        }
        public string GetNoviceActivityName(NoviceActiveType activeType)
        {
            string translateId = "Gastos com consumo diários";
            switch (activeType)
            {
                case NoviceActiveType.GRADE_UP_ACTIVE:
                    translateId = "Suba de nível para receber recompensas";
                    break;
                case NoviceActiveType.STRENGTHEN_WEAPON_ACTIVE:
                    translateId = "Melhorar a oferta de presentes";
                    break;              
                case NoviceActiveType.USE_MONEY_ACTIVE:
                    translateId = "Gastos com bônus diários";
                    break;
                case NoviceActiveType.RECHANGE_MONEY_ACTIVE:
                    translateId = "Recarregue todos os dias";
                    break;
                case NoviceActiveType.RECHANGE_MONEY_Accumulated:
                    translateId = "Recarga Acumulada";
                    break;
            }
            return string.Format(translateId);
        }

        public EventRewardProcessInfo GetEventProcess(int activeType)
    {
      lock (this.m_lock)
      {
        if (!this.dictionary_0.ContainsKey(activeType))
          this.dictionary_0.Add(activeType, this.method_0(activeType));
        return this.dictionary_0[activeType];
      }
    }

    public EventAwardInfo GetSpecialTem(int type, int pos) => new EventAwardInfo()
    {
      TemplateID = type,
      Position = pos,
      Count = 1
    };
        public void BeginOnlineGameTimer()
        {
            int dueTime = 60000;
            if (_onlineGameTimer == null)
            {
                _onlineGameTimer = new Timer(OnlineGameCheck, null, dueTime, dueTime);
            }
            else
            {
                _onlineGameTimer.Change(dueTime, dueTime);
            }
        }
     public void ResetNoviceEvent(NoviceActiveType activeType)
    {
      EventRewardProcessInfo eventProcess = this.GetEventProcess((int) activeType);
      eventProcess.AwardGot = 0;
      eventProcess.Conditions = 0;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
        playerBussiness.UpdateUsersEventProcess(eventProcess);
    }

        protected void HotSpringCheck(object sender)
        {
            try
            {
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                if (m_player.CurrentHotSpringRoom == null)
                {
                    StopHotSpringTimer();
                    return;
                }
                if (Info.MinHotSpring <= 0)
                {
                    m_player.CurrentHotSpringRoom.RemovePlayer(m_player);
                    GSPacketIn pkg = new GSPacketIn(169);
                    pkg.WriteString(string.Format("Tempo limite de entrada da fonte termal, por favor, estenda!!!"));
                    m_player.SendTCP(pkg);
                    return;
                }
                int expWithLevel = HotSpringMgr.GetExpWithLevel(m_player.PlayerCharacter.Grade) / 10;
                int ExpHotSpring = 0;
                if (expWithLevel > 0)
                {
                    Info.MinHotSpring--;
                    m_player.OnPlayerSpa(1);
                    if (Info.MinHotSpring <= 5)
                    {
                        m_player.SendMessage("Você só tem " + Info.MinHotSpring + " minuto.");
                    }
                    if (m_player.CurrentHotSpringRoom.Info.roomID <= 4)
                    {
                        ExpHotSpring = expWithLevel * 2;
                        m_player.SendMessage(string.Format("|Sala normal | -EXP obtido é {0}", expWithLevel * 2));
                    }
                    else
                    {
                        /* ExpHotSpring = expWithLevel * 4;
                         m_player.SendMessage(string.Format("| Sala VIP | -O EXP recebido é { 0}", expWithLevel * 4));*/
                        ExpHotSpring = expWithLevel * 2;
                        m_player.SendMessage(string.Format("|Sala normal | -EXP obtido é {0}", expWithLevel * 2));
                    }
                    m_player.AddGP(ExpHotSpring);
                    m_player.Out.SendHotSpringUpdateTime(m_player, ExpHotSpring);
                    m_player.OnHotSpingExpAdd(Info.MinHotSpring, ExpHotSpring);
                }
                Thread.CurrentThread.Priority = priority;
                tickCount = Environment.TickCount - tickCount;
            }
            catch (Exception exception)
            {
                Console.WriteLine("HotSpringCheck: " + exception);
            }
        }
        protected void OnlineGameCheck(object sender)
        {
            try
            {
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                m_player.OnOnlineGameAdd();
                Thread.CurrentThread.Priority = priority;
                tickCount = Environment.TickCount - tickCount;
            }
            catch (Exception exception)
            {
                Console.WriteLine("OnlineGameCheck: " + exception);
            }
        }
        public void StopAllTimer()
        {
            StopHotSpringTimer();
            StopOnlineGameTimer();
        }
        public virtual void LoadFromDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        this.m_Info = playerBussiness.GetSingleUsersExtra(this.m_player.PlayerCharacter.ID);
        if (this.m_Info == null)
          this.m_Info = this.CreateUserExtra(this.Player.PlayerCharacter.ID);
        this.dictionary_0 = new Dictionary<int, EventRewardProcessInfo>();
        foreach (EventRewardProcessInfo rewardProcessInfo in playerBussiness.GetUserEventProcess(this.m_player.PlayerCharacter.ID))
        {
          if (!this.dictionary_0.ContainsKey(rewardProcessInfo.ActiveType))
            this.dictionary_0.Add(rewardProcessInfo.ActiveType, rewardProcessInfo);
        }
      }
    }

        private EventRewardProcessInfo method_0(int int_0) => new EventRewardProcessInfo()
    {
      UserID = this.m_player.PlayerCharacter.ID,
      ActiveType = int_0,
      Conditions = 0,
      AwardGot = 0
    };

    public virtual void SaveToDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          if (this.m_Info == null || !this.m_Info.IsDirty)
            return;
          playerBussiness.UpdateUserExtra(this.m_Info);
        }
      }
    }

    public void StopHotSpringTimer()
    {
      if (this._hotSpringTimer == null)
        return;
      this._hotSpringTimer.Dispose();
      this._hotSpringTimer = (Timer) null;
    }
        public void StopOnlineGameTimer()
        {
            if (_onlineGameTimer != null)
            {
                _onlineGameTimer.Dispose();
                _onlineGameTimer = null;
            }
        }
        public void UpdateEventCondition(int activeType, int value) => this.UpdateEventCondition(activeType, value, false, 0);

    public void UpdateEventCondition(int activeType, int value, bool isPlus, int awardGot)
    {
      PlayerBussiness playerBussiness = new PlayerBussiness();
      EventRewardProcessInfo rewardProcessInfo = this.GetEventProcess(activeType) ?? this.method_0(activeType);
      if (isPlus)
        rewardProcessInfo.Conditions += value;
      else if (rewardProcessInfo.Conditions < value)
        rewardProcessInfo.Conditions = value;
      if (awardGot != 0)
        rewardProcessInfo.AwardGot = awardGot;
      DateTime now = DateTime.Now;
      DateTime endTime = DateTime.Now.AddYears(2);
      EventRewardProcessInfo info = rewardProcessInfo;
      playerBussiness.UpdateUsersEventProcess(info);
      this.m_player.Out.SendOpenNoviceActive(0, activeType, rewardProcessInfo.Conditions, rewardProcessInfo.AwardGot, now, endTime);
    }

    public UsersExtraInfo Info
    {
      get => this.m_Info;
      set => this.m_Info = value;
    }

    public Dictionary<int, int> KingBlessInfo
    {
      get => this.m_kingBlessInfo;
      set => this.m_kingBlessInfo = value;
    }

    public GamePlayer Player => this.m_player;

    public List<EventAwardInfo> SearchGoodItems
    {
      get => this.m_searchGoodItems;
      set => this.m_searchGoodItems = value;
    }
  }
}
