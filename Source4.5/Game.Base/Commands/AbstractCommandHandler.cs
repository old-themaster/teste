// Decompiled with JetBrains decompiler
// Type: Game.Base.AbstractCommandHandler
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

namespace Game.Base
{
  public abstract class AbstractCommandHandler
  {
    public virtual void DisplayMessage(BaseClient client, string format, params object[] args) => this.DisplayMessage(client, string.Format(format, args));

    public virtual void DisplayMessage(BaseClient client, string message) => client?.DisplayMessage(message);

    public virtual void DisplaySyntax(BaseClient client)
    {
      if (client == null)
        return;
      CmdAttribute[] customAttributes = (CmdAttribute[]) this.GetType().GetCustomAttributes(typeof (CmdAttribute), false);
      if (customAttributes.Length == 0)
        return;
      client.DisplayMessage(customAttributes[0].Description);
      foreach (string msg in customAttributes[0].Usage)
        client.DisplayMessage(msg);
    }
  }
}
