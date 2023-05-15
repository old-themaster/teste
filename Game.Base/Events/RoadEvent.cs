// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.RoadEvent
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

namespace Game.Base.Events
{
  public abstract class RoadEvent
  {
    protected string m_EventName;

    public RoadEvent(string name) => this.m_EventName = name;

    public virtual bool IsValidFor(object o) => true;

    public override string ToString() => "DOLEvent(" + this.m_EventName + ")";

    public string Name => this.m_EventName;
  }
}
