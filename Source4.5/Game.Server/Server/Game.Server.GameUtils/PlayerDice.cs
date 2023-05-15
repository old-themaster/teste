// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PlayerDice
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F406BD-7233-42D4-AF91-73FA12644876
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\dll8.6\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Server.GameUtils
{
    public class PlayerDice
    {
        public int MAX_LEVEL = 5;
        public int refreshPrice = GameProperties.DiceRefreshPrice;
        public int commonDicePrice = GameProperties.CommonDicePrice;
        public int doubleDicePrice = GameProperties.DoubleDicePrice;
        public int bigDicePrice = GameProperties.BigDicePrice;
        public int smallDicePrice = GameProperties.SmallDicePrice;
        public int[] IntegralPoint = new int[5]
        {
      100,
      300,
      700,
      1500,
      3100
        };
        protected GamePlayer m_player;
        protected object m_lock = new object();
        private int m_result;
        private List<EventAwardInfo> m_rewardItem;
        private string m_rewardName;
        private Dictionary<int, List<DiceLevelAwardInfo>> m_LevelAward;
        private DiceDataInfo m_diceData;
        private bool m_saveToDb;

        public GamePlayer Player => this.m_player;

        public int result
        {
            get => this.m_result;
            set => this.m_result = value;
        }

        public List<EventAwardInfo> RewardItem
        {
            get => this.m_rewardItem;
            set => this.m_rewardItem = value;
        }

        public string RewardName
        {
            get => this.m_rewardName;
            set => this.m_rewardName = value;
        }

        public Dictionary<int, List<DiceLevelAwardInfo>> LevelAward
        {
            get => this.m_LevelAward;
            set => this.m_LevelAward = value;
        }

        public DiceDataInfo Data
        {
            get => this.m_diceData;
            set => this.m_diceData = value;
        }

        public PlayerDice(GamePlayer player, bool saveTodb)
        {
            this.m_player = player;
            this.m_saveToDb = saveTodb;
            this.m_result = 0;
            this.m_rewardName = "";
        }

        public void LoadFromDatabase()
        {
            if (!this.IsDiceOpen())
                return;
            if (this.m_diceData == null)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    this.m_diceData = playerBussiness.GetSingleDiceData(this.Player.PlayerCharacter.ID);
                    if (this.m_diceData == null)
                        this.SetupDiceData();
                }
            }
            this.ReceiveLevelAward();
        }

        public bool IsDiceOpen()
        {
            Convert.ToDateTime(GameProperties.DiceBeginTime);
            return DateTime.Now.Date < Convert.ToDateTime(GameProperties.DiceEndTime).Date;
        }

        public void SendDiceActiveOpen()
        {
            if (!this.IsDiceOpen())
                return;
            this.Player.Out.SendDiceActiveOpen(this);
        }

        public void Reset()
        {
            if (!this.IsDiceOpen())
                return;
            lock (this.m_lock)
            {
                this.m_diceData.LuckIntegral = 0;
                this.m_diceData.CurrentPosition = -1;
                this.m_diceData.LuckIntegralLevel = -1;
                this.m_diceData.UserFirstCell = false;
                this.m_diceData.FreeCount = 3;
                this.m_diceData.Level = 0;
            }
        }

        private void SetupDiceData()
        {
            lock (this.m_lock)
            {
                this.m_diceData = new DiceDataInfo();
                this.m_diceData.UserID = this.Player.PlayerCharacter.ID;
                this.m_diceData.LuckIntegral = 0;
                this.m_diceData.CurrentPosition = -1;
                this.m_diceData.LuckIntegralLevel = -1;
                this.m_diceData.UserFirstCell = false;
                this.m_diceData.FreeCount = 3;
                this.m_diceData.Level = 0;
                this.m_diceData.AwardArray = "";
            }
        }

        public void ReceiveData()
        {
            if (string.IsNullOrEmpty(this.m_diceData.AwardArray))
            {
                this.CreateDiceAward();
            }
            else
            {
                this.m_rewardItem = new List<EventAwardInfo>();
                string awardArray = this.m_diceData.AwardArray;
                char[] chArray1 = new char[1] { '|' };
                foreach (string str in awardArray.Split(chArray1))
                {
                    char[] chArray2 = new char[1] { ',' };
                    string[] strArray = str.Split(chArray2);
                    this.m_rewardItem.Add(new EventAwardInfo()
                    {
                        TemplateID = int.Parse(strArray[0]),
                        StrengthenLevel = int.Parse(strArray[1]),
                        Count = int.Parse(strArray[2]),
                        ValidDate = int.Parse(strArray[3]),
                        IsBinds = bool.Parse(strArray[4])
                    });
                }
                if (this.m_rewardItem.Count >= 19)
                    return;
                this.CreateDiceAward();
            }
        }

        public void ReceiveLevelAward()
        {
            lock (this.m_lock)
            {
                this.m_LevelAward = new Dictionary<int, List<DiceLevelAwardInfo>>();
                for (int key = 0; key < this.MAX_LEVEL; ++key)
                {
                    List<DiceLevelAwardInfo> diceLevelAwardAward = DiceLevelAwardMgr.GetAllDiceLevelAwardAward(key + 1);
                    if (!this.m_LevelAward.ContainsKey(key))
                        this.m_LevelAward.Add(key, diceLevelAwardAward);
                    else
                        this.m_LevelAward[key] = diceLevelAwardAward;
                }
            }
        }

        public void GetLevelAward()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (DiceLevelAwardInfo diceLevelAwardInfo in (IEnumerable<DiceLevelAwardInfo>)this.m_LevelAward[this.m_diceData.LuckIntegralLevel])
            {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(diceLevelAwardInfo.TemplateID);
                if (itemTemplate != null)
                {
                    SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, diceLevelAwardInfo.Count, 103);
                    this.Player.AddTemplate(fromTemplate);
                    stringBuilder.Append(fromTemplate.Template.Name + "x" + (object)fromTemplate.Count + "; ");
                }
            }
            if (this.m_diceData.LuckIntegralLevel <= 3)
                return;
            this.Player.SendMessage("Bạn nhận được " + stringBuilder.ToString());
        }

        public void CreateDiceAward()
        {
            lock (this.m_lock)
            {
                this.m_rewardItem = new List<EventAwardInfo>();
                Dictionary<int, EventAwardInfo> dictionary = new Dictionary<int, EventAwardInfo>();
                int num = 0;
                while (this.m_rewardItem.Count < 19)
                {
                    List<EventAwardInfo> diceAward = EventAwardMgr.GetDiceAward(eEventType.DICE);
                    if (diceAward.Count > 0)
                    {
                        EventAwardInfo eventAwardInfo = diceAward[0];
                        if (!dictionary.Keys.Contains<int>(eventAwardInfo.TemplateID))
                        {
                            dictionary.Add(eventAwardInfo.TemplateID, eventAwardInfo);
                            this.m_rewardItem.Add(eventAwardInfo);
                        }
                    }
                    ++num;
                }
            }
            this.ConvertAwardArray();
        }

        public void ConvertAwardArray()
        {
            if (this.m_rewardItem.Count <= 0)
                return;
            string str = "";
            foreach (EventAwardInfo eventAwardInfo in this.m_rewardItem)
                str += string.Format("{0},{1},{2},{3},{4}|", (object)eventAwardInfo.TemplateID, (object)eventAwardInfo.StrengthenLevel, (object)eventAwardInfo.Count, (object)eventAwardInfo.ValidDate, (object)eventAwardInfo.IsBinds.ToString());
            this.m_diceData.AwardArray = str.Substring(0, str.Length - 1);
        }

        public virtual void SaveToDatabase()
        {
            if (!this.m_saveToDb)
                return;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                lock (this.m_lock)
                {
                    if (this.m_diceData != null && this.m_diceData.IsDirty)
                    {
                        if (this.m_diceData.ID > 0)
                            playerBussiness.UpdateDiceData(this.m_diceData);
                        else
                            playerBussiness.AddDiceData(this.m_diceData);
                    }
                }
            }
        }
    }
}
