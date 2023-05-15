using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Game.Server.Managers;
namespace Game.Server.GameUtils
{
    public class PlayerAvatarCollection
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected object m_lock = new object();
		protected GamePlayer m_player;
		private List<UserAvatarCollectionInfo> m_avtcollect;
		private bool m_saveToDb;
		public GamePlayer Player
		{
			get
			{
				return this.m_player;
			}
		}
		public List<UserAvatarCollectionInfo> AvatarCollect
		{
			get
			{
				return this.m_avtcollect;
			}
			set
			{
				this.m_avtcollect = value;
			}
		}
		public PlayerAvatarCollection(GamePlayer player, bool saveTodb)
		{
			this.m_player = player;
			this.m_saveToDb = saveTodb;
			this.m_avtcollect = new List<UserAvatarCollectionInfo>();
		}
		public virtual void LoadFromDatabase()
		{
			if (this.m_saveToDb)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					List<UserAvatarCollectionInfo> singleAvatarCollect = playerBussiness.GetSingleAvatarCollect(this.Player.PlayerCharacter.ID);
					if (singleAvatarCollect.Count > 0)
					{
						foreach (UserAvatarCollectionInfo current in singleAvatarCollect)
						{
							this.AddAvatarCollect(current);
						}
					}
				}
			}
		}
		public void AddAvatarCollect(UserAvatarCollectionInfo info)
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			try
			{
				this.m_avtcollect.Add(info);
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
		}
		public virtual UserAvatarCollectionInfo GetAvatarCollectWithAvatarID(int avatarId)
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			UserAvatarCollectionInfo result;
			try
			{
				if (this.m_avtcollect.Count > 0)
				{
					foreach (UserAvatarCollectionInfo current in this.m_avtcollect)
					{
						if (current.AvatarID == avatarId && current.IsExit)
						{
							result = current;
							return result;
						}
					}
				}
				result = null;
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
			return result;
		}
		public virtual List<UserAvatarCollectionInfo> GetAvatarCollectActived()
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			List<UserAvatarCollectionInfo> result;
			try
			{
				List<UserAvatarCollectionInfo> list = new List<UserAvatarCollectionInfo>();
				if (this.m_avtcollect.Count > 0)
				{
					foreach (UserAvatarCollectionInfo current in this.m_avtcollect)
					{
						if (current.IsActive = current.IsExit)
						{
							list.Add(current);
						}
					}
				}
				result = list;
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
			return result;
		}
		public virtual List<UserAvatarCollectionInfo> GetAvatarPropertyActived()
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			List<UserAvatarCollectionInfo> result;
			try
			{
				List<UserAvatarCollectionInfo> list = new List<UserAvatarCollectionInfo>();
				List<UserAvatarCollectionInfo> avatarCollectActived = this.m_player.AvatarCollect.GetAvatarCollectActived();
				if (avatarCollectActived.Count > 0)
				{
					foreach (UserAvatarCollectionInfo current in avatarCollectActived)
					{
						ClothPropertyTemplateInfo clothPropertyWithID = ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(current.AvatarID);
						if (clothPropertyWithID != null)
						{
							current.ClothProperty = clothPropertyWithID;
							list.Add(current);
						}
					}
				}
				result = list;
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
			return result;
		}
		public virtual UserAvatarCollectionInfo GetAvatarCollectWithAvatarID(int avatarId, int Sex)
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			UserAvatarCollectionInfo result;
			try
			{
				if (this.m_avtcollect.Count > 0)
				{
					foreach (UserAvatarCollectionInfo current in this.m_avtcollect)
					{
						if (current.AvatarID == avatarId && current.Sex == Sex && current.IsExit)
						{
							result = current;
							return result;
						}
					}
				}
				result = null;
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
			return result;
		}
		public virtual void AddAvatarCollection(UserAvatarCollectionInfo avt)
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			try
			{
				if (this.GetAvatarCollectWithAvatarID(avt.AvatarID) == null)
				{
					this.m_avtcollect.Add(avt);
				}
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
		}
		public virtual bool RemoveAvatarCollect(UserAvatarCollectionInfo avt)
		{
			List<UserAvatarCollectionInfo> avtcollect;
			Monitor.Enter(avtcollect = this.m_avtcollect);
			bool result;
			try
			{
				foreach (UserAvatarCollectionInfo current in this.m_avtcollect)
				{
					if (current == avt)
					{
						current.IsExit = false;
						result = true;
						return result;
					}
				}
				result = false;
			}
			finally
			{
				Monitor.Exit(avtcollect);
			}
			return result;
		}
		public virtual void ScanAvatarVaildDate()
		{
			//List<UserAvatarCollectionInfo> avtcollect;
			//Monitor.Enter(avtcollect = this.m_avtcollect);
			//try
			//{
			//    if (this.m_avtcollect.Count > 0)
			//    {
			//        int num = 0;
			//        foreach (UserAvatarCollectionInfo current in this.m_avtcollect)
			//        {
			//            if (current.IsActive && current.TimeEnd <= DateTime.Now && current.Items != null)
			//            {
			//                current.IsActive = false;
			//                num++;
			//            }
			//        }
			//        if (num > 0)
			//        {
			//            this.SaveToDatabase();
			//            this.Player.SendMessage("Hiện bạn có " + num + " bộ sưu tập đã hết hạn. Hãy gia hạn ngay nhé!");
			//        }
			//    }
			//}
			//finally
			//{
			//    Monitor.Exit(avtcollect);
			//}
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
						for (int i = 0; i < this.m_avtcollect.Count; i++)
						{
							UserAvatarCollectionInfo userAvatarCollectionInfo = this.m_avtcollect[i];
							if (userAvatarCollectionInfo != null && userAvatarCollectionInfo.IsDirty)
							{
								if (userAvatarCollectionInfo.ID > 0)
								{
									playerBussiness.UpdateUserAvatarCollect(userAvatarCollectionInfo);
								}
								else
								{
									if (userAvatarCollectionInfo.ID <= 0)
									{
										playerBussiness.AddUserAvatarCollect(userAvatarCollectionInfo);
									}
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
