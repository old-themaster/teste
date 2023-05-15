using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bussiness;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.GypsyShop
{
    public class GypsyShopMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Random rand;
        protected static Timer _dbTimer;

        public static void BeginTimer()
        {
            int interval = 1 * 60 * 1000;
            if (_dbTimer == null)
            {
                _dbTimer = new Timer(new TimerCallback(GypsyTimeCheck), null, interval, interval);
            }
            else
            {
                _dbTimer.Change(interval, interval);
            }
        }

        private static bool _openOrClose;

        public static bool OpenOrClose
        {
            get { return _openOrClose; }
        }

        private static bool _freshTime;

        protected static void GypsyTimeCheck(object sender)
        {
            try
            {
                int startTick = Environment.TickCount;
                ThreadPriority oldprio = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                //some code  
                int startTime = int.Parse(GameProperties.MysteryShopOpenTime.Split('|')[0]);
                int endTime = int.Parse(GameProperties.MysteryShopOpenTime.Split('|')[1]);
                int currentHour = DateTime.Now.Hour;
                GamePlayer[] players = WorldMgr.GetAllPlayers();
                if (currentHour >= endTime && currentHour < startTime && OpenOrClose)
                {
                    _openOrClose = false;
                    foreach (GamePlayer p in players)
                    {
                        if (p != null)
                        {
                          //  p.Actives.SendGypsyShopOpenClose(false);
                        }
                    }
                }
                else if (currentHour >= startTime && !OpenOrClose)
                {
                    _openOrClose = true;
                    foreach (GamePlayer p in players)
                    {
                        if (p != null)
                        {
                            if (p != null)
                            {
                                p.Actives.ResetMysteryShop();
                                p.Actives.SendGypsyShopOpenClose(true);
                            }
                        }
                    }
                }

                int mysteryShopFreshTime = GameProperties.MysteryShopFreshTime;
                if (mysteryShopFreshTime == currentHour && _freshTime == false)
                {
                    _freshTime = true;
                    foreach (GamePlayer p in players)
                    {
                        if (p != null)
                        {
                            if (p != null)
                            {
                                p.Actives.RefreshMysteryShopByHour();
                            }
                        }
                    }
                }

                if (currentHour != mysteryShopFreshTime && _freshTime == true)
                {
                    _freshTime = false;
                }

                //end code
                Thread.CurrentThread.Priority = oldprio;
                startTick = Environment.TickCount - startTick;
            }
            catch (Exception e)
            {
                Console.WriteLine("Gypsy TimeCheck: " + e);
            }
        }

        public static void StopAllTimer()
        {
            if (_dbTimer != null)
            {
                _dbTimer.Dispose();
                _dbTimer = null;
            }
        }

        private static Dictionary<int, List<MysteryShopInfo>> m_MysteryShops =
            new Dictionary<int, List<MysteryShopInfo>>();

        public static bool Init()
        {
            try
            {
                _openOrClose = false;
                rand = new Random();
                MysteryShopInfo[] tempMysteryShop = LoadMysteryShopDb();
                Dictionary<int, List<MysteryShopInfo>> tempMysteryShops = LoadMysteryShops(tempMysteryShop);
                if (tempMysteryShop.Length > 0)
                {
                    Interlocked.Exchange(ref m_MysteryShops, tempMysteryShops);
                }

                return true;
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("ReLoad MysteryShop", e);
                return false;
            }
            finally
            {
                BeginTimer();
            }
        }

        public static MysteryShopInfo[] LoadMysteryShopDb()
        {
            using (ProduceBussiness pb = new ProduceBussiness())
            {
                MysteryShopInfo[] infos = pb.GetAllMysteryShop();
                return infos;
            }
        }

        public static Dictionary<int, List<MysteryShopInfo>> LoadMysteryShops(MysteryShopInfo[] MysteryShop)
        {
            Dictionary<int, List<MysteryShopInfo>> infos = new Dictionary<int, List<MysteryShopInfo>>();
            foreach (MysteryShopInfo info in MysteryShop)
            {
                if (!infos.Keys.Contains(info.LableType))
                {
                    IEnumerable<MysteryShopInfo> temp = MysteryShop.Where(s => s.LableType == info.LableType);
                    infos.Add(info.LableType, temp.ToList());
                }
            }

            return infos;
        }

        public static List<MysteryShopInfo> FindMysteryShop(int LableType)
        {
            List<MysteryShopInfo> items = new List<MysteryShopInfo>();
            if (m_MysteryShops.ContainsKey(LableType))
            {
                List<MysteryShopInfo> temps = m_MysteryShops[LableType];
                foreach (MysteryShopInfo rate in temps)
                {
                    items.Add(rate);
                }
            }

            return items;
        }

        public static List<MysteryShopInfo> GetRateMysteryShop()
        {
            List<MysteryShopInfo> items = new List<MysteryShopInfo>();
            List<MysteryShopInfo> temps = FindMysteryShop(2);
            int maxCount = temps.Count > 6 ? 6 : temps.Count;
            for (int i = 0; i < maxCount; i++)
            {
                MysteryShopInfo info = temps[i];
                items.Add(info);
            }

            return items;
        }

        public static List<MysteryShopInfo> GetMysteryShop()
        {
            List<MysteryShopInfo> items = new List<MysteryShopInfo>();
            List<MysteryShopInfo> qualityInfos = FindMysteryShop(2);
            for (int i = 0; items.Count < 8; i++)
            {
                List<MysteryShopInfo> infos = GetRateAward();
                foreach (MysteryShopInfo info in infos)
                {
                    foreach (MysteryShopInfo q in qualityInfos)
                    {
                        if (info.InfoID == q.InfoID)
                        {
                            info.Quality = 1;
                            break;
                        }
                    }

                    items.Add(info);
                    //Console.WriteLine("rate {0}", rate.Random);
                }
            }

            //rand.ShufferList(items);
            return items;
        }

        public static List<MysteryShopInfo> GetRateAward()
        {
            List<MysteryShopInfo> FiltInfos = new List<MysteryShopInfo>();
            List<MysteryShopInfo> unFiltInfos = FindMysteryShop(1);
            int dropItemCount = 1;
            int maxRound = ThreadSafeRandom.NextStatic(unFiltInfos.Select(s => s.Random).Max());
            List<MysteryShopInfo> RoundInfos = unFiltInfos.Where(s => s.Random >= maxRound).ToList();
            int maxItems = RoundInfos.Count();
            if (maxItems > 0)
            {
                dropItemCount = dropItemCount > maxItems ? maxItems : dropItemCount;
                int[] randomArray = GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
                foreach (int i in randomArray)
                {
                    MysteryShopInfo item = RoundInfos[i];
                    FiltInfos.Add(item);
                }
            }

            return FiltInfos;
        }

        public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            int j;
            int[] resultRound = new int[count];
            for (j = 0; j < count; j++)
            {
                int i = rand.Next(minValue, maxValue + 1);
                int num = 0;
                for (int k = 0; k < j; k++)
                {
                    if (resultRound[k] == i)
                    {
                        num = num + 1;
                    }
                }

                if (num == 0)
                {
                    resultRound[j] = i;
                }
                else
                {
                    j = j - 1;
                }
            }

            return resultRound;
        }
    }
}