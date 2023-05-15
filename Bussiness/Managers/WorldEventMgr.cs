// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.WorldEventMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System;
using System.Linq;

namespace Bussiness.Managers
{
    public class WorldEventMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ReaderWriterLock m_lock;
        private static Dictionary<int, LuckyStartToptenAwardInfo> m_lanternriddlesToptenAward;
        private static Dictionary<int, EntertamentModeAwardInfo> m_entertamentModeAward;
        private static ThreadSafeRandom random = new ThreadSafeRandom();

        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, LuckyStartToptenAwardInfo> templuckyStartToptenAward = new Dictionary<int, LuckyStartToptenAwardInfo>();
                Dictionary<int, LuckyStartToptenAwardInfo> templanternriddlesToptenAward = new Dictionary<int, LuckyStartToptenAwardInfo>();
                Dictionary<int, EntertamentModeAwardInfo> tempEntertamentModeAward = new Dictionary<int, EntertamentModeAwardInfo>();
                if (WorldEventMgr.LoadData(templuckyStartToptenAward, templanternriddlesToptenAward, tempEntertamentModeAward))
                {
                    WorldEventMgr.m_lock.AcquireWriterLock(-1);
                    try
                    {
                        WorldEventMgr.m_luckyStartToptenAward = templuckyStartToptenAward;
                        WorldEventMgr.m_lanternriddlesToptenAward = templanternriddlesToptenAward;
                        WorldEventMgr.m_entertamentModeAward = tempEntertamentModeAward;
                        return true;
                    }
                    catch
                    {
                    }
                    finally
                    {
                        WorldEventMgr.m_lock.ReleaseWriterLock();
                    }
                }
            }
            catch (Exception e)
            {
                if (WorldEventMgr.log.IsErrorEnabled)
                {
                    WorldEventMgr.log.Error("ReLoad", e);
                }
            }
            return false;
        }
        public static bool SendItemsToMail(
          List<ItemInfo> infos,
          int PlayerId,
          string Nickname,
          string title,
          string content = null)
        {
            bool flag = false;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                List<ItemInfo> itemInfoList = new List<ItemInfo>();
                foreach (ItemInfo info in infos)
                {
                    if (info.Template.MaxCount == 1)
                    {
                        for (int index = 0; index < info.Count; ++index)
                        {
                            ItemInfo itemInfo = ItemInfo.CloneFromTemplate(info.Template, info);
                            itemInfo.Count = 1;
                            itemInfoList.Add(itemInfo);
                        }
                    }
                    else
                        itemInfoList.Add(info);
                }
                for (int index1 = 0; index1 < itemInfoList.Count; index1 += 5)
                {
                    MailInfo mail = new MailInfo()
                    {
                        Title = title,
                        Content = content,
                        Gold = 0,
                        IsExist = true,
                        Money = 0,
                        Receiver = Nickname,
                        ReceiverID = PlayerId,
                        Sender = "Administrador do Sistema",
                        SenderID = 0,
                        Type = 9,
                        GiftToken = 0
                    };
                    StringBuilder stringBuilder1 = new StringBuilder();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    stringBuilder1.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark"));
                    int index2 = index1;
                    if (itemInfoList.Count > index2)
                    {
                        ItemInfo itemInfo = itemInfoList[index2];
                        if (itemInfo.ItemID == 0)
                            playerBussiness.AddGoods(itemInfo);
                        mail.Annex1 = itemInfo.ItemID.ToString();
                        mail.Annex1Name = itemInfo.Template.Name;
                        stringBuilder1.Append("1、" + mail.Annex1Name + "x" + (object)itemInfo.Count + ";");
                        stringBuilder2.Append("1、" + mail.Annex1Name + "x" + (object)itemInfo.Count + ";");
                    }
                    int index3 = index1 + 1;
                    if (itemInfoList.Count > index3)
                    {
                        ItemInfo itemInfo = itemInfoList[index3];
                        if (itemInfo.ItemID == 0)
                            playerBussiness.AddGoods(itemInfo);
                        mail.Annex2 = itemInfo.ItemID.ToString();
                        mail.Annex2Name = itemInfo.Template.Name;
                        stringBuilder1.Append("2、" + mail.Annex2Name + "x" + (object)itemInfo.Count + ";");
                        stringBuilder2.Append("2、" + mail.Annex2Name + "x" + (object)itemInfo.Count + ";");
                    }
                    int index4 = index1 + 2;
                    if (itemInfoList.Count > index4)
                    {
                        ItemInfo itemInfo = itemInfoList[index4];
                        if (itemInfo.ItemID == 0)
                            playerBussiness.AddGoods(itemInfo);
                        mail.Annex3 = itemInfo.ItemID.ToString();
                        mail.Annex3Name = itemInfo.Template.Name;
                        stringBuilder1.Append("3、" + mail.Annex3Name + "x" + (object)itemInfo.Count + ";");
                        stringBuilder2.Append("3、" + mail.Annex3Name + "x" + (object)itemInfo.Count + ";");
                    }
                    int index5 = index1 + 3;
                    if (itemInfoList.Count > index5)
                    {
                        ItemInfo itemInfo = itemInfoList[index5];
                        if (itemInfo.ItemID == 0)
                            playerBussiness.AddGoods(itemInfo);
                        mail.Annex4 = itemInfo.ItemID.ToString();
                        mail.Annex4Name = itemInfo.Template.Name;
                        stringBuilder1.Append("4、" + mail.Annex4Name + "x" + (object)itemInfo.Count + ";");
                        stringBuilder2.Append("4、" + mail.Annex4Name + "x" + (object)itemInfo.Count + ";");
                    }
                    int index6 = index1 + 4;
                    if (itemInfoList.Count > index6)
                    {
                        ItemInfo itemInfo = itemInfoList[index6];
                        if (itemInfo.ItemID == 0)
                            playerBussiness.AddGoods(itemInfo);
                        mail.Annex5 = itemInfo.ItemID.ToString();
                        mail.Annex5Name = itemInfo.Template.Name;
                        stringBuilder1.Append("5、" + mail.Annex5Name + "x" + (object)itemInfo.Count + ";");
                        stringBuilder2.Append("5、" + mail.Annex5Name + "x" + (object)itemInfo.Count + ";");
                    }
                    mail.AnnexRemark = stringBuilder1.ToString();
                    mail.Content = mail.Content ?? stringBuilder2.ToString();
                    flag = playerBussiness.SendMail(mail);
                }
            }
            return flag;
        }
        private static System.Collections.Generic.List<LuckStarRewardRecordInfo> m_recordList = new System.Collections.Generic.List<LuckStarRewardRecordInfo>();

        public static bool Init()
        {
            bool result;
            try
            {
                WorldEventMgr.m_lock = new ReaderWriterLock();
                WorldEventMgr.m_luckyStartToptenAward = new Dictionary<int, LuckyStartToptenAwardInfo>();
                WorldEventMgr.m_lanternriddlesToptenAward = new Dictionary<int, LuckyStartToptenAwardInfo>();
                 WorldEventMgr.m_entertamentModeAward = new Dictionary<int, EntertamentModeAwardInfo>();
                result = WorldEventMgr.LoadData(WorldEventMgr.m_luckyStartToptenAward, WorldEventMgr.m_lanternriddlesToptenAward, WorldEventMgr.m_entertamentModeAward);
            }
            catch (Exception e)
            {
                if (WorldEventMgr.log.IsErrorEnabled)
                {
                    WorldEventMgr.log.Error("Init", e);
                }
                result = false;
            }
            return result;
        }
        public static List<EntertamentModeAwardInfo> GetEntertamentAwardWithPoint(int point)
        {
            Dictionary<int, EntertamentModeAwardInfo> entertamentModeAward;
            Monitor.Enter(entertamentModeAward = WorldEventMgr.m_entertamentModeAward);
            List<EntertamentModeAwardInfo> result;
            try
            {
                List<EntertamentModeAwardInfo> infos = new List<EntertamentModeAwardInfo>();
                IOrderedEnumerable<KeyValuePair<int, EntertamentModeAwardInfo>> items =
                    from pair in WorldEventMgr.m_entertamentModeAward
                    orderby pair.Value.PointNeed descending
                    select pair;
                int pointGet = 0;
                foreach (KeyValuePair<int, EntertamentModeAwardInfo> item in items)
                {
                    if (point >= item.Value.PointNeed)
                    {
                        pointGet = item.Value.PointNeed;
                        break;
                    }
                }
                if (pointGet > 0)
                {
                    foreach (EntertamentModeAwardInfo item2 in WorldEventMgr.m_entertamentModeAward.Values)
                    {
                        if (item2.PointNeed == pointGet)
                        {
                            infos.Add(item2);
                        }
                    }
                }
                result = infos;
            }
            finally
            {
                Monitor.Exit(entertamentModeAward);
            }
            return result;
        }
        public static List<LuckyStartToptenAwardInfo> GetLuckyStartAwardByRank(int rank)
        {
            int type = 0;
            switch (rank)
            {
                case 1:
                    type = 11;
                    break;
                case 2:
                    type = 12;
                    break;
                case 3:
                    type = 13;
                    break;
                case 4:
                case 5:
                    type = 14;
                    break;
                case 6:
                case 7:
                    type = 15;
                    break;
                case 8:
                case 9:
                case 10:
                    type = 16;
                    break;
            }
            List<LuckyStartToptenAwardInfo> infos = new List<LuckyStartToptenAwardInfo>();
            foreach (LuckyStartToptenAwardInfo info in WorldEventMgr.m_luckyStartToptenAward.Values)
            {
                if (info.Type == type)
                {
                    infos.Add(info);
                }
            }
            return infos;
        }

        public static bool LoadData(Dictionary<int, LuckyStartToptenAwardInfo> luckyStarts, Dictionary<int, LuckyStartToptenAwardInfo> lanternriddles, Dictionary<int, EntertamentModeAwardInfo> entertamentModeAward)
        {
            using (PlayerBussiness db = new PlayerBussiness())
            {
                LuckyStartToptenAwardInfo[] luckyStartDbs = db.GetAllLuckyStartToptenAward();
                LuckyStartToptenAwardInfo[] array = luckyStartDbs;
                for (int i = 0; i < array.Length; i++)
                {
                    LuckyStartToptenAwardInfo award = array[i];
                    if (!luckyStarts.Keys.Contains(award.ID))
                    {
                        luckyStarts.Add(award.ID, award);
                    }
                }
                LuckyStartToptenAwardInfo[] lanternriddleDbs = db.GetAllLanternriddlesTopTenAward();
                LuckyStartToptenAwardInfo[] array2 = lanternriddleDbs;
                for (int j = 0; j < array2.Length; j++)
                {
                    LuckyStartToptenAwardInfo award2 = array2[j];
                    if (!lanternriddles.Keys.Contains(award2.ID))
                    {
                        lanternriddles.Add(award2.ID, award2);
                    }
                }              
            }
            return true;
        }

        public static System.Collections.Generic.List<LuckStarRewardRecordInfo> RecordList
        {
            get
            {
                return WorldEventMgr.m_recordList;
            }
        }
        private static Dictionary<int, LuckyStartToptenAwardInfo> m_luckyStartToptenAward;
        public static List<LuckyStartToptenAwardInfo> GetLuckyStartToptenAward()
        {
            List<LuckyStartToptenAwardInfo> list = new List<LuckyStartToptenAwardInfo>();
            foreach (LuckyStartToptenAwardInfo current in WorldEventMgr.m_luckyStartToptenAward.Values)
            {
                list.Add(current);
            }
            return list;
        }

        public static List<LuckyStartToptenAwardInfo> GetLanternriddlesAwardByRank(int rank)
        {
            List<LuckyStartToptenAwardInfo> infos = new List<LuckyStartToptenAwardInfo>();
            foreach (LuckyStartToptenAwardInfo info in WorldEventMgr.m_lanternriddlesToptenAward.Values)
            {
                if (info.Type == rank)
                {
                    infos.Add(info);
                }
            }
            return infos;
        }
        public static bool SendItemToMail(
      ItemInfo info,
      int PlayerId,
      string Nickname,
      int zoneId,
      AreaConfigInfo areaConfig,
      string title) => WorldEventMgr.SendItemsToMail(new List<ItemInfo>()
    {
      info
    }, PlayerId, Nickname, title);

        public static bool SendItemsToMails(
          List<ItemInfo> infos,
          int PlayerId,
          string Nickname,
          int zoneId,
          AreaConfigInfo areaConfig,
          string title) => WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title);

        public static bool SendItemsToMails(
          List<ItemInfo> infos,
          int PlayerId,
          string Nickname,
          int zoneId,
          AreaConfigInfo areaConfig,
          string title,
          string content) => WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title);

        public static bool SendItemToMail(
          ItemInfo info,
          int PlayerId,
          string Nickname,
          int zoneId,
          AreaConfigInfo areaConfig,
          string title,
          string sender) => WorldEventMgr.SendItemsToMail(new List<ItemInfo>()
        {
      info
        }, PlayerId, Nickname, title);

        public static bool SendItemsToMail(
          List<ItemInfo> infos,
          int PlayerId,
          string Nickname,
          int zoneId,
          AreaConfigInfo areaConfig,
          string title,
          int type,
          string sender) => WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title);

        public static bool SendItemsToMail(
          List<ItemInfo> infos,
          int PlayerId,
          string Nickname,
          int zoneId,
          AreaConfigInfo areaConfig,
          string title,
          string content) => WorldEventMgr.SendItemsToMail(infos, PlayerId, Nickname, title);
    }
}
