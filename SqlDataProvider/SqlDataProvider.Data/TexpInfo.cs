// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.TexpInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class TexpInfo : DataObject
  {
    private int _attTexpExp;
    private int _defTexpExp;
    private int _hpTexpExp;
    private int _lukTexpExp;
    private int _spdTexpExp;
    private int _texpCount;
    private int _texpTaskCount;
    private DateTime _texpTaskDate;
    private int _userID;

    public bool IsValidadteTexp() => this._texpTaskDate.Date < DateTime.Now.Date;

    public int attTexpExp
    {
      get => this._attTexpExp;
      set
      {
        this._attTexpExp = value;
        this._isDirty = true;
      }
    }

    public int defTexpExp
    {
      get => this._defTexpExp;
      set
      {
        this._defTexpExp = value;
        this._isDirty = true;
      }
    }

    public int hpTexpExp
    {
      get => this._hpTexpExp;
      set
      {
        this._hpTexpExp = value;
        this._isDirty = true;
      }
    }

    public int ID { get; set; }

    public int lukTexpExp
    {
      get => this._lukTexpExp;
      set
      {
        this._lukTexpExp = value;
        this._isDirty = true;
      }
    }

    public int spdTexpExp
    {
      get => this._spdTexpExp;
      set
      {
        this._spdTexpExp = value;
        this._isDirty = true;
      }
    }

    public int texpCount
    {
      get => this._texpCount;
      set
      {
        this._texpCount = value;
        this._isDirty = true;
      }
    }

    public int texpTaskCount
    {
      get => this._texpTaskCount;
      set
      {
        this._texpTaskCount = value;
        this._isDirty = true;
      }
    }

    public DateTime texpTaskDate
    {
      get => this._texpTaskDate;
      set
      {
        this._texpTaskDate = value;
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
