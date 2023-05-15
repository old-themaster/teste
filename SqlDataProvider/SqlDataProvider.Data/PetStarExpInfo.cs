// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PetStarExpInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class PetStarExpInfo
  {
    private int _Exp;
    private int _NewID;
    private int _OldID;

    public int Exp
    {
      get => this._Exp;
      set => this._Exp = value;
    }

    public int NewID
    {
      get => this._NewID;
      set => this._NewID = value;
    }

    public int OldID
    {
      get => this._OldID;
      set => this._OldID = value;
    }
  }
}
