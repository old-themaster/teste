// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.ScheduleMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using Bussiness;
using log4net;
using System;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
    internal class ScheduleMgr
    {
        private static ServiceBussiness serviceBussiness = new ServiceBussiness();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static System.Threading.Timer m_UpdateSchedule;
        private static System.Timers.Timer m_SuperWinnerTimer;
        private static bool mail;
        private static ReaderWriterLock m_lock;

        public static bool InitGlobalTimers()
        {
           // ScheduleMgr.UpdateActivites();
            int dueTime = 86400000 - Convert.ToInt32((DateTime.Now - DateTime.Today).TotalMilliseconds);
            if (ScheduleMgr.m_UpdateSchedule == null)
                ScheduleMgr.m_UpdateSchedule = new System.Threading.Timer(new TimerCallback(ScheduleMgr.ScanSchedulProc), (object)null, dueTime, 86400000);
            else
                ScheduleMgr.m_UpdateSchedule.Change(dueTime, 86400000);
            return true;
        }

        protected static void ScanSchedulProc(object sender)
        {
            ThreadPriority priority = Thread.CurrentThread.Priority;
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                int tickCount = Environment.TickCount;
                if (ScheduleMgr.log.IsInfoEnabled)
                {
                    ScheduleMgr.log.Info((object)"Scan ScheduleMgr ...");
                    ScheduleMgr.log.Debug((object)("Scan ThreadId=" + Thread.CurrentThread.ManagedThreadId.ToString()));
                }
                int num = Environment.TickCount - tickCount;
                try
                {
                 //   ScheduleMgr.UpdateActivites();
                }
                catch (Exception ex)
                {
                    if (ScheduleMgr.log.IsErrorEnabled)
                        ScheduleMgr.log.Error((object)"UpdateActivites error!", ex);
                }
                if (!ScheduleMgr.log.IsInfoEnabled)
                    return;
                ScheduleMgr.log.Info((object)"Scan ScheduleMgr complete!");
            }
            catch (Exception ex)
            {
                if (!ScheduleMgr.log.IsErrorEnabled)
                    return;
                ScheduleMgr.log.Error((object)"Scan ScheduleMgr Proc", ex);
            }
            finally
            {
                Thread.CurrentThread.Priority = priority;
            }
        }

        public static bool Init()
        {
            bool flag = false;
            try
            {
                flag = true;
            }
            catch (Exception ex)
            {
                ScheduleMgr.log.Error((object)"ScheduleMgr Init", ex);
            }
            return flag;
        }     
        
    }
}
