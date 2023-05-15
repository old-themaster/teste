using Bussiness;
using Bussiness.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Game.Server.GameUtils
{
    public class PlayerTreasure
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected object m_lock = new object();
		protected GamePlayer m_player;
		private List<TreasureDataInfo> m_TreasureData;
		private List<TreasureDataInfo> m_TreasureDig;
		private UserTreasureInfo m_Treasure;
		private bool m_saveToDb;
		public GamePlayer Player
		{
			get
			{
				return this.m_player;
			}
		}
		public List<TreasureDataInfo> TreasureData
		{
			get
			{
				return this.m_TreasureData;
			}
			set
			{
				this.m_TreasureData = value;
			}
		}
		public List<TreasureDataInfo> TreasureDig
		{
			get
			{
				return this.m_TreasureDig;
			}
			set
			{
				this.m_TreasureDig = value;
			}
		}
		public UserTreasureInfo CurrentTreasure
		{
			get
			{
				return this.m_Treasure;
			}
			set
			{
				this.m_Treasure = value;
			}
		}
		public PlayerTreasure(GamePlayer player, bool saveTodb)
		{
			this.m_player = player;
			this.m_saveToDb = saveTodb;
			this.m_TreasureData = new List<TreasureDataInfo>();
			this.m_TreasureDig = new List<TreasureDataInfo>();
			this.m_Treasure = new UserTreasureInfo();
		}
		public virtual void LoadFromDatabase()
		{
			if (this.m_saveToDb)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					this.m_Treasure = playerBussiness.GetSingleTreasure(this.Player.PlayerCharacter.ID);
					List<TreasureDataInfo> singleTreasureData = playerBussiness.GetSingleTreasureData(this.Player.PlayerCharacter.ID);
					if (this.m_Treasure == null)
					{
						this.CreateTreasure();
					}
					foreach (TreasureDataInfo current in singleTreasureData)
					{
						this.m_TreasureData.Add(current);
						if (current.pos > 0)
						{
							this.m_TreasureDig.Add(current);
						}
					}
				}
			}
		}
		public void CreateTreasure()
		{
			if (this.m_Treasure == null)
			{
				this.m_Treasure = new UserTreasureInfo();
			}
			UserTreasureInfo treasure;
			Monitor.Enter(treasure = this.m_Treasure);
			try
			{
				this.m_Treasure.ID = 0;
				this.m_Treasure.UserID = this.Player.PlayerCharacter.ID;
				this.m_Treasure.NickName = this.Player.PlayerCharacter.NickName;
				this.m_Treasure.treasure = 1;
				this.m_Treasure.treasureAdd = 0;
				this.m_Treasure.logoinDays = 1;
				this.m_Treasure.friendHelpTimes = 0;
				this.m_Treasure.isBeginTreasure = false;
				this.m_Treasure.isEndTreasure = false;
				this.m_Treasure.LastLoginDay = DateTime.Now;
			}
			finally
			{
				Monitor.Exit(treasure);
			}
		}
		public void AddfriendHelpTimes()
		{
			UserTreasureInfo treasure;
			Monitor.Enter(treasure = this.m_Treasure);
			try
			{
				if (this.m_Treasure.friendHelpTimes < 5)
				{
					this.m_Treasure.friendHelpTimes++;
					if (this.m_Treasure.friendHelpTimes == 5 && this.m_Treasure.treasureAdd == 0)
					{
						this.m_Treasure.treasureAdd++;
					}
				}
			}
			finally
			{
				Monitor.Exit(treasure);
			}
		}
		public void UpdateUserTreasure(UserTreasureInfo info)
		{
			UserTreasureInfo treasure;
			Monitor.Enter(treasure = this.m_Treasure);
			try
			{
				this.m_Treasure = info;
			}
			finally
			{
				Monitor.Exit(treasure);
			}
		}
		public void Clear()
		{
			List<TreasureDataInfo> treasureDig;
			Monitor.Enter(treasureDig = this.m_TreasureDig);
			try
			{
				this.m_TreasureDig = new List<TreasureDataInfo>();
			}
			finally
			{
				Monitor.Exit(treasureDig);
			}
		}
		public void UpdateLoginDay()
		{
			int iD = this.Player.PlayerCharacter.ID;
			List<TreasureDataInfo> list = this.m_TreasureData;
			if (list.Count == 0)
			{
				list = TreasureAwardMgr.CreateTreasureData(iD);
				this.AddTreasureData(list);
			}
			else
			{
				if (this.m_Treasure.isValidDate())
				{
					list = TreasureAwardMgr.CreateTreasureData(iD);
					this.UpdateTreasureData(list);
					this.Clear();
				}
			}
			UserTreasureInfo treasure;
			Monitor.Enter(treasure = this.m_Treasure);
			try
			{
				if (this.m_Treasure.isValidDate())
				{
					if ((int)DateTime.Now.Subtract(this.m_Treasure.LastLoginDay).TotalDays > 1)
					{
						this.m_Treasure.logoinDays = 0;
					}
					this.m_Treasure.logoinDays++;
					if (this.m_Treasure.logoinDays > 3)
					{
						this.m_Treasure.treasure = 3;
					}
					else
					{
						this.m_Treasure.treasure = this.m_Treasure.logoinDays;
					}
					this.m_Treasure.treasureAdd = 0;
					this.m_Treasure.friendHelpTimes = 0;
					this.m_Treasure.isBeginTreasure = false;
					this.m_Treasure.isEndTreasure = false;
					this.m_Treasure.LastLoginDay = DateTime.Now;
				}
			}
			finally
			{
				Monitor.Exit(treasure);
			}
		}
		public void AddTreasureData(List<TreasureDataInfo> datas)
		{
			List<TreasureDataInfo> treasureData;
			Monitor.Enter(treasureData = this.m_TreasureData);
			try
			{
				foreach (TreasureDataInfo current in datas)
				{
					this.m_TreasureData.Add(current);
				}
			}
			finally
			{
				Monitor.Exit(treasureData);
			}
		}
		public void UpdateTreasureData(List<TreasureDataInfo> datas)
		{
			for (int i = 0; i < this.m_TreasureData.Count; i++)
			{
				datas[i].ID = this.m_TreasureData[i].ID;
			}
			List<TreasureDataInfo> treasureData;
			Monitor.Enter(treasureData = this.m_TreasureData);
			try
			{
				this.m_TreasureData = datas;
			}
			finally
			{
				Monitor.Exit(treasureData);
			}
		}
		public void AddTreasureDig(TreasureDataInfo info, int index)
		{
			List<TreasureDataInfo> treasureDig;
			Monitor.Enter(treasureDig = this.m_TreasureDig);
			try
			{
				this.m_TreasureDig.Add(info);
			}
			finally
			{
				Monitor.Exit(treasureDig);
			}
			List<TreasureDataInfo> treasureData;
			Monitor.Enter(treasureData = this.m_TreasureData);
			try
			{
				this.m_TreasureData[index].pos = info.pos;
			}
			finally
			{
				Monitor.Exit(treasureData);
			}
		}
		public virtual void SaveToDatabase()
		{
			if (this.m_saveToDb)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					object @lock;
					Monitor.Enter(@lock = this.m_lock);
					try
					{
						if (this.m_Treasure != null && this.m_Treasure.IsDirty)
						{
							if (this.m_Treasure.ID > 0)
							{
							//	playerBussiness.UpdateUserTreasureInfo(this.m_Treasure);
							}
							else
							{
								playerBussiness.AddUserTreasureInfo(this.m_Treasure);
							}
						}
						for (int i = 0; i < this.m_TreasureData.Count; i++)
						{
							TreasureDataInfo treasureDataInfo = this.m_TreasureData[i];
							if (treasureDataInfo != null && treasureDataInfo.IsDirty)
							{
								if (treasureDataInfo.ID > 0)
								{
									playerBussiness.UpdateTreasureData(treasureDataInfo);
								}
								else
								{
									playerBussiness.AddTreasureData(treasureDataInfo);
								}
							}
						}
					}
					finally
					{
						Monitor.Exit(@lock);
					}
				}
			}
		}
	}
}
