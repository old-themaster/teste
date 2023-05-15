// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.ScriptEvent
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

namespace Game.Base.Events
{
  public class ScriptEvent : RoadEvent
  {
    public static readonly ScriptEvent Loaded = new ScriptEvent("Script.Loaded");
    public static readonly ScriptEvent Unloaded = new ScriptEvent("Script.Unloaded");

    protected ScriptEvent(string name)
      : base(name)
    {
    }
  }
}
