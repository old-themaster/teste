// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PacketHandlerAttribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;

namespace Game.Server.Packets.Client
{
  [AttributeUsage(AttributeTargets.Class)]
  public class PacketHandlerAttribute : Attribute
  {
    protected int m_code;
    protected string m_desc;

    public PacketHandlerAttribute(int code, string desc)
    {
      this.m_code = code;
      this.m_desc = desc;
    }

    public int Code => this.m_code;

    public string Description => this.m_desc;
  }
}
