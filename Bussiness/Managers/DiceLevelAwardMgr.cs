// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.DiceLevelAwardMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C296B30-7657-4846-9B22-51DBD7C5C5FE
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\dll8.6\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class DiceLevelAwardMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static DiceLevelAwardInfo[] m_diceLevelAward;
        private static Dictionary<int, List<DiceLevelAwardInfo>> m_DiceLevelAwards;
        private static ThreadSafeRandom random = new ThreadSafeRandom();

        public static bool ReLoad()
        {
            try
            {
                DiceLevelAwardInfo[] DiceLevelAwards = DiceLevelAwardMgr.LoadDiceLevelAwardDb();
                Dictionary<int, List<DiceLevelAwardInfo>> dictionary = DiceLevelAwardMgr.LoadDiceLevelAwards(DiceLevelAwards);
                if (DiceLevelAwards != null)
                {
                    Interlocked.Exchange<DiceLevelAwardInfo[]>(ref DiceLevelAwardMgr.m_diceLevelAward, DiceLevelAwards);
                    Interlocked.Exchange<Dictionary<int, List<DiceLevelAwardInfo>>>(ref DiceLevelAwardMgr.m_DiceLevelAwards, dictionary);
                }
            }
            catch (Exception ex)
            {
                if (DiceLevelAwardMgr.log.IsErrorEnabled)
                    DiceLevelAwardMgr.log.Error((object)nameof(ReLoad), ex);
                return false;
            }
            return true;
        }

        public static bool Init() => DiceLevelAwardMgr.ReLoad();

        public static DiceLevelAwardInfo[] LoadDiceLevelAwardDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                return produceBussiness.GetDiceLevelAwardInfos();
        }

        public static Dictionary<int, List<DiceLevelAwardInfo>> LoadDiceLevelAwards(
          DiceLevelAwardInfo[] DiceLevelAwards)
        {
            Dictionary<int, List<DiceLevelAwardInfo>> dictionary = new Dictionary<int, List<DiceLevelAwardInfo>>();
            foreach (DiceLevelAwardInfo diceLevelAward in DiceLevelAwards)
            {
                DiceLevelAwardInfo info = diceLevelAward;
                if (!dictionary.Keys.Contains<int>(info.DiceLevel))
                {
                    IEnumerable<DiceLevelAwardInfo> source = ((IEnumerable<DiceLevelAwardInfo>)DiceLevelAwards).Where<DiceLevelAwardInfo>((Func<DiceLevelAwardInfo, bool>)(s => s.DiceLevel == info.DiceLevel));
                    dictionary.Add(info.DiceLevel, source.ToList<DiceLevelAwardInfo>());
                }
            }
            return dictionary;
        }

        public static List<DiceLevelAwardInfo> FindDiceLevelAward(int DataId) => DiceLevelAwardMgr.m_DiceLevelAwards.ContainsKey(DataId) ? DiceLevelAwardMgr.m_DiceLevelAwards[DataId] : (List<DiceLevelAwardInfo>)null;

        public static List<DiceLevelAwardInfo> GetAllDiceLevelAwardAward(
          int DataId)
        {
            return DiceLevelAwardMgr.FindDiceLevelAward(DataId);
        }
    }
}
