// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.MarryCommandAttbute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  public class MarryCommandAttbute : Attribute
  {
    public MarryCommandAttbute(byte code) => this.Code = code;

    public byte Code { get; private set; }
  }
}
