// Decompiled with JetBrains decompiler
// Type: Game.Base.Config.ConfigPropertyAttribute
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

using System;

namespace Game.Base.Config
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ConfigPropertyAttribute : Attribute
  {
    private object m_defaultValue;
    private string m_description;
    private string m_key;

    public ConfigPropertyAttribute(string key, string description, object defaultValue)
    {
      this.m_key = key;
      this.m_description = description;
      this.m_defaultValue = defaultValue;
    }

    public object DefaultValue => this.m_defaultValue;

    public string Description => this.m_description;

    public string Key => this.m_key;
  }
}
