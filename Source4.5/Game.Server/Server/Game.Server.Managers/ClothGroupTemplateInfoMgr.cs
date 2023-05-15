using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Game.Server.Managers
{
	public class ClothGroupTemplateInfoMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static Dictionary<int, ClothGroupTemplateInfo> _clothGroup;
		private static ReaderWriterLock m_lock;
		public static bool Init()
		{
			bool result;
			try
			{
				ClothGroupTemplateInfoMgr.m_lock = new ReaderWriterLock();
				ClothGroupTemplateInfoMgr._clothGroup = new Dictionary<int, ClothGroupTemplateInfo>();
				result = ClothGroupTemplateInfoMgr.LoadClothGroup(ClothGroupTemplateInfoMgr._clothGroup);
			}
			catch (Exception exception)
			{
				if (ClothGroupTemplateInfoMgr.log.IsErrorEnabled)
				{
					ClothGroupTemplateInfoMgr.log.Error("ClothGroupMgr", exception);
				}
				result = false;
			}
			return result;
		}
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ClothGroupTemplateInfo> clothGroup = new Dictionary<int, ClothGroupTemplateInfo>();
				if (ClothGroupTemplateInfoMgr.LoadClothGroup(clothGroup))
				{
					ClothGroupTemplateInfoMgr.m_lock.AcquireWriterLock(-1);
					try
					{
						ClothGroupTemplateInfoMgr._clothGroup = clothGroup;
						return true;
					}
					catch
					{
					}
					finally
					{
						ClothGroupTemplateInfoMgr.m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				if (ClothGroupTemplateInfoMgr.log.IsErrorEnabled)
				{
					ClothGroupTemplateInfoMgr.log.Error("ClothGroupMgr", exception);
				}
			}
			return false;
		}
		private static bool LoadClothGroup(Dictionary<int, ClothGroupTemplateInfo> clothGroup)
		{
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				ClothGroupTemplateInfo[] allClothGroup = playerBussiness.GetAllClothGroup();
				ClothGroupTemplateInfo[] array = allClothGroup;
				for (int i = 0; i < array.Length; i++)
				{
					ClothGroupTemplateInfo clothGroupTemplateInfo = array[i];
					if (!clothGroup.ContainsKey(clothGroupTemplateInfo.ItemID))
					{
						clothGroup.Add(clothGroupTemplateInfo.ItemID, clothGroupTemplateInfo);
					}
				}
			}
			return true;
		}
		public static List<ClothGroupTemplateInfo> GetClothGroupWithID(int ID)
		{
			ReaderWriterLock @lock;
			Monitor.Enter(@lock = ClothGroupTemplateInfoMgr.m_lock);
			List<ClothGroupTemplateInfo> result;
			try
			{
				List<ClothGroupTemplateInfo> list = new List<ClothGroupTemplateInfo>();
				if (ClothGroupTemplateInfoMgr._clothGroup.Count > 0)
				{
					foreach (ClothGroupTemplateInfo current in ClothGroupTemplateInfoMgr._clothGroup.Values)
					{
						if (current.ID == ID)
						{
							list.Add(current);
						}
					}
				}
				result = list;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return result;
		}
		public static int CountClothGroupWithID(int ID)
		{
			ReaderWriterLock @lock;
			Monitor.Enter(@lock = ClothGroupTemplateInfoMgr.m_lock);
			int count;
			try
			{
				List<ClothGroupTemplateInfo> clothGroupWithID = ClothGroupTemplateInfoMgr.GetClothGroupWithID(ID);
				count = clothGroupWithID.Count;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return count;
		}
		public static ClothGroupTemplateInfo GetClothGroup(int ID, int TemplateID, int Sex)
		{
			ReaderWriterLock @lock;
			Monitor.Enter(@lock = m_lock);
			ClothGroupTemplateInfo result;
			try
			{
				if (ClothGroupTemplateInfoMgr._clothGroup.Count > 0)
				{
					foreach (ClothGroupTemplateInfo current in ClothGroupTemplateInfoMgr._clothGroup.Values)
					{
						if (current.TemplateID == TemplateID && current.ID == ID && current.Sex == Sex)
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
				Monitor.Exit(@lock);
			}
			return result;
		}
	}
}
