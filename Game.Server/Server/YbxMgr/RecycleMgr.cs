// Decompiled with JetBrains decompiler
// Type: YbxMgr.RecycleMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using EntityDatabase.ServerModels;
using System.Collections.Generic;
using System.Linq;

namespace YbxMgr
{
  public static class RecycleMgr
  {
    public static List<Recycleactivityinfoes> RecycleActivityinfos;
    public static List<EntityDatabase.ServerModels.BombTurnTableGoods> BombTurnTableGoods;

    public static bool Init()
    {
      try
      {
        ServerData serverData = new ServerData();
        RecycleMgr.RecycleActivityinfos = serverData.Recycleactivityinfoes.ToList<Recycleactivityinfoes>();
        RecycleMgr.BombTurnTableGoods = serverData.BombTurnTableGoods.ToList<EntityDatabase.ServerModels.BombTurnTableGoods>();
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
