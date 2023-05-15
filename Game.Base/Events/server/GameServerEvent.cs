// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.GameServerEvent
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

namespace Game.Base.Events
{
  public class GameServerEvent : RoadEvent
  {
    public static readonly GameServerEvent Started = new GameServerEvent("Server.Started");
    public static readonly GameServerEvent Stopped = new GameServerEvent("Server.Stopped");
    public static readonly GameServerEvent WorldSave = new GameServerEvent("Server.WorldSave");

    protected GameServerEvent(string name)
      : base(name)
    {
    }
  }
}
