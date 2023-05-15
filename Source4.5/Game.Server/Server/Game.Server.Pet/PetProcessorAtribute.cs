// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.PetProcessorAtribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;

namespace Game.Server.Pet
{
  public class PetProcessorAtribute : Attribute
  {
    private byte byte_0;
    private string string_0;

    public PetProcessorAtribute(byte code, string description)
    {
      this.byte_0 = code;
      this.string_0 = description;
    }

    public byte Code => this.byte_0;

    public string Description => this.string_0;
  }
}
