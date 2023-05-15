using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.GypsyShop.Handle
{
    public class GypsyShopHandleMgr
    {
        private Dictionary<int, IGypsyShopCommandHadler> handles = new Dictionary<int, IGypsyShopCommandHadler>();

        public IGypsyShopCommandHadler LoadCommandHandler(int code)
        {
            if (handles.ContainsKey(code))
                return handles[code];
            log.Error("LoadCommandHandler code010: " + code.ToString());
            return null;
        }

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GypsyShopHandleMgr()
        {
            handles.Clear();
            SearchCommandHandlers(Assembly.GetAssembly(typeof(GameServer)));
        }

        protected int SearchCommandHandlers(Assembly assembly)
        {
            int count = 0;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass != true)
                    continue;
                if (type.GetInterface("Game.Server.GypsyShop.Handle.IGypsyShopCommandHadler") == null)
                    continue;

                GypsyShopHandleAttbute[] attr =
                    (GypsyShopHandleAttbute[]) type.GetCustomAttributes(typeof(GypsyShopHandleAttbute), true);
                if (attr.Length > 0)
                {
                    count++;
                    RegisterCommandHandler(attr[0].Code, Activator.CreateInstance(type) as IGypsyShopCommandHadler);
                }
            }

            return count;
        }

        protected void RegisterCommandHandler(int code, IGypsyShopCommandHadler handle)
        {
            handles.Add(code, handle);
        }
    }
}