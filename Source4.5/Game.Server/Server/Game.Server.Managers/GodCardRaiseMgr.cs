
using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
    public class GodCardRaiseMgr
    {
        private static readonly ILog ilog_0;
        private static Dictionary<int, Godcardpointrewardlist> dictionary_0;
        private static List<GodcardlistgroupCard> list_0;
        private static Dictionary<int, Godcardlistgroup> dictionary_1;
        private static List<Godcardlist> list_1;
        private static ReaderWriterLock readerWriterLock_0;
        private static ThreadSafeRandom threadSafeRandom_0;

        public static Godcardlist[] Get_Godcardlist()
        {
            Godcardlist[] godcardlistArray;
            try
            {
                if (GodCardRaiseMgr.list_1.Count > 0)
                {
                    godcardlistArray = GodCardRaiseMgr.list_1.ToArray();
                    goto label_5;
                }
            }
            catch
            {
            }
            godcardlistArray = (Godcardlist[])null;
        label_5:
            return godcardlistArray;
        }

        public static Godcardlist FindGodcard(int ID)
        {
            Godcardlist godcardlist1;
            foreach (Godcardlist godcardlist2 in GodCardRaiseMgr.list_1)
            {
                if (ID == godcardlist2.ID)
                {
                    godcardlist1 = godcardlist2;
                    goto label_7;
                }
            }
            godcardlist1 = (Godcardlist)null;
        label_7:
            return godcardlist1;
        }

        public static Godcardpointrewardlist FindRewardcard(int ID) => !GodCardRaiseMgr.dictionary_0.ContainsKey(ID) ? (Godcardpointrewardlist)null : GodCardRaiseMgr.dictionary_0[ID];

        public static GodcardlistgroupCard[] FindGodcardlistgroupCard(
          int GroupID,
          ref int GiftID)
        {
            List<GodcardlistgroupCard> godcardlistgroupCardList = new List<GodcardlistgroupCard>();
            if (GodCardRaiseMgr.dictionary_1.ContainsKey(GroupID))
            {
                Godcardlistgroup godcardlistgroup = GodCardRaiseMgr.dictionary_1[GroupID];
                GiftID = godcardlistgroup.GiftID;
            }
            foreach (GodcardlistgroupCard godcardlistgroupCard in GodCardRaiseMgr.list_0)
            {
                if (GroupID == godcardlistgroupCard.GroupID)
                    godcardlistgroupCardList.Add(godcardlistgroupCard);
            }
            return godcardlistgroupCardList.ToArray();
        }

        public static bool Init()
        {
            try
            {
                GodCardRaiseMgr.readerWriterLock_0 = new ReaderWriterLock();
                GodCardRaiseMgr.dictionary_0 = new Dictionary<int, Godcardpointrewardlist>();
                GodCardRaiseMgr.list_0 = new List<GodcardlistgroupCard>();
                GodCardRaiseMgr.dictionary_1 = new Dictionary<int, Godcardlistgroup>();
                GodCardRaiseMgr.list_1 = new List<Godcardlist>();
                GodCardRaiseMgr.threadSafeRandom_0 = new ThreadSafeRandom();
                return GodCardRaiseMgr.smethod_0();
            }
            catch (Exception ex)
            {
                if (GodCardRaiseMgr.ilog_0.IsErrorEnabled)
                    GodCardRaiseMgr.ilog_0.Error((object)nameof(GodCardRaiseMgr), ex);
                return false;
            }
        }

        private static bool smethod_0()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
            {
                foreach (Godcardlist godcardlist in produceBussiness.Get_Godcardlist())
                    GodCardRaiseMgr.list_1.Add(godcardlist);
                foreach (Godcardlistgroup godcardlistgroup in produceBussiness.Get_Godcardlistgroup())
                {
                    if (!GodCardRaiseMgr.dictionary_1.ContainsKey(godcardlistgroup.GroupID))
                        GodCardRaiseMgr.dictionary_1.Add(godcardlistgroup.GroupID, godcardlistgroup);
                }
                foreach (GodcardlistgroupCard godcardlistgroupCard in produceBussiness.Get_GodcardlistgroupCard())
                    GodCardRaiseMgr.list_0.Add(godcardlistgroupCard);
                foreach (Godcardpointrewardlist godcardpointrewardlist in produceBussiness.Get_Godcardpointrewardlist())
                {
                    if (!GodCardRaiseMgr.dictionary_0.ContainsKey(godcardpointrewardlist.ID))
                        GodCardRaiseMgr.dictionary_0.Add(godcardpointrewardlist.ID, godcardpointrewardlist);
                }
            }
            return true;
        }

        public GodCardRaiseMgr()
        {
            Class16.jxNons6zk6YVo();
            // ISSUE: explicit constructor call
   
        }

        static GodCardRaiseMgr()
        {
            Class17.kLjw4iIsCLsZtxc4lksN0j();
            Class16.jxNons6zk6YVo();
            GodCardRaiseMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
