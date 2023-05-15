using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Bussiness;
using Bussiness.Managers;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.Managers
{
	public class FusionMgr
	{
		private static Dictionary<string, FusionInfo> dictionary_0;

		private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static Items_Fusion_List_Info[] m_itemsfusionlist = null;

		private static Random random_0;

		private static ReaderWriterLock readerWriterLock_0;

		public static ItemTemplateInfo Fusion(List<ItemInfo> Items, List<ItemInfo> AppendItems, ref bool isBind, ref bool result)
		{
			Func<ItemTemplateInfo, bool> predicate = null;
			Func<ItemTemplateInfo, bool> func3 = null;
			List<int> list = new List<int>();
			int level = 0;
			int TotalRate = 0;
			int maxValue = 0;
			if (Items != null)
			{
				ItemTemplateInfo info = null;
				foreach (ItemInfo info3 in Items)
				{
					if (info3 != null)
					{
						list.Add(info3.Template.FusionType);
						if (info3.Template.Level > level)
						{
							level = info3.Template.Level;
						}
						TotalRate += info3.Template.FusionRate;
						maxValue += info3.Template.FusionNeedRate;
						if (info3.IsBinds)
						{
							isBind = true;
						}
					}
				}
				foreach (ItemInfo info4 in AppendItems)
				{
					TotalRate += info4.Template.FusionRate / 2;
					maxValue += info4.Template.FusionNeedRate / 2;
					if (info4.IsBinds)
					{
						isBind = true;
					}
				}
				list.Sort();
				StringBuilder builder = new StringBuilder();
				foreach (int num3 in list)
				{
					builder.Append(num3);
				}
				string key = builder.ToString();
				readerWriterLock_0.AcquireReaderLock(-1);
				try
				{
					if (dictionary_0.ContainsKey(key))
					{
						FusionInfo fusionInfo = dictionary_0[key];
						ItemTemplateInfo goodsbyFusionTypeandLevel = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, level);
						ItemTemplateInfo item = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, level + 1);
						ItemTemplateInfo info5 = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, level + 2);
						List<ItemTemplateInfo> source = new List<ItemTemplateInfo>();
						if (info5 != null)
						{
							source.Add(info5);
						}
						if (item != null)
						{
							source.Add(item);
						}
						if (goodsbyFusionTypeandLevel != null)
						{
							source.Add(goodsbyFusionTypeandLevel);
						}
						if (predicate == null)
						{
							predicate = (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate <= 1.1;
						}
						Func<ItemTemplateInfo, double> keySelector = (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate;
						ItemTemplateInfo info6 = source.Where(predicate).OrderByDescending(keySelector).FirstOrDefault();
						if (func3 == null)
						{
							func3 = (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate > 1.1;
						}
						Func<ItemTemplateInfo, double> func4 = (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate;
						ItemTemplateInfo info7 = source.Where(func3).OrderBy(func4).FirstOrDefault();
						if (info6 != null && info7 == null)
						{
							info = info6;
							Items_Fusion_List_Info info2 = null;
							for (int i = 0; i < m_itemsfusionlist.Count(); i++)
							{
								if (m_itemsfusionlist[i].TemplateID == info6.TemplateID)
								{
									info2 = m_itemsfusionlist[i];
									break;
								}
							}
							if (info2 != null)
							{
								if (new Random().Next(1, 100) < info2.Real)
								{
									result = true;
								}
								else
								{
									result = false;
								}
							}
							else
							{
								result = true;
							}
						}
						if (info6 != null && info7 != null)
						{
							if (info6.Level - info7.Level == 2)
							{
								_ = (double)(100 * TotalRate) * 0.6 / (double)info6.FusionNeedRate;
							}
							else
							{
								_ = (double)(100 * TotalRate) / (double)info6.FusionNeedRate;
							}
							if ((double)(100 * TotalRate) / (double)info6.FusionNeedRate > (double)random_0.Next(100))
							{
								info = info6;
								result = true;
							}
							else
							{
								info = info7;
								result = true;
							}
						}
						if (info6 == null && info7 != null)
						{
							info = info7;
							if (random_0.Next(maxValue) < TotalRate)
							{
								result = true;
							}
						}
						if (result)
						{
							using (List<ItemInfo>.Enumerator enumerator = Items.GetEnumerator())
							{
								do
								{
									if (!enumerator.MoveNext())
									{
										return info;
									}
								}
								while (enumerator.Current.Template.TemplateID != info.TemplateID);
								result = false;
							}
						}
						return info;
					}
				}
				catch
				{
				}
				finally
				{
					readerWriterLock_0.ReleaseReaderLock();
				}
			}
			return null;
		}

		public static Dictionary<int, double> FusionPreview(List<ItemInfo> Items, List<ItemInfo> AppendItems, ref bool isBind)
		{
			Func<ItemTemplateInfo, bool> predicate = null;
			Func<ItemTemplateInfo, bool> func3 = null;
			List<int> list = new List<int>();
			int level = 0;
			int TotalRate = 0;
			int num2 = 0;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Clear();
			foreach (ItemInfo info in Items)
			{
				list.Add(info.Template.FusionType);
				if (info.Template.Level > level)
				{
					level = info.Template.Level;
				}
				TotalRate += info.Template.FusionRate;
				num2 += info.Template.FusionNeedRate;
				if (info.IsBinds)
				{
					isBind = true;
				}
			}
			foreach (ItemInfo info3 in AppendItems)
			{
				TotalRate += info3.Template.FusionRate / 2;
				num2 += info3.Template.FusionRate / 2;
				if (info3.IsBinds)
				{
					isBind = true;
				}
			}
			list.Sort();
			StringBuilder builder = new StringBuilder();
			foreach (int num3 in list)
			{
				builder.Append(num3);
			}
			string key = builder.ToString().Trim();
			readerWriterLock_0.AcquireReaderLock(-1);
			try
			{
				if (dictionary_0.ContainsKey(key))
				{
					double num4 = 0.0;
					double num5 = 0.0;
					FusionInfo fusionInfo = dictionary_0[key];
					ItemTemplateInfo goodsbyFusionTypeandLevel = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, level);
					ItemTemplateInfo item = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, level + 1);
					ItemTemplateInfo info4 = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, level + 2);
					List<ItemTemplateInfo> source = new List<ItemTemplateInfo>();
					if (info4 != null)
					{
						source.Add(info4);
					}
					if (item != null)
					{
						source.Add(item);
					}
					if (goodsbyFusionTypeandLevel != null)
					{
						source.Add(goodsbyFusionTypeandLevel);
					}
					if (predicate == null)
					{
						predicate = (ItemTemplateInfo s) => (double)(TotalRate / s.FusionNeedRate) <= 1.1;
					}
					Func<ItemTemplateInfo, double> keySelector = (ItemTemplateInfo s) => TotalRate / s.FusionNeedRate;
					ItemTemplateInfo info5 = source.Where(predicate).OrderByDescending(keySelector).FirstOrDefault();
					if (func3 == null)
					{
						func3 = (ItemTemplateInfo s) => (double)(TotalRate / s.FusionNeedRate) > 1.1;
					}
					Func<ItemTemplateInfo, double> func4 = (ItemTemplateInfo s) => TotalRate / s.FusionNeedRate;
					ItemTemplateInfo info6 = source.Where(func3).OrderBy(func4).FirstOrDefault();
					if (info5 != null && info6 == null)
					{
						Items_Fusion_List_Info info7 = null;
						for (int num6 = 0; num6 < FusionCombined.m_itemsfusionlist.Count(); num6++)
						{
							if (FusionCombined.m_itemsfusionlist[num6].TemplateID == info5.TemplateID)
							{
								info7 = FusionCombined.m_itemsfusionlist[num6];
								break;
							}
						}
						if (info7 != null)
						{
							dictionary.Add(info5.TemplateID, info7.Show);
						}
						else
						{
							dictionary.Add(info5.TemplateID, 100.0);
						}
					}
					if (info5 != null && info6 != null)
					{
						if (info5.Level - info6.Level == 2)
						{
							num4 = (double)(100 * TotalRate) * 0.6 / (double)info5.FusionNeedRate;
							num5 = 100.0 - num4;
						}
						else
						{
							num4 = (double)(100 * TotalRate) / (double)info5.FusionNeedRate;
							num5 = 100.0 - num4;
						}
						dictionary.Add(info5.TemplateID, num4);
						dictionary.Add(info6.TemplateID, num5);
					}
					if (info5 == null && info6 != null)
					{
						Items_Fusion_List_Info info7 = null;
						for (int num6 = 0; num6 < FusionCombined.m_itemsfusionlist.Count(); num6++)
						{
							if (FusionCombined.m_itemsfusionlist[num6].TemplateID == info6.TemplateID)
							{
								info7 = FusionCombined.m_itemsfusionlist[num6];
								break;
							}
						}
						if (info7 != null)
						{
							dictionary.Add(info6.TemplateID, info7.Show);
						}
						else
						{
							dictionary.Add(info6.TemplateID, 100.0);
						}
					}
					int[] array = dictionary.Keys.ToArray();
					foreach (int num7 in array)
					{
						foreach (ItemInfo info2 in Items)
						{
							if (num7 == info2.Template.TemplateID && dictionary.ContainsKey(num7))
							{
								dictionary.Remove(num7);
							}
						}
					}
				}
				return dictionary;
			}
			catch
			{
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
			return null;
		}

		public static bool Init()
		{
			try
			{
				readerWriterLock_0 = new ReaderWriterLock();
				dictionary_0 = new Dictionary<string, FusionInfo>();
				random_0 = new Random();
				return smethod_0(dictionary_0);
			}
			catch (Exception exception)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("FusionMgr", exception);
				}
				return false;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<string, FusionInfo> dictionary = new Dictionary<string, FusionInfo>();
				if (smethod_0(dictionary))
				{
					readerWriterLock_0.AcquireWriterLock(-1);
					try
					{
						dictionary_0 = dictionary;
						return true;
					}
					catch
					{
					}
					finally
					{
						readerWriterLock_0.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("FusionMgr", exception);
				}
			}
			return false;
		}

		private static bool smethod_0(Dictionary<string, FusionInfo> PVQImtJIaVCSTenHlR1)
		{
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				FusionInfo[] allFusion = bussiness.GetAllFusion();
				foreach (FusionInfo info in allFusion)
				{
					List<int> list = new List<int>();
					list.Add(info.Item1);
					list.Add(info.Item2);
					list.Add(info.Item3);
					list.Add(info.Item4);
					list.Sort();
					StringBuilder builder = new StringBuilder();
					foreach (int num in list)
					{
						if (num != 0)
						{
							builder.Append(num);
						}
					}
					string key = builder.ToString();
					if (!PVQImtJIaVCSTenHlR1.ContainsKey(key))
					{
						PVQImtJIaVCSTenHlR1.Add(key, info);
					}
				}
				m_itemsfusionlist = bussiness.GetAllFusionList();
			}
			return true;
		}
	}
}
