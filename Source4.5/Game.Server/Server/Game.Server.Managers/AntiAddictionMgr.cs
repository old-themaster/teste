// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.AntiAddictionMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using System;

namespace Game.Server.Managers
{
    internal class AntiAddictionMgr
  {
    private static bool _isASSon;
    public static int count;

    public static int AASStateGet(GamePlayer player)
    {
      int id = player.PlayerCharacter.ID;
      bool result = true;
      player.IsAASInfo = false;
      player.IsMinor = true;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        string assInfoSingle = playerBussiness.GetASSInfoSingle(id);
        if (assInfoSingle != "")
        {
          player.IsAASInfo = true;
          result = false;
          int int32_1 = Convert.ToInt32(assInfoSingle.Substring(6, 4));
          int int32_2 = Convert.ToInt32(assInfoSingle.Substring(10, 2));
          if (DateTime.Now.Year.CompareTo(int32_1 + 18) <= 0)
          {
            int num = DateTime.Now.Year;
            if (num.CompareTo(int32_1 + 18) == 0)
            {
              num = DateTime.Now.Month;
              if (num.CompareTo(int32_2) < 0)
                goto label_9;
            }
            else
              goto label_9;
          }
          player.IsMinor = false;
        }
      }
label_9:
      if (result && player.PlayerCharacter.IsFirst != 0 && (player.PlayerCharacter.DayLoginCount < 1 && AntiAddictionMgr.ISASSon))
        player.Out.SendAASState(result);
      if (player.IsMinor || !player.IsAASInfo && AntiAddictionMgr.ISASSon)
        player.Out.SendAASControl(AntiAddictionMgr.ISASSon, player.IsAASInfo, player.IsMinor);
      return 0;
    }

    public static double GetAntiAddictionCoefficient(int onlineTime)
    {
      if (!AntiAddictionMgr._isASSon || 0 <= onlineTime && onlineTime <= 240)
        return 1.0;
      return 240 < onlineTime && onlineTime <= 300 ? 0.5 : 0.0;
    }

    public static void SetASSState(bool ASSState) => AntiAddictionMgr._isASSon = ASSState;

    public static bool ISASSon => AntiAddictionMgr._isASSon;
  }
}
