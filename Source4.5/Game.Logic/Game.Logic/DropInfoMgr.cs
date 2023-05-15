using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
	public class DropInfoMgr
	{
		public static Dictionary<int, MacroDropInfo> DropInfo;

		protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected static ReaderWriterLock m_lock = new ReaderWriterLock();

		public static bool CanDrop(int templateId)
		{
			if (DropInfo != null)
			{
				m_lock.AcquireWriterLock(-1);
				try
				{
					if (DropInfo.ContainsKey(templateId))
					{
						MacroDropInfo macroDropInfo = DropInfo[templateId];
						if (macroDropInfo.DropCount < macroDropInfo.MaxDropCount || macroDropInfo.SelfDropCount >= macroDropInfo.DropCount)
						{
							macroDropInfo.SelfDropCount++;
							macroDropInfo.DropCount++;
							return true;
						}
						return false;
					}
				}
				catch (Exception exception)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("DropInfoMgr CanDrop", exception);
					}
				}
				finally
				{
					m_lock.ReleaseWriterLock();
				}
			}
			return true;
		}
	}
}
