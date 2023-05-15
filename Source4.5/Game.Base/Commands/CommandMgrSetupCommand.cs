// Decompiled with JetBrains decompiler
// Type: Game.Base.Commands.CommandMgrSetupCommand
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

namespace Game.Base.Commands
{
  [Cmd("&cmd", ePrivLevel.Admin, "Config the command system.", new string[] {"/cmd [option] <para1> <para2>      ", "eg: /cmd -reload           :Reload the command system.", "    /cmd -list             :Display all commands."})]
  public class CommandMgrSetupCommand : AbstractCommandHandler, ICommandHandler
  {
    public bool OnCommand(BaseClient client, string[] args)
    {
      if (args.Length > 1)
      {
        string str = args[1];
        if (!(str == "-reload"))
        {
          if (str == "-list")
            CommandMgr.DisplaySyntax(client);
          else
            this.DisplaySyntax(client);
        }
        else
          CommandMgr.LoadCommands();
      }
      else
        this.DisplaySyntax(client);
      return true;
    }
  }
}
