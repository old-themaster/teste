// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.SevenItem
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class SevenItem
  {
    private int _Index;
    private int _PosX;
    private int _Tag;
    private int _Type;

    public SevenItem()
    {
    }

    public SevenItem(int index, int type, int posx, int tag)
    {
      this._Index = index;
      this._Type = type;
      this._PosX = posx;
      this._Tag = tag;
    }

    public int Index
    {
      get => this._Index;
      set => this._Index = value;
    }

    public int PosX
    {
      get => this._PosX;
      set => this._PosX = value;
    }

    public int Tag
    {
      get => this._Tag;
      set => this._Tag = value;
    }

    public int Type
    {
      get => this._Type;
      set => this._Type = value;
    }
  }
}
