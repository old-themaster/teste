using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
namespace Bussiness.Managers
{
	public class TreasureAwardMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static Dictionary<int, TreasureAwardInfo> _treasureAward;
		private static ReaderWriterLock m_lock;
		private static ThreadSafeRandom rand;
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, TreasureAwardInfo> tempTreasureAward = new Dictionary<int, TreasureAwardInfo>();
				if (TreasureAwardMgr.Load(tempTreasureAward))
				{
					TreasureAwardMgr.m_lock.AcquireWriterLock(-1);
					try
					{
						TreasureAwardMgr._treasureAward = tempTreasureAward;
						return true;
					}
					catch
					{
					}
					finally
					{
						TreasureAwardMgr.m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception e)
			{
				if (TreasureAwardMgr.log.IsErrorEnabled)
				{
					TreasureAwardMgr.log.Error("TreasureAwardMgr", e);
				}
			}
			return false;
		}
		public static bool Init()
		{
			bool result;
			try
			{
				TreasureAwardMgr.m_lock = new ReaderWriterLock();
				TreasureAwardMgr._treasureAward = new Dictionary<int, TreasureAwardInfo>();
				TreasureAwardMgr.rand = new ThreadSafeRandom();
				result = TreasureAwardMgr.Load(TreasureAwardMgr._treasureAward);
			}
			catch (Exception e)
			{
				if (TreasureAwardMgr.log.IsErrorEnabled)
				{
					TreasureAwardMgr.log.Error("TreasureAwardMgr", e);
				}
				result = false;
			}
			return result;
		}
		private static bool Load(Dictionary<int, TreasureAwardInfo> treasureAward)
		{
			using (PlayerBussiness db = new PlayerBussiness())
			{
				TreasureAwardInfo[] infos = db.GetAllTreasureAward();
				TreasureAwardInfo[] array = infos;
				for (int i = 0; i < array.Length; i++)
				{
					TreasureAwardInfo info = array[i];
					if (!treasureAward.ContainsKey(info.ID))
					{
						treasureAward.Add(info.ID, info);
					}
				}
			}
			return true;
		}
		public static TreasureAwardInfo FindTreasureAwardInfo(int ID)
		{
			TreasureAwardMgr.m_lock.AcquireReaderLock(10000);
			try
			{
				if (TreasureAwardMgr._treasureAward.ContainsKey(ID))
				{
					return TreasureAwardMgr._treasureAward[ID];
				}
			}
			catch
			{
			}
			finally
			{
				TreasureAwardMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}
		public static List<TreasureAwardInfo> GetTreasureInfos()
		{
			if (TreasureAwardMgr._treasureAward == null)
			{
				TreasureAwardMgr.Init();
			}
			List<TreasureAwardInfo> list = new List<TreasureAwardInfo>();
			TreasureAwardMgr.m_lock.AcquireReaderLock(10000);
			try
			{
				foreach (TreasureAwardInfo treas in TreasureAwardMgr._treasureAward.Values)
				{
					list.Add(treas);
				}
			}
			finally
			{
				TreasureAwardMgr.m_lock.ReleaseReaderLock();
			}
			return list;
		}
		public static List<TreasureDataInfo> CreateTreasureData(int UserID)
		{
			List<TreasureDataInfo> infos = new List<TreasureDataInfo>();
			Dictionary<int, TreasureDataInfo> temp = new Dictionary<int, TreasureDataInfo>();
			int i = 0;
			while (infos.Count < 16)
			{
				List<TreasureDataInfo> list = TreasureAwardMgr.GetTreasureData();
				int index = TreasureAwardMgr.rand.Next(list.Count);
				TreasureDataInfo item = list[index];
				item.UserID = UserID;
				if (!temp.Keys.Contains(item.TemplateID))
				{
					temp.Add(item.TemplateID, item);
					infos.Add(item);
				}
				i++;
			}
			return infos;
		}
		public static List<TreasureDataInfo> GetTreasureData()
		{
			List<TreasureDataInfo> infos = new List<TreasureDataInfo>();
			List<TreasureAwardInfo> FiltInfos = new List<TreasureAwardInfo>();
			List<TreasureAwardInfo> unFiltInfos = TreasureAwardMgr.GetTreasureInfos();
			int dropItemCount = 1;
			int maxRound = ThreadSafeRandom.NextStatic((
				from s in unFiltInfos
				select s.Random).Max());
			List<TreasureAwardInfo> RoundInfos = (
				from s in unFiltInfos
				where s.Random >= maxRound
				select s).ToList<TreasureAwardInfo>();
			int maxItems = RoundInfos.Count<TreasureAwardInfo>();
			if (maxItems > 0)
			{
				dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
				int[] randomArray = TreasureAwardMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
				int[] array = randomArray;
				for (int j = 0; j < array.Length; j++)
				{
					int i = array[j];
					TreasureAwardInfo item = RoundInfos[i];
					FiltInfos.Add(item);
				}
			}
			foreach (TreasureAwardInfo info in FiltInfos)
			{
				infos.Add(new TreasureDataInfo
				{
					ID = 0,
					UserID = 0,
					TemplateID = info.TemplateID,
					Count = info.Count,
					ValidDate = info.Validate,
					pos = -1,
					BeginDate = DateTime.Now,
					IsExit = true
				});
			}
			return infos;
		}
		public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
		{
			int[] resultRound = new int[count];
			for (int i = 0; i < count; i++)
			{
				int j = ThreadSafeRandom.NextStatic(minValue, maxValue + 1);
				int num = 0;
				for (int k = 0; k < i; k++)
				{
					if (resultRound[k] == j)
					{
						num++;
					}
				}
				if (num == 0)
				{
					resultRound[i] = j;
				}
				else
				{
					i--;
				}
			}
			return resultRound;
		}
	}
}
