// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.ItemBoxMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class ItemBoxMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ItemBoxInfo[] m_itemBox;
        private static Dictionary<int, List<ItemBoxInfo>> m_itemBoxs;
        private static ThreadSafeRandom random = new ThreadSafeRandom();

        public static bool ReLoad()
        {
            try
            {
                ItemBoxInfo[] itemBoxs = ItemBoxMgr.LoadItemBoxDb();
                Dictionary<int, List<ItemBoxInfo>> dictionary = ItemBoxMgr.LoadItemBoxs(itemBoxs);
                if (itemBoxs != null)
                {
                    Interlocked.Exchange<ItemBoxInfo[]>(ref ItemBoxMgr.m_itemBox, itemBoxs);
                    Interlocked.Exchange<Dictionary<int, List<ItemBoxInfo>>>(ref ItemBoxMgr.m_itemBoxs, dictionary);
                }
            }
            catch (Exception ex)
            {
                if (ItemBoxMgr.log.IsErrorEnabled)
                    ItemBoxMgr.log.Error((object)nameof(ReLoad), ex);
                return false;
            }
            return true;
        }

        public static bool Init() => ItemBoxMgr.ReLoad();

        public static ItemBoxInfo[] LoadItemBoxDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                return produceBussiness.GetItemBoxInfos();
        }
      
        public static Dictionary<int, List<ItemBoxInfo>> LoadItemBoxs(
          ItemBoxInfo[] itemBoxs)
        {
            Dictionary<int, List<ItemBoxInfo>> dictionary = new Dictionary<int, List<ItemBoxInfo>>();
            for (int index = 0; index < itemBoxs.Length; ++index)
            {
                ItemBoxInfo info = itemBoxs[index];
                if (!dictionary.Keys.Contains<int>(info.ID))
                {
                    IEnumerable<ItemBoxInfo> source = ((IEnumerable<ItemBoxInfo>)itemBoxs).Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.ID == info.ID));
                    dictionary.Add(info.ID, source.ToList<ItemBoxInfo>());
                }
            }
            return dictionary;
        }

        public static List<ItemBoxInfo> FindItemBox(int DataId) => ItemBoxMgr.m_itemBoxs.ContainsKey(DataId) ? ItemBoxMgr.m_itemBoxs[DataId] : (List<ItemBoxInfo>)null;


        public static bool CreateItemBox(
     int DateId,
     List<SqlDataProvider.Data.ItemInfo> itemInfos,
     ref int gold,
     ref int point,
     ref int giftToken,
     ref int medal,
    // ref int Ascension,
     ref int exp,
     ref int myHonor)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> itemBox = ItemBoxMgr.FindItemBox(DateId);
            if (itemBox == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < itemBox.Count)
                maxRound = ThreadSafeRandom.NextStatic(itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
            List<ItemBoxInfo> list = itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect && s.Random >= maxRound)).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -1100:
                        giftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -800:
                        myHonor += itemBoxInfo.ItemCount;
                        continue;
                    case -300:
                        medal += itemBoxInfo.ItemCount;
                        continue;
                    case -1111:
                      //  Ascension += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        point += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        gold += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        exp += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.Count = itemBoxInfo.ItemCount;
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            fromTemplate.IsTips = (uint)itemBoxInfo.IsTips > 0U;
                            fromTemplate.IsLogs = itemBoxInfo.IsLogs;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }
        public static ItemBoxInfo FindSpecialItemBox(int DataId)
        {
            ItemBoxInfo itemBoxInfo = new ItemBoxInfo();
            switch (DataId)
            {
                case -1100:
                    itemBoxInfo.TemplateId = 11213;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -300:
                    itemBoxInfo.TemplateId = 11420;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -200:
                    itemBoxInfo.TemplateId = 112244;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -100:
                    itemBoxInfo.TemplateId = 11233;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case 11408:
                    itemBoxInfo.TemplateId = 11420;
                    itemBoxInfo.ItemCount = 1;
                    break;
            }
            return itemBoxInfo;
        }

        public static bool CreateItemBox(
          int DateId,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          SpecialItemDataInfo specialInfo) => ItemBoxMgr.CreateItemBox(DateId, (List<ItemBoxInfo>)null, itemInfos, specialInfo);

        public static bool CreateItemBox(
          int DateId,
          List<ItemBoxInfo> tempBox,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          SpecialItemDataInfo specialInfo)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> source = ItemBoxMgr.FindItemBox(DateId);
            if (tempBox != null && tempBox.Count > 0)
                source = tempBox;
            if (source == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < source.Count)
            {
                maxRound = ThreadSafeRandom.NextStatic(source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
                if (maxRound <= 0)
                {
                    ItemBoxMgr.log.Error((object)("ItemBoxMgr Random Error: " + (object)maxRound + " | " + (object)DateId));
                    maxRound = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max();
                }
            }
            List<ItemBoxInfo> list = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect && s.Random >= maxRound)).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -300:
                        specialInfo.GiftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        specialInfo.Money += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        specialInfo.Gold += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        specialInfo.GP += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }
       
        public static void CreateItemBox(int dateId, List<ItemInfo> itemInfoList, ref int gold, ref int point, ref int giftToken, ref int medal, ref int Ascension, ref int exp, ref int honor, ref int hardCurrency, ref int leagueMoney, ref int useableScore, ref int prestge, ref int magicStonePoint, ref int loveNum)
        {
            throw new NotImplementedException();
        }
        public static bool CreateItemBox(
          int DateId,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          ref int gold,
          ref int point,
          ref int giftToken,
          ref int medal,
         // ref int Ascension,
          ref int exp)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> itemBox = ItemBoxMgr.FindItemBox(DateId);
            if (itemBox == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < itemBox.Count)
                maxRound = ThreadSafeRandom.NextStatic(itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
            List<ItemBoxInfo> list = itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect && s.Random >= maxRound)).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -1100:
                        giftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -300:
                        medal += itemBoxInfo.ItemCount;
                        continue;
                    case -1111:
                      //  Ascension += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        point += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        gold += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        exp += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.Count = itemBoxInfo.ItemCount;
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            fromTemplate.IsTips = (uint)itemBoxInfo.IsTips > 0U;
                            fromTemplate.IsLogs = itemBoxInfo.IsLogs;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }
        public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int medal, ref int Ascension, ref int exp, ref int honor, ref int hardCurrency, ref int leagueMoney, ref int useableScore, ref int prestge, ref int magicStonePoint)
        {
            List<ItemBoxInfo> FiltInfos = new List<ItemBoxInfo>();
            List<ItemBoxInfo> unFiltInfos = ItemBoxMgr.FindItemBox(DateId);
            if (unFiltInfos == null)
            {
                return false;
            }
            else
            {
                FiltInfos = (from s in unFiltInfos
                             where s.IsSelect
                             select s).ToList<ItemBoxInfo>();
                int dropItemCount = 1;
                int maxRound = 0;
                if (FiltInfos.Count < unFiltInfos.Count)
                {
                    maxRound = ThreadSafeRandom.NextStatic((from s in unFiltInfos
                                                            where !s.IsSelect
                                                            select s.Random).Max());
                }
                List<ItemBoxInfo> RoundInfos = (
                    from s in unFiltInfos
                    where !s.IsSelect && s.Random >= maxRound
                    select s).ToList<ItemBoxInfo>();
                int maxItems = RoundInfos.Count<ItemBoxInfo>();
                if (maxItems > 0)
                {
                    dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
                    int[] randomArray = ItemBoxMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
                    int[] array = randomArray;
                    for (int j = 0; j < array.Length; j++)
                    {
                        int i = array[j];
                        ItemBoxInfo item = RoundInfos[i];
                        if (FiltInfos == null)
                        {
                            FiltInfos = new List<ItemBoxInfo>();
                        }
                        FiltInfos.Add(item);
                    }
                }
                foreach (ItemBoxInfo info in FiltInfos)
                {
                    if (info == null)
                    {
                        return false;
                    }
                    int templateId = info.TemplateId;
                    if (templateId <= -1000)
                    {
                        if (templateId <= -1300)
                        {
                            if (templateId == -1400)
                            {
                                magicStonePoint += info.ItemCount;
                                continue;
                            }
                            if (templateId == -1300)
                            {
                                prestge += info.ItemCount;
                                continue;
                            }
                        }
                        else
                        {
                            if (templateId == -1200)
                            {
                                useableScore += info.ItemCount;
                                continue;
                            }
                            if (templateId == -1100)
                            {
                                giftToken += info.ItemCount;
                                continue;
                            }
                            if (templateId == -1000)
                            {
                                leagueMoney += info.ItemCount;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (templateId <= -300)
                        {
                            if (templateId == -900)
                            {
                                hardCurrency += info.ItemCount;
                                continue;
                            }
                            if (templateId == -800)
                            {
                                honor += info.ItemCount;
                                continue;
                            }
                            if (templateId == -300)
                            {
                                medal += info.ItemCount;
                                continue;
                            }
                            if (templateId == -1111)
                            {
                                Ascension += info.ItemCount;
                                continue;
                            }
                        }
                        else
                        {
                            if (templateId == -200)
                            {
                                point += info.ItemCount;
                                continue;
                            }
                            if (templateId == -100)
                            {
                                gold += info.ItemCount;
                                continue;
                            }
                            if (templateId == 11107)
                            {
                                exp += info.ItemCount;
                                continue;
                            }
                        }
                    }
                    ItemTemplateInfo temp = ItemMgr.FindItemTemplate(info.TemplateId);
                    ItemInfo item2 = ItemInfo.CreateFromTemplate(temp, info.ItemCount, 101);
                    if (item2 != null)
                    {
                        item2.Count = info.ItemCount;
                        item2.IsBinds = info.IsBind;
                        item2.ValidDate = info.ItemValid;
                        item2.StrengthenLevel = info.StrengthenLevel;
                        item2.AttackCompose = info.AttackCompose;
                        item2.DefendCompose = info.DefendCompose;
                        item2.AgilityCompose = info.AgilityCompose;
                        item2.LuckCompose = info.LuckCompose;
                        item2.IsTips = (info.IsTips != 0);
                        item2.IsLogs = info.IsLogs;
                        if (itemInfos == null)
                        {
                            itemInfos = new List<ItemInfo>();
                        }
                        itemInfos.Add(item2);
                    }
                }
                return true;
            }
        }
        public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            int[] numArray = new int[count];
            for (int index1 = 0; index1 < count; ++index1)
            {
                int num1 = ItemBoxMgr.random.Next(minValue, maxValue + 1);
                int num2 = 0;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    if (numArray[index2] == num1)
                        ++num2;
                }
                if (num2 == 0)
                    numArray[index1] = num1;
                else
                    --index1;
            }
            return numArray;
        }

        public static bool CreateItemBox(
          int DateId,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          ref int gold,
          ref int point,
          ref int giftToken,
          ref int exp) => ItemBoxMgr.CreateItemBox(DateId, (List<ItemBoxInfo>)null, itemInfos, ref gold, ref point, ref giftToken, ref exp);

        public static bool CreateItemBox(
          int DateId,
          List<ItemBoxInfo> tempBox,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          ref int gold,
          ref int point,
          ref int giftToken,
          ref int exp)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> source = ItemBoxMgr.FindItemBox(DateId);
            if (tempBox != null && tempBox.Count > 0)
                source = tempBox;
            if (source == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < source.Count)
                maxRound = ThreadSafeRandom.NextStatic(source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
            List<ItemBoxInfo> list = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect && s.Random >= maxRound)).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -300:
                        giftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        point += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        gold += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        exp += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }

        public static List<ItemBoxInfo> FindLotteryItemBoxByRand(
          int DateId,
          int countSelect)
        {
            List<ItemBoxInfo> lotteryItemBox = ItemBoxMgr.FindLotteryItemBox(DateId);
            List<ItemBoxInfo> itemBoxInfoList = new List<ItemBoxInfo>();
            for (int index1 = 0; index1 < countSelect; ++index1)
            {
                int index2 = ThreadSafeRandom.NextStatic(0, lotteryItemBox.Count);
                if (index2 < lotteryItemBox.Count)
                {
                    itemBoxInfoList.Add(lotteryItemBox[index2]);
                    lotteryItemBox.Remove(lotteryItemBox[index2]);
                }
            }
            return itemBoxInfoList;
        }

        public static List<ItemBoxInfo> FindLotteryItemBox(int DataId)
        {
            if (!ItemBoxMgr.m_itemBoxs.ContainsKey(DataId))
                return (List<ItemBoxInfo>)null;
            List<ItemBoxInfo> itemBoxInfoList = new List<ItemBoxInfo>();
            using (List<ItemBoxInfo>.Enumerator enumerator = ItemBoxMgr.m_itemBoxs[DataId].GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ItemBoxInfo current = enumerator.Current;
                    bool flag = true;
                    foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList)
                    {
                        if (itemBoxInfo.TemplateId == current.TemplateId && itemBoxInfo.ItemCount == current.ItemCount)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                        itemBoxInfoList.Add(current);
                }
                return itemBoxInfoList;
            }
        }
    }
}
