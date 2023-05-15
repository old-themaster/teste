using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
	public class BallConfigMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static Dictionary<int, BallConfigInfo> m_infos;

		public static BallConfigInfo FindBall(int id)
		{
			if (m_infos.ContainsKey(id))
			{
				return m_infos[id];
			}
			return null;
		}

		public static bool Init()
		{
			return ReLoad();
		}

		private static Dictionary<int, BallConfigInfo> LoadFromDatabase()
		{
			Dictionary<int, BallConfigInfo> dictionary = new Dictionary<int, BallConfigInfo>();
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				BallConfigInfo[] allBallConfig = produceBussiness.GetAllBallConfig();
				foreach (BallConfigInfo ballConfigInfo in allBallConfig)
				{
					if (!dictionary.ContainsKey(ballConfigInfo.TemplateID))
					{
						dictionary.Add(ballConfigInfo.TemplateID, ballConfigInfo);
					}
				}
				return dictionary;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, BallConfigInfo> dictionary = LoadFromDatabase();
				if (dictionary.Values.Count > 0)
				{
					Interlocked.Exchange(ref m_infos, dictionary);
					return true;
				}
			}
			catch (Exception exception)
			{
				log.Error("Ball Mgr init error:", exception);
			}
			return false;
		}
	}
}
