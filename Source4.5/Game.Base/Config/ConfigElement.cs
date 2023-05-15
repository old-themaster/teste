// Decompiled with JetBrains decompiler
// Type: Game.Base.Config.ConfigElement
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

using System.Collections;

namespace Game.Base.Config
{
  public class ConfigElement
  {
    protected Hashtable m_children = new Hashtable();
    protected ConfigElement m_parent;
    protected string m_value;

    public ConfigElement(ConfigElement parent) => this.m_parent = parent;

    public bool GetBoolean() => bool.Parse(this.m_value);

    public bool GetBoolean(bool defaultValue) => this.m_value == null ? defaultValue : bool.Parse(this.m_value);

    public int GetInt() => int.Parse(this.m_value);

    public int GetInt(int defaultValue) => this.m_value == null ? defaultValue : int.Parse(this.m_value);

    public long GetLong() => long.Parse(this.m_value);

    public long GetLong(long defaultValue) => this.m_value == null ? defaultValue : long.Parse(this.m_value);

    protected virtual ConfigElement GetNewConfigElement(ConfigElement parent) => new ConfigElement(parent);

    public string GetString() => this.m_value;

    public string GetString(string defaultValue) => this.m_value == null ? defaultValue : this.m_value;

    public void Set(object value) => this.m_value = value.ToString();

    public Hashtable Children => this.m_children;

    public bool HasChildren => this.m_children.Count > 0;

    public ConfigElement this[string key]
    {
      get
      {
        lock (this.m_children)
        {
          if (!this.m_children.Contains((object) key))
            this.m_children.Add((object) key, (object) this.GetNewConfigElement(this));
        }
        return (ConfigElement) this.m_children[(object) key];
      }
      set
      {
        lock (this.m_children)
          this.m_children[(object) key] = (object) value;
      }
    }

    public ConfigElement Parent => this.m_parent;
  }
}
