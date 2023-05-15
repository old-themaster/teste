using Bussiness;
using Bussiness.Managers;
using Bussiness.Protocol;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Logic
{
	public class DropInventory
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static ThreadSafeRandom random = new ThreadSafeRandom();

		public static int roundDate = 0;

		public static bool AnswerDrop(int answerId, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Answer, answerId.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Answer, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool BossDrop(int missionId, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Boss, missionId.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Boss, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool BoxDrop(eRoomType e, ref List<ItemInfo> info)
		{
			int num = (int)e;
			int dropCondiction = GetDropCondiction(eDropType.Box, num.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Box, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool CardDrop(eRoomType e, ref List<ItemInfo> info)
		{
			int num = (int)e;
			int dropCondiction = GetDropCondiction(eDropType.Cards, num.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Cards, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool CopyAllDrop(int copyId, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Copy, copyId.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetAllDropItems(eDropType.Copy, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool CopyDrop(int copyId, int user, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Copy, copyId.ToString(), user.ToString());
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Copy, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static List<ItemInfo> CopySystemDrop(int copyId, int OpenCount)
		{
			int num = Convert.ToInt32((double)OpenCount * 0.1);
			int num2 = Convert.ToInt32((double)OpenCount * 0.3);
			int num3 = OpenCount - num - num2;
			List<ItemInfo> list = new List<ItemInfo>();
			List<ItemInfo> itemInfos = null;
			int dropCondiction = GetDropCondiction(eDropType.Copy, copyId.ToString(), "2");
			if (dropCondiction > 0)
			{
				for (int i = 0; i < num; i++)
				{
					if (GetDropItems(eDropType.Copy, dropCondiction, ref itemInfos))
					{
						list.Add(itemInfos[0]);
						itemInfos = null;
					}
				}
			}
			int dropCondiction2 = GetDropCondiction(eDropType.Copy, copyId.ToString(), "3");
			if (dropCondiction2 > 0)
			{
				for (int i = 0; i < num2; i++)
				{
					if (GetDropItems(eDropType.Copy, dropCondiction2, ref itemInfos))
					{
						list.Add(itemInfos[0]);
						itemInfos = null;
					}
				}
			}
			int dropCondiction3 = GetDropCondiction(eDropType.Copy, copyId.ToString(), "4");
			if (dropCondiction3 > 0)
			{
				for (int i = 0; i < num3; i++)
				{
					if (GetDropItems(eDropType.Copy, dropCondiction3, ref itemInfos))
					{
						list.Add(itemInfos[0]);
						itemInfos = null;
					}
				}
			}
			return RandomSortList(list);
		}

		public static bool FireDrop(eRoomType e, ref List<ItemInfo> info)
		{
			int num = (int)e;
			int dropCondiction = GetDropCondiction(eDropType.Fire, num.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Fire, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		private static bool GetAllDropItems(eDropType type, int dropId, ref List<ItemInfo> itemInfos)
		{
			if (dropId != 0)
			{
				try
				{
					int num = 1;
					List<DropItem> list = DropMgr.FindDropItem(dropId);
					int maxRound = ThreadSafeRandom.NextStatic((from s in list
					select s.Random).Max());
					int num2 = (from s in list
					where s.Random >= maxRound
					select s).ToList().Count();
					if (num2 == 0)
					{
						return false;
					}
					num = ((num > num2) ? num2 : num);
					GetRandomUnrepeatArray(0, num2 - 1, num);
					foreach (DropItem item in list)
					{
						int count = ThreadSafeRandom.NextStatic(item.BeginData, item.EndData);
						ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(item.ItemId);
						ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, count, 101);
						if (itemInfo != null)
						{
							itemInfo.IsBinds = item.IsBind;
							itemInfo.ValidDate = item.ValueDate;
							itemInfo.IsTips = item.IsTips;
							itemInfo.IsLogs = item.IsLogs;
							if (itemInfos == null)
							{
								itemInfos = new List<ItemInfo>();
							}
							if (DropInfoMgr.CanDrop(itemTemplateInfo.TemplateID))
							{
								itemInfos.Add(itemInfo);
							}
						}
					}
					return true;
				}
				catch
				{
					if (log.IsErrorEnabled)
					{
						log.Error("Drop Error：" + type + " dropId " + dropId);
					}
				}
			}
			return false;
		}

		public static bool GetDrop(int copyId, int user, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Trminhpc, copyId.ToString(), user.ToString());
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Trminhpc, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		private static int GetDropCondiction(eDropType type, string para1, string para2)
		{
			try
			{
				return DropMgr.FindCondiction(type, para1, para2);
			}
			catch (Exception ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Drop Error：" + type + " @ " + ex);
				}
			}
			return 0;
		}

		private static bool GetDropItems(eDropType type, int dropId, ref List<ItemInfo> itemInfos)
		{
			if (dropId != 0)
			{
				try
				{
					int num = 1;
					List<DropItem> source = DropMgr.FindDropItem(dropId);
					int maxRound = ThreadSafeRandom.NextStatic((from s in source
					select s.Random).Max());
					List<DropItem> list = (from s in source
					where s.Random >= maxRound
					select s).ToList();
					int num2 = list.Count();
					if (num2 == 0)
					{
						return false;
					}
					num = ((num > num2) ? num2 : num);
					int[] randomUnrepeatArray = GetRandomUnrepeatArray(0, num2 - 1, num);
					foreach (int index in randomUnrepeatArray)
					{
						int count = ThreadSafeRandom.NextStatic(list[index].BeginData, list[index].EndData);
						ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(list[index].ItemId);
						ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, count, 101);
						if (itemInfo != null)
						{
							itemInfo.IsBinds = list[index].IsBind;
							itemInfo.ValidDate = list[index].ValueDate;
							itemInfo.IsTips = list[index].IsTips;
							itemInfo.IsLogs = list[index].IsLogs;
							if (itemInfos == null)
							{
								itemInfos = new List<ItemInfo>();
							}
							if (DropInfoMgr.CanDrop(itemTemplateInfo.TemplateID))
							{
								itemInfos.Add(itemInfo);
							}
						}
					}
					return true;
				}
				catch
				{
					if (log.IsErrorEnabled)
					{
						log.Error("Drop Error：" + type + " dropId " + dropId);
					}
				}
			}
			return false;
		}

		public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
		{
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				int num = ThreadSafeRandom.NextStatic(minValue, maxValue + 1);
				int num2 = 0;
				for (int j = 0; j < i; j++)
				{
					if (array[j] == num)
					{
						num2++;
					}
				}
				if (num2 == 0)
				{
					array[i] = num;
				}
				else
				{
					i--;
				}
			}
			return array;
		}

		public static bool NPCDrop(int dropId, ref List<ItemInfo> info)
		{
			if (dropId > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.NPC, dropId, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool PvEQuestsDrop(int npcId, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.PveQuests, npcId.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.PveQuests, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool PvPQuestsDrop(eRoomType e, bool playResult, ref List<ItemInfo> info)
		{
			int num = (int)e;
			int dropCondiction = GetDropCondiction(eDropType.PvpQuests, num.ToString(), Convert.ToInt16(playResult).ToString());
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.PvpQuests, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static List<ItemInfo> RandomSortList(List<ItemInfo> list)
		{
			return (from key in list
			orderby random.Next()
			select key).ToList();
		}

		public static bool RetrieveDrop(int user, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Retrieve, user.ToString(), "0");
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Retrieve, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		public static bool SpecialDrop(int missionId, int boxType, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.Special, missionId.ToString(), boxType.ToString());
			if (dropCondiction > 0)
			{
				List<ItemInfo> itemInfos = null;
				if (GetDropItems(eDropType.Special, dropCondiction, ref itemInfos))
				{
					info = ((itemInfos != null) ? itemInfos : null);
					return true;
				}
			}
			return false;
		}

		private static bool GetDropPets(eDropType type, int dropId, ref List<PetTemplateInfo> petInfos)
		{
			if (dropId == 0)
				return false;
			try
			{
				int num1 = 1;
				List<DropItem> dropItem = DropMgr.FindDropItem(dropId);
				int maxRound = ThreadSafeRandom.NextStatic(dropItem.Select<DropItem, int>((Func<DropItem, int>)(s => s.Random)).Max());
				List<DropItem> list = dropItem.Where<DropItem>((Func<DropItem, bool>)(s => s.Random >= maxRound)).ToList<DropItem>();
				int num2 = list.Count<DropItem>();
				if (num2 == 0)
					return false;
				int count = num1 > num2 ? num2 : num1;
				foreach (int randomUnrepeat in DropInventory.GetRandomUnrepeatArray(0, num2 - 1, count))
				{
					ThreadSafeRandom.NextStatic(list[randomUnrepeat].BeginData, list[randomUnrepeat].EndData);
					PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(list[randomUnrepeat].ItemId);
					if (petTemplate != null)
					{
						if (petInfos == null)
							petInfos = new List<PetTemplateInfo>();
						if (DropInfoMgr.CanDrop(petTemplate.TemplateID))
							petInfos.Add(petTemplate);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				if (DropInventory.log.IsErrorEnabled)
					DropInventory.log.Error((object)("Drop Error：" + (object)type + " @ " + (object)ex));
			}
			return false;
		}

		public static bool GetPetDrop(int copyId, int user, ref List<PetTemplateInfo> info)
		{
			int dropCondiction = DropInventory.GetDropCondiction(eDropType.Trminhpc, copyId.ToString(), user.ToString());
			if (dropCondiction > 0)
			{
				List<PetTemplateInfo> petInfos = (List<PetTemplateInfo>)null;
				if (DropInventory.GetDropPets(eDropType.Trminhpc, dropCondiction, ref petInfos))
				{
					info = petInfos ?? (List<PetTemplateInfo>)null;
					return true;
				}
			}
			return false;
		}


		public static bool FightLabUserDrop(int copyId, ref List<ItemInfo> info)
		{
			int dropCondiction = GetDropCondiction(eDropType.FightLab, copyId.ToString(), "1");
			if (dropCondiction > 0)
			{
				List<DropItem> list = DropMgr.FindDropItem(dropCondiction);
				for (int i = 0; i < list.Count; i++)
				{
					int count = random.Next(list[i].BeginData, list[i].EndData);
					ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(list[i].ItemId), count, copyId);
					if (itemInfo != null)
					{
						itemInfo.IsBinds = list[i].IsBind;
						itemInfo.ValidDate = list[i].ValueDate;
						itemInfo.IsTips = list[i].IsTips;
						itemInfo.IsLogs = list[i].IsLogs;
						if (info == null)
						{
							info = new List<ItemInfo>();
						}
						info.Add(itemInfo);
					}
				}
				return true;
			}
			return false;
		}
	}
}
