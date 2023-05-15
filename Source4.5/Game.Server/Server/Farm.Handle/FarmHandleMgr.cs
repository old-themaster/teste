using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Farm.Handle
{
    public class FarmHandleMgr
	{
		private Dictionary<int, IFarmCommandHadler> dictionary_0;

		private readonly static ILog ilog_0;

		static FarmHandleMgr()
		{
			
			FarmHandleMgr.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public FarmHandleMgr()
		{
			
			this.dictionary_0 = new Dictionary<int, IFarmCommandHadler>();
			
			this.dictionary_0.Clear();
			this.SearchCommandHandlers(Assembly.GetAssembly(typeof(GameServer)));
		}

		public IFarmCommandHadler LoadCommandHandler(int code)
		{
			if (this.dictionary_0.ContainsKey(code))
			{
				return this.dictionary_0[code];
			}
			FarmHandleMgr.ilog_0.Error(string.Concat("LoadCommandHandler code001: ", code.ToString()));
			return null;
		}

		protected void RegisterCommandHandler(int code, IFarmCommandHadler handle)
		{
			this.dictionary_0.Add(code, handle);
		}

		protected int SearchCommandHandlers(Assembly assembly)
		{
			int num = 0;
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < (int)types.Length; i++)
			{
				Type type = types[i];
				if (type.IsClass && !(type.GetInterface("Game.Server.Farm.Handle.IFarmCommandHadler") == null))
				{
					Farm5[] customAttributes = (Farm5[])type.GetCustomAttributes(typeof(Farm5), true);
					if (customAttributes.Length != 0)
					{
						num++;
						this.RegisterCommandHandler((int)customAttributes[0].method_0(), Activator.CreateInstance(type) as IFarmCommandHadler);
					}
				}
			}
			return num;
		}
	}
}