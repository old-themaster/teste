using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Bussiness.Managers
{
	public class ActiveMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public static Dictionary<int, ActiveAwardInfo> m_ActiveAwardInfo = new Dictionary<int, ActiveAwardInfo>();
		public static Dictionary<int, List<ActiveConditionInfo>> m_ActiveConditionInfo = new Dictionary<int, List<ActiveConditionInfo>>();
		public static List<ActiveAwardInfo> GetAwardInfo(DateTime lastDate, int playerGrade)
		{
			string awardId = null;
			int days = (DateTime.Now - lastDate).Days;
			if (DateTime.Now.DayOfYear > lastDate.DayOfYear)
			{
				days++;
			}
			List<ActiveAwardInfo> list = new List<ActiveAwardInfo>();
			foreach (List<ActiveConditionInfo> list2 in ActiveMgr.m_ActiveConditionInfo.Values)
			{
				foreach (ActiveConditionInfo info in list2)
				{
					if (ActiveMgr.IsValid(info) && ActiveMgr.IsInGrade(info.LimitGrade, playerGrade) && info.Condition <= days)
					{
						awardId = info.AwardId;
						int arg_98_0 = info.ActiveID;
					}
				}
			}
			if (!string.IsNullOrEmpty(awardId))
			{
				string[] strArray = awardId.Split(new char[]
				{
					','
				});
				string[] array = strArray;
				for (int i = 0; i < array.Length; i++)
				{
					string str2 = array[i];
					if (!string.IsNullOrEmpty(str2) && ActiveMgr.m_ActiveAwardInfo.ContainsKey(Convert.ToInt32(str2)))
					{
						list.Add(ActiveMgr.m_ActiveAwardInfo[Convert.ToInt32(str2)]);
					}
				}
			}
			return list;
		}
		private static Dictionary<int, List<ActiveConvertItemInfo>> dictionary_1;
		public static ActiveConvertItemInfo GetActiveConvertItem(int id, int templateID, int index)
		{
			if (ActiveMgr.dictionary_1.ContainsKey(id))
			{
				List<ActiveConvertItemInfo> list = ActiveMgr.dictionary_1[id];
				ActiveConvertItemInfo result;
				using (List<ActiveConvertItemInfo>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActiveConvertItemInfo current = enumerator.Current;
						if (current.TemplateID == templateID && current.ItemType == index)
						{
							result = current;
							return result;
						}
					}
					goto IL_5E;
				}
				return result;
			}
		IL_5E:
			return null;
		}
		public static int GetGoodsAward(int index)
		{
			switch (index)
			{
				case 1:
					return 3;
				case 2:
					return 5;
				case 3:
					return 7;
				default:
					return 1;
			}
		}
		public static List<ActiveConvertItemInfo> GetActiveConvertItemAward(int id, int index)
		{
			List<ActiveConvertItemInfo> list = new List<ActiveConvertItemInfo>();
			if (ActiveMgr.dictionary_1.ContainsKey(id))
			{
				List<ActiveConvertItemInfo> list2 = ActiveMgr.dictionary_1[id];
				foreach (ActiveConvertItemInfo current in list2)
				{
					if (current.ItemType == ActiveMgr.GetGoodsAward(index))
					{
						list.Add(current);
					}
				}
			}
			return list;
		}
		private static Dictionary<int, ActiveInfo> dictionary_0;
		public static ActiveInfo GetSingleActive(int id)
		{
			if (ActiveMgr.dictionary_0.Count == 0)
			{
				ActiveMgr.ReLoad();
			}
			if (ActiveMgr.dictionary_0.ContainsKey(id))
			{
				return ActiveMgr.dictionary_0[id];
			}
			return null;
		}
		public static bool Init()
		{
			return ActiveMgr.ReLoad();
		}
		private static bool IsInGrade(string limitGrade, int playerGrade)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			if (limitGrade != null)
			{
				string[] strArray = limitGrade.Split(new char[]
				{
					'-'
				});
				if (strArray.Length == 2)
				{
					num = Convert.ToInt32(strArray[0]);
					num2 = Convert.ToInt32(strArray[1]);
				}
				if (num <= playerGrade && num2 >= playerGrade)
				{
					flag = true;
				}
			}
			return flag;
		}
		public static bool IsValid(ActiveConditionInfo info)
		{
			DateTime arg_06_0 = info.StartTime;
			DateTime arg_0D_0 = info.EndTime;
			return info.StartTime.Ticks <= DateTime.Now.Ticks && info.EndTime.Ticks >= DateTime.Now.Ticks;
		}
		public static Dictionary<int, ActiveAwardInfo> LoadActiveAwardDb(Dictionary<int, List<ActiveConditionInfo>> conditions)
		{
			Dictionary<int, ActiveAwardInfo> dictionary = new Dictionary<int, ActiveAwardInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ActiveAwardInfo[] allActiveAwardInfo = bussiness.GetAllActiveAwardInfo();
				foreach (int num in conditions.Keys)
				{
					ActiveAwardInfo[] array = allActiveAwardInfo;
					for (int i = 0; i < array.Length; i++)
					{
						ActiveAwardInfo info = array[i];
						if (num == info.ActiveID && !dictionary.ContainsKey(info.ID))
						{
							dictionary.Add(info.ID, info);
						}
					}
				}
			}
			return dictionary;
		}
		public static Dictionary<int, List<ActiveConditionInfo>> LoadActiveConditionDb()
		{
			Dictionary<int, List<ActiveConditionInfo>> dictionary = new Dictionary<int, List<ActiveConditionInfo>>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ActiveConditionInfo[] allActiveConditionInfo = bussiness.GetAllActiveConditionInfo();
				ActiveConditionInfo[] array = allActiveConditionInfo;
				for (int i = 0; i < array.Length; i++)
				{
					ActiveConditionInfo info = array[i];
					List<ActiveConditionInfo> list = new List<ActiveConditionInfo>();
					if (!dictionary.ContainsKey(info.ActiveID))
					{
						list.Add(info);
						dictionary.Add(info.ActiveID, list);
					}
					else
					{
						dictionary[info.ActiveID].Add(info);
					}
				}
			}
			return dictionary;
		}
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, List<ActiveConditionInfo>> conditions = ActiveMgr.LoadActiveConditionDb();
				Dictionary<int, ActiveAwardInfo> dictionary2 = ActiveMgr.LoadActiveAwardDb(conditions);
				if (conditions.Count > 0)
				{
					Interlocked.Exchange<Dictionary<int, List<ActiveConditionInfo>>>(ref ActiveMgr.m_ActiveConditionInfo, conditions);
					Interlocked.Exchange<Dictionary<int, ActiveAwardInfo>>(ref ActiveMgr.m_ActiveAwardInfo, dictionary2);
				}
				return true;
			}
			catch (Exception exception)
			{
				ActiveMgr.log.Error("QuestMgr", exception);
			}
			return false;
		}
	}
}
