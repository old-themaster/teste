// Decompiled with JetBrains decompiler
// Type: YbxMgr.FairMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using EntityDatabase.ServerModels;
using System.Collections.Generic;
using System.Linq;

namespace YbxMgr
{
  public static class FairMgr
  {
    public static List<FairBattleSkillGetTemplates> fairBattleSkillGetTemplates;
    public static List<FairSkills> fairSkills;
    public static List<Y_MountSkillTemplate> Y_MountSkillTemplates;

    public static bool Init()
    {
      try
      {
        ServerData serverData = new ServerData();
        FairMgr.fairBattleSkillGetTemplates = serverData.FairBattleSkillGetTemplates.ToList<FairBattleSkillGetTemplates>();
        FairMgr.fairSkills = serverData.FairSkills.ToList<FairSkills>();
        FairMgr.Y_MountSkillTemplates = serverData.Y_MountSkillTemplate.ToList<Y_MountSkillTemplate>();
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
