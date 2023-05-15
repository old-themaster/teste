// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.PetHandleMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Pet.Handle
{
  public class PetHandleMgr
  {
    private Dictionary<int, IPetCommandHadler> dictionary_0;
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public IPetCommandHadler LoadCommandHandler(int code)
    {
      if (this.dictionary_0.ContainsKey(code))
        return this.dictionary_0[code];
      PetHandleMgr.ilog_0.Error((object) ("LoadCommandHandler code024: " + code.ToString()));
      return (IPetCommandHadler) null;
    }

    public PetHandleMgr()
    {
      this.dictionary_0 = new Dictionary<int, IPetCommandHadler>();
      this.dictionary_0.Clear();
      this.SearchCommandHandlers(Assembly.GetAssembly(typeof (GameServer)));
    }

    protected int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && !(type.GetInterface("Game.Server.Pet.Handle.IPetCommandHadler") == (Type) null))
        {
          global::Pet[] customAttributes = (global::Pet[]) type.GetCustomAttributes(typeof (global::Pet), true);
          if (customAttributes.Length != 0)
          {
            ++num;
            this.RegisterCommandHandler((int) customAttributes[0].method_0(), Activator.CreateInstance(type) as IPetCommandHadler);
          }
        }
      }
      return num;
    }

    protected void RegisterCommandHandler(int code, IPetCommandHadler handle) => this.dictionary_0.Add(code, handle);
  }
}
