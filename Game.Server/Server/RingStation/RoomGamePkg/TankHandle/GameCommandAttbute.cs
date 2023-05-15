// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.GameCommandAttbute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  public class GameCommandAttbute : Attribute
  {
    public GameCommandAttbute(byte code) => this.Code = code;

    public byte Code { get; private set; }
  }
}
