// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.FusionCombined
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using SqlDataProvider.Data;

namespace Game.Server.Managers
{
  public class FusionCombined
  {
    public static Items_Fusion_List_Info[] m_itemsfusionlist;
    public static FusionCombinedInfo[] m_listFusionCombined;

    public static FusionCombinedInfo[] ListCombinedFusion()
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
        FusionCombined.m_itemsfusionlist = produceBussiness.GetAllFusionList();
      return FusionCombined.m_listFusionCombined;
    }
  }
}
