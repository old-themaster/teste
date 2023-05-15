// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserDrillInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class UserDrillInfo : DataObject
  {
    private int _beadPlace;
    private int _drillPlace;
    private int _holeExp;
    private int _holeLv;
    private int _userID;

    public int BeadPlace
    {
      get => this._beadPlace;
      set
      {
        this._beadPlace = value;
        this._isDirty = true;
      }
    }

    public int DrillPlace
    {
      get => this._drillPlace;
      set
      {
        this._drillPlace = value;
        this._isDirty = true;
      }
    }

    public int HoleExp
    {
      get => this._holeExp;
      set
      {
        this._holeExp = value;
        this._isDirty = true;
      }
    }

    public int HoleLv
    {
      get => this._holeLv;
      set
      {
        this._holeLv = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get => this._userID;
      set
      {
        this._userID = value;
        this._isDirty = true;
      }
    }
  }
}
