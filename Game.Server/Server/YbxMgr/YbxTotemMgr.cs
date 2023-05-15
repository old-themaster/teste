// Decompiled with JetBrains decompiler
// Type: YbxMgr.YbxTotemMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using EntityDatabase.ServerModels;
using System.Collections.Generic;
using System.Linq;

namespace YbxMgr
{
  public static class YbxTotemMgr
  {
    public static List<Ts_Upgradetemplate> ts_Upgradetemplates;

    public static bool Init()
    {
      ServerData serverData = new ServerData();
      try
      {
        YbxTotemMgr.ts_Upgradetemplates = serverData.Ts_Upgradetemplate.ToList<Ts_Upgradetemplate>();
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
