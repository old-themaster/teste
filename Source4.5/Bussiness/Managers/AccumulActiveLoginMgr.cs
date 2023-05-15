using log4net;
using SqlDataProvider.Data;
using System.Linq;

namespace Bussiness.Managers
{
    public static class AccumulActiveLoginMgr
    {
        private static readonly ILog ilog_0;

        private static AccumulAtiveLoginAwardInfo[] accumulAtiveLoginAwardInfo_0;

        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>> dictionary_0;

        private static ThreadSafeRandom threadSafeRandom_0;

        public static bool ReLoad()
        {
            bool result;
            bool flag;
            try
            {
                AccumulAtiveLoginAwardInfo[] array = AccumulActiveLoginMgr.LoadAccumulAtiveLoginAwardDb();
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>> value = AccumulActiveLoginMgr.LoadAccumulAtiveLoginAwards(array);
                if (array != null)
                {
                    System.Threading.Interlocked.Exchange<AccumulAtiveLoginAwardInfo[]>(ref AccumulActiveLoginMgr.accumulAtiveLoginAwardInfo_0, array);
                    System.Threading.Interlocked.Exchange<System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>>>(ref AccumulActiveLoginMgr.dictionary_0, value);
                }
                result = true;
                return result;
            }
            catch (System.Exception exception)
            {
                if (AccumulActiveLoginMgr.ilog_0.IsErrorEnabled)
                {
                    AccumulActiveLoginMgr.ilog_0.Error("ReLoad", exception);
                }
                flag = false;
            }
            result = flag;
            return result;
        }

        public static bool Init()
        {
            return AccumulActiveLoginMgr.ReLoad();
        }

        public static AccumulAtiveLoginAwardInfo[] LoadAccumulAtiveLoginAwardDb()
        {
            AccumulAtiveLoginAwardInfo[] result;
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
            {
                AccumulAtiveLoginAwardInfo[] accumulAtiveLoginAwardInfos = produceBussiness.GetAccumulAtiveLoginAwardInfos();
                result = accumulAtiveLoginAwardInfos;
            }
            return result;
        }

        public static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>> LoadAccumulAtiveLoginAwards(AccumulAtiveLoginAwardInfo[] AccumulAtiveLoginAwards)
        {
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>> dictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>>();
            for (int i = 0; i < AccumulAtiveLoginAwards.Length; i++)
            {
                AccumulAtiveLoginAwardInfo info = AccumulAtiveLoginAwards[i];
                if (!dictionary.Keys.Contains(info.Count))
                {
                    System.Collections.Generic.IEnumerable<AccumulAtiveLoginAwardInfo> source = from s in AccumulAtiveLoginAwards
                                                                                                where s.Count == info.Count
                                                                                                select s;
                    dictionary.Add(info.Count, source.ToList<AccumulAtiveLoginAwardInfo>());
                }
            }
            return dictionary;
        }

        public static System.Collections.Generic.List<AccumulAtiveLoginAwardInfo> FindAccumulAtiveLoginAward(int Count)
        {
            System.Collections.Generic.List<AccumulAtiveLoginAwardInfo> result;
            if (AccumulActiveLoginMgr.dictionary_0.ContainsKey(Count))
            {
                result = AccumulActiveLoginMgr.dictionary_0[Count];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static System.Collections.Generic.List<ItemInfo> GetAllAccumulAtiveLoginAward(int Count)
        {
            System.Collections.Generic.List<AccumulAtiveLoginAwardInfo> list = AccumulActiveLoginMgr.FindAccumulAtiveLoginAward(Count);
            System.Collections.Generic.List<ItemInfo> list2 = new System.Collections.Generic.List<ItemInfo>();
            if (list != null)
            {
                foreach (AccumulAtiveLoginAwardInfo current in list)
                {
                    ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(current.RewardItemID), current.RewardItemCount, 105);
                    itemInfo.IsBinds = current.IsBind;
                    itemInfo.ValidDate = current.RewardItemValid;
                    itemInfo.StrengthenLevel = current.StrengthenLevel;
                    itemInfo.AttackCompose = current.AttackCompose;
                    itemInfo.DefendCompose = current.DefendCompose;
                    itemInfo.AgilityCompose = current.AgilityCompose;
                    itemInfo.LuckCompose = current.LuckCompose;
                    list2.Add(itemInfo);
                }
            }
            return list2;
        }

        public static System.Collections.Generic.List<ItemInfo> GetSelecedAccumulAtiveLoginAward(int ID)
        {
            System.Collections.Generic.List<ItemInfo> list = new System.Collections.Generic.List<ItemInfo>();
            System.Collections.Generic.List<AccumulAtiveLoginAwardInfo> list2 = AccumulActiveLoginMgr.FindAccumulAtiveLoginAward(7);
            if (list2 != null)
            {
                foreach (AccumulAtiveLoginAwardInfo current in list2)
                {
                    if (ID == current.ID)
                    {
                        ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(current.RewardItemID), current.RewardItemCount, 105);
                        itemInfo.IsBinds = current.IsBind;
                        itemInfo.ValidDate = current.RewardItemValid;
                        itemInfo.StrengthenLevel = current.StrengthenLevel;
                        itemInfo.AttackCompose = current.AttackCompose;
                        itemInfo.DefendCompose = current.DefendCompose;
                        itemInfo.AgilityCompose = current.AgilityCompose;
                        itemInfo.LuckCompose = current.LuckCompose;
                        list.Add(itemInfo);
                        break;
                    }
                }
            }
            return list;
        }

        static AccumulActiveLoginMgr()
        {
            AccumulActiveLoginMgr.ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            AccumulActiveLoginMgr.dictionary_0 = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<AccumulAtiveLoginAwardInfo>>();
            AccumulActiveLoginMgr.threadSafeRandom_0 = new ThreadSafeRandom();
        }
    }
}
