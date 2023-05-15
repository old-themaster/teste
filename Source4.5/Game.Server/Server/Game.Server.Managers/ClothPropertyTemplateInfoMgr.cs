using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Game.Server.Managers
{
	public class ClothPropertyTemplateInfoMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static Dictionary<int, ClothPropertyTemplateInfo> _clothProperty;
		private static ReaderWriterLock m_lock;
		public static bool Init()
		{
			bool result;
			try
			{
				ClothPropertyTemplateInfoMgr.m_lock = new ReaderWriterLock();
				ClothPropertyTemplateInfoMgr._clothProperty = new Dictionary<int, ClothPropertyTemplateInfo>();
				result = ClothPropertyTemplateInfoMgr.LoadClothProperty(ClothPropertyTemplateInfoMgr._clothProperty);
			}
			catch (Exception exception)
			{
				if (ClothPropertyTemplateInfoMgr.log.IsErrorEnabled)
				{
					ClothPropertyTemplateInfoMgr.log.Error("ClothPropertyMgr", exception);
				}
				result = false;
			}
			return result;
		}
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ClothPropertyTemplateInfo> clothProperty = new Dictionary<int, ClothPropertyTemplateInfo>();
				if (ClothPropertyTemplateInfoMgr.LoadClothProperty(clothProperty))
				{
					ClothPropertyTemplateInfoMgr.m_lock.AcquireWriterLock(-1);
					try
					{
						ClothPropertyTemplateInfoMgr._clothProperty = clothProperty;
						return true;
					}
					catch
					{
					}
					finally
					{
						ClothPropertyTemplateInfoMgr.m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				if (ClothPropertyTemplateInfoMgr.log.IsErrorEnabled)
				{
					ClothPropertyTemplateInfoMgr.log.Error("ClothPropertyMgr", exception);
				}
			}
			return false;
		}
		private static bool LoadClothProperty(Dictionary<int, ClothPropertyTemplateInfo> clothProperty)
		{
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				ClothPropertyTemplateInfo[] allClothProperty = playerBussiness.GetAllClothProperty();
				ClothPropertyTemplateInfo[] array = allClothProperty;
				for (int i = 0; i < array.Length; i++)
				{
					ClothPropertyTemplateInfo clothPropertyTemplateInfo = array[i];
					if (!clothProperty.ContainsKey(clothPropertyTemplateInfo.ID))
					{
						clothProperty.Add(clothPropertyTemplateInfo.ID, clothPropertyTemplateInfo);
					}
				}
			}
			return true;
		}
		public static ClothPropertyTemplateInfo GetClothPropertyWithID(int ID)
		{
			ReaderWriterLock @lock;
			Monitor.Enter(@lock = ClothPropertyTemplateInfoMgr.m_lock);
			ClothPropertyTemplateInfo result;
			try
			{
				if (ClothPropertyTemplateInfoMgr._clothProperty.Count > 0)
				{
					foreach (ClothPropertyTemplateInfo current in ClothPropertyTemplateInfoMgr._clothProperty.Values)
					{
						if (current.ID == ID)
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
		public static ClothPropertyTemplateInfo GetClothPropertyWithID(int ID, int Sex)
		{
			ReaderWriterLock @lock;
			Monitor.Enter(@lock = ClothPropertyTemplateInfoMgr.m_lock);
			ClothPropertyTemplateInfo result;
			try
			{
				if (ClothPropertyTemplateInfoMgr._clothProperty.Count > 0)
				{
					foreach (ClothPropertyTemplateInfo current in ClothPropertyTemplateInfoMgr._clothProperty.Values)
					{
						if (current.ID == ID && current.Sex == Sex)
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
