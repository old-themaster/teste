// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.GameProcessorAttribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;

namespace Game.Server.RingStation.RoomGamePkg
{
  public class GameProcessorAttribute : Attribute
  {
    private byte _code;
    private string _descript;

    public GameProcessorAttribute(byte code, string description)
    {
      this._code = code;
      this._descript = description;
    }

    public byte Code => this._code;

    public string Description => this._descript;
  }
}
