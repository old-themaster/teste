using System;
using System.Collections.Generic;
using System.Reflection;
using Bussiness;
using Bussiness.Managers;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils

{

	public class PlayerRank
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected object m_lock = new object();

		private UserRankInfo m_currentRank;

		protected GamePlayer m_player;

		private List<UserRankInfo> m_rank;

		private List<UserRankInfo> m_removeRank;

		private bool m_saveToDb;

		public UserRankInfo CurrentRank
		{
			get
			{
				return m_currentRank;
			}
			set
			{
				m_currentRank = value;
			}
		}

		public GamePlayer Player => m_player;

		public List<UserRankInfo> Ranks
		{
			get
			{
				return m_rank;
			}
			set
			{
				m_rank = value;
			}
		}

		public PlayerRank(GamePlayer player, bool saveTodb)
		{
			m_player = player;
			m_saveToDb = saveTodb;
			m_rank = new List<UserRankInfo>();
			m_removeRank = new List<UserRankInfo>();
			m_currentRank = GetRank(m_player.PlayerCharacter.Honor);
		}

		public void AddRank(UserRankInfo info)
		{
			lock (m_rank)
			{
				m_rank.Add(info);
			}
		}

		public UserRankInfo GetRankByHonnor(int honor)
		{
			foreach (UserRankInfo item in m_rank)
			{
				if (item.NewTitleID == honor)
				{
					return item;
				}
			}
			return null;
		}

		public bool UpdateCurrentRank()
		{
			lock (m_lock)
			{
				m_currentRank = GetSingleRank();
			}
			return CurrentRank != null;
		}

		public UserRankInfo GetSingleRank()
		{
			UserRankInfo rankByHonnor = GetRankByHonnor(m_player.PlayerCharacter.honorId);
			if (rankByHonnor == null || rankByHonnor.IsValidRank())
			{
				return rankByHonnor;
			}
			m_player.PlayerCharacter.honorId = 0;
			m_player.PlayerCharacter.Honor = "";
			RemoveRank(rankByHonnor);
			return null;
		}

		public UserRankInfo GetSingleRank(string honor)
		{
			foreach (UserRankInfo item in m_rank)
			{
				if (item.Name.Contains(honor) && item.IsValidRank())
				{
					return item;
				}
			}
			return null;
		}

		public void AddNewRank(int id, int days)
		{
			NewTitleInfo newTitleInfo = NewTitleMgr.FindNewTitle(id);
			if (newTitleInfo != null)
			{
				UserRankInfo rankByHonnor = GetRankByHonnor(id);
				if (rankByHonnor == null)
				{
					UserRankInfo userRankInfo = new UserRankInfo();
					userRankInfo.UserID = Player.PlayerCharacter.ID;
					userRankInfo.Name = newTitleInfo.Name;
					userRankInfo.NewTitleID = newTitleInfo.ID;
					userRankInfo.Attack = newTitleInfo.Att;
					userRankInfo.Defence = newTitleInfo.Def;
					userRankInfo.Agility = newTitleInfo.Agi;
					userRankInfo.Luck = newTitleInfo.Luck;
					userRankInfo.BeginDate = DateTime.Now;
					userRankInfo.EndDate = DateTime.Now.AddDays(days);
					userRankInfo.Validate = days;
					userRankInfo.IsExit = true;
					AddRank(userRankInfo);
				}
				else
				{
					rankByHonnor.IsExit = true;
					rankByHonnor.EndDate = DateTime.Now.AddDays(days);
				}
				SendUserRanks();
				SaveToDatabase();
			}
		}

		public void SendUserRanks()
		{
			Player.Out.SendUserRanks(Player.PlayerCharacter.ID, m_rank);
		}

		public void AddRank(string honor, int HonorId)
		{
			AddRank(new UserRankInfo
			{
				ID = 0,
				UserID = m_player.PlayerCharacter.ID,
				NewTitleID = HonorId,
				Name = honor,
				Attack = 0,
				Defence = 0,
				Luck = 0,
				Agility = 0,
				HP = 0,
				Damage = 0,
				Guard = 0,
				BeginDate = DateTime.Now,
				Validate = 0,
				IsExit = true
			});
		}

		/*	public void CreateRank(int UserID)
			{
				List<UserRankInfo> list = new List<UserRankInfo>();
				NewTitleInfo newTitleInfo = NewTitleMgr.FindNewTitle(127);
				if (newTitleInfo != null)
				{
					AddRank(new UserRankInfo
					{
						Info = newTitleInfo,
						ID = 0,
						UserID = UserID,
						//NewTitleID = 2002,
						NewTitleID = newTitleInfo.ID,
						Name = newTitleInfo.Name,
						BeginDate = DateTime.Now,
						EndDate = DateTime.Now,
						Validate = 0,
						IsExit = true
					});
				}
			}*/

		public void CreateRank(int UserID)
		{
			List<UserRankInfo> userRankInfoList = new List<UserRankInfo>();
			this.AddRank(new UserRankInfo()
			{
				ID = 0,
				UserID = UserID,
				NewTitleID = 618,
				Name = "Cigarra",
				Attack = 5,
				Defence = 5,
				Luck = 5,
				Agility = 5,
				HP = 0,
				Damage = 0,
				Guard = 0,
				BeginDate = DateTime.Now,
				EndDate = DateTime.Now.AddYears(50),
				Validate = 0,
				IsExit = true
			});
		}


		public List<UserRankInfo> GetRank()
		{
			List<UserRankInfo> list = new List<UserRankInfo>();
			foreach (UserRankInfo item in m_rank)
			{
				if (item.IsExit)
				{
					list.Add(item);
				}
			}
			return list;
		}

		public UserRankInfo GetRank(string honor)
		{
			foreach (UserRankInfo item in m_rank)
			{
				if (item.Name == honor)
				{
					return item;
				}
			}
			return null;
		}

		public bool IsRank(string honor)
		{
			foreach (UserRankInfo item in m_rank)
			{
				if (item.Name == honor)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void LoadFromDatabase()
		{
			if (!this.m_saveToDb)
				return;
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				List<UserRankInfo> singleUserRank = playerBussiness.GetSingleUserRank(this.Player.PlayerCharacter.ID);
				if (singleUserRank.Count == 0)
				{
					this.CreateRank(this.Player.PlayerCharacter.ID);
				}
				else
				{
					foreach (UserRankInfo info in singleUserRank)
					{
						if (info.IsValidRank())
							this.AddRank(info);
						else
							this.RemoveRank(info);
					}
				}
			}
		}

		public void RemoveRank(UserRankInfo item)
		{
			bool flag = false;
			lock (m_rank)
			{
				flag = m_rank.Remove(item);
			}
			if (!flag)
			{
				return;
			}
			item.IsExit = false;
			lock (m_removeRank)
			{
				m_removeRank.Add(item);
			}
		}

		public virtual void SaveToDatabase()
		{
			if (!m_saveToDb)
			{
				return;
			}
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
				lock (m_lock)
				{
					foreach (UserRankInfo item in m_rank)
					{
						if (item != null && item.IsDirty)
						{
							if (item.ID > 0)
							{
								playerBussiness.UpdateUserRank(item);
							}
							else
							{
								playerBussiness.AddUserRank(item);
							}
						}
					}
					foreach (UserRankInfo item2 in m_removeRank)
					{
						playerBussiness.UpdateUserRank(item2);
					}
					m_removeRank.Clear();
				}
		}
	}
}
