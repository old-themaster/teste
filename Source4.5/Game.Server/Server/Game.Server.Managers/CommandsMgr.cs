// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.CommandsMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using System.Collections.Generic;

namespace Game.Server.Managers
{
  public class CommandsMgr
  {
    private static Dictionary<int, List<string>> Commands;

    public static bool CheckAdmin(int UserID, string Command) => CommandsMgr.Commands.ContainsKey(UserID);

    public static bool Init()
    {
      CommandsMgr.Commands = new PlayerBussiness().LoadCommands();
      return CommandsMgr.Commands != null && CommandsMgr.Commands.Count > 0;
    }
  }
}
