// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.LightriddleQuestMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C296B30-7657-4846-9B22-51DBD7C5C5FE
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\DLL\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class LightriddleQuestMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<int, LightriddleQuestInfo> _lightriddleQuests;
        private static ReaderWriterLock m_lock;
        private static ThreadSafeRandom random = new ThreadSafeRandom();

        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, LightriddleQuestInfo> infos = new Dictionary<int, LightriddleQuestInfo>();
                if (LightriddleQuestMgr.LoadData(infos))
                {
                    LightriddleQuestMgr.m_lock.AcquireWriterLock(-1);
                    try
                    {
                        LightriddleQuestMgr._lightriddleQuests = infos;
                        return true;
                    }
                    catch
                    {
                    }
                    finally
                    {
                        LightriddleQuestMgr.m_lock.ReleaseWriterLock();
                    }
                }
            }
            catch (Exception ex)
            {
                if (LightriddleQuestMgr.log.IsErrorEnabled)
                    LightriddleQuestMgr.log.Error((object)nameof(ReLoad), ex);
            }
            return false;
        }

        public static bool Init()
        {
            try
            {
                LightriddleQuestMgr.m_lock = new ReaderWriterLock();
                LightriddleQuestMgr._lightriddleQuests = new Dictionary<int, LightriddleQuestInfo>();
                return LightriddleQuestMgr.LoadData(LightriddleQuestMgr._lightriddleQuests);
            }
            catch (Exception ex)
            {
                if (LightriddleQuestMgr.log.IsErrorEnabled)
                    LightriddleQuestMgr.log.Error((object)nameof(Init), ex);
                return false;
            }
        }

        public static bool LoadData(Dictionary<int, LightriddleQuestInfo> infos)
        {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                foreach (LightriddleQuestInfo lightriddleQuestInfo in playerBussiness.GetAllLightriddleQuestInfo())
                {
                    if (!infos.Keys.Contains<int>(lightriddleQuestInfo.QuestionID))
                        infos.Add(lightriddleQuestInfo.QuestionID, lightriddleQuestInfo);
                }
            }
            return true;
        }

        public static Dictionary<int, LightriddleQuestInfo> Get30LightriddleQuest()
        {
            if (LightriddleQuestMgr._lightriddleQuests == null)
                LightriddleQuestMgr.Init();
            Dictionary<int, LightriddleQuestInfo> dictionary1 = new Dictionary<int, LightriddleQuestInfo>();
            Dictionary<int, LightriddleQuestInfo> dictionary2 = new Dictionary<int, LightriddleQuestInfo>();
            LightriddleQuestMgr.m_lock.AcquireReaderLock(10000);
            try
            {
                int count = LightriddleQuestMgr._lightriddleQuests.Count;
                int key1 = 1;
                int num = 0;
                while (dictionary1.Count < 30)
                {
                    int key2 = LightriddleQuestMgr.random.Next(1, count);
                    LightriddleQuestInfo lightriddleQuest = LightriddleQuestMgr._lightriddleQuests[key2];
                    if (!dictionary2.Keys.Contains<int>(lightriddleQuest.QuestionID))
                    {
                        dictionary1.Add(key1, lightriddleQuest);
                        dictionary2.Add(lightriddleQuest.QuestionID, lightriddleQuest);
                        ++key1;
                    }
                    ++num;
                }
                return dictionary1;
            }
            finally
            {
                LightriddleQuestMgr.m_lock.ReleaseReaderLock();
            }
        }
    }
}
