using Game.Base.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Logic.Cmd
{
	public class CommandMgr
	{
		private static Dictionary<int, ICommandHandler> handles = new Dictionary<int, ICommandHandler>();

		public static ICommandHandler LoadCommandHandler(int code)
		{
			return handles[code];
		}

		[ScriptLoadedEvent]
		public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
		{
			handles.Clear();
			SearchCommandHandlers(Assembly.GetAssembly(typeof(BaseGame)));
		}

		protected static void RegisterCommandHandler(int code, ICommandHandler handle)
		{
			handles.Add(code, handle);
		}

		protected static int SearchCommandHandlers(Assembly assembly)
		{
			int num = 0;
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsClass && type.GetInterface("Game.Logic.Cmd.ICommandHandler") != null)
				{
					GameCommandAttribute[] array = (GameCommandAttribute[])type.GetCustomAttributes(typeof(GameCommandAttribute), inherit: true);
					if (array.Length != 0)
					{
						num++;
						RegisterCommandHandler(array[0].Code, Activator.CreateInstance(type) as ICommandHandler);
					}
				}
			}
			return num;
		}
	}
}
