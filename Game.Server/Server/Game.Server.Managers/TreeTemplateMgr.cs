using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
    public class TreeTemplateMgr
    {
        private static Dictionary<int, TreeTemplateInfo> dictionary_0;
        private static readonly ILog ilog_0;
        private static ReaderWriterLock readerWriterLock_0;
        private static ThreadSafeRandom threadSafeRandom_0;

        public static TreeTemplateInfo GetTreeTemplateWithLevel(int level)
        {
            lock (TreeTemplateMgr.readerWriterLock_0)
                return TreeTemplateMgr.dictionary_0.ContainsKey(level) ? TreeTemplateMgr.dictionary_0[level] : (TreeTemplateInfo)null;
        }

        public static bool Init()
        {
            try
            {
                TreeTemplateMgr.readerWriterLock_0 = new ReaderWriterLock();
                TreeTemplateMgr.dictionary_0 = new Dictionary<int, TreeTemplateInfo>();
                TreeTemplateMgr.threadSafeRandom_0 = new ThreadSafeRandom();
                return TreeTemplateMgr.smethod_0(TreeTemplateMgr.dictionary_0);
            }
            catch (Exception ex)
            {
                if (TreeTemplateMgr.ilog_0.IsErrorEnabled)
                    TreeTemplateMgr.ilog_0.Error((object)nameof(TreeTemplateMgr), ex);
                return false;
            }
        }

        private static bool smethod_0(Dictionary<int, TreeTemplateInfo> dictionary_1)
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
            {
                foreach (TreeTemplateInfo treeTemplateInfo in produceBussiness.GetAllTreeTemplate())
                {
                    if (!dictionary_1.ContainsKey(treeTemplateInfo.Level))
                        dictionary_1.Add(treeTemplateInfo.Level, treeTemplateInfo);
                }
            }
            return true;
        }

        public static bool ReLoad()
        {
            try
            {
                Dictionary<int, TreeTemplateInfo> dictionary_1 = new Dictionary<int, TreeTemplateInfo>();
                if (TreeTemplateMgr.smethod_0(dictionary_1))
                {
                    TreeTemplateMgr.readerWriterLock_0.AcquireWriterLock(-1);
                    try
                    {
                        TreeTemplateMgr.dictionary_0 = dictionary_1;
                        return true;
                    }
                    catch
                    {
                    }
                    finally
                    {
                        TreeTemplateMgr.readerWriterLock_0.ReleaseWriterLock();
                    }
                }
            }
            catch (Exception ex)
            {
                if (TreeTemplateMgr.ilog_0.IsErrorEnabled)
                    TreeTemplateMgr.ilog_0.Error((object)nameof(TreeTemplateMgr), ex);
            }
            return false;
        }

        static TreeTemplateMgr()
        {
            LicenseManager.Validate(typeof(TreeTemplateMgr));
            TreeTemplateMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
