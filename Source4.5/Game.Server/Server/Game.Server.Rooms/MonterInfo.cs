// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.MonterInfo
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System.Drawing;

namespace Game.Server.Rooms
{
  public class MonterInfo
  {
    public int ID { get; set; }

    public Point MonsterNewPos { get; set; }

    public Point MonsterPos { get; set; }

    public int PlayerID { get; set; }

    public int state { get; set; }

    public int type { get; set; }
  }
}
