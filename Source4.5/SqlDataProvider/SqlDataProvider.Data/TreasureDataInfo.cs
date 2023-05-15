// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.TreasureDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class TreasureDataInfo : DataObject
  {
    private DateTime _BeginDate;
    private int _Count;
    private int _ID;
    private bool _IsExit;
    private int _pos;
    private int _TemplateID;
    private int _UserID;
    private int _ValidDate;

    public DateTime BeginDate
    {
      get => this._BeginDate;
      set
      {
        this._BeginDate = value;
        this._isDirty = true;
      }
    }

    public int Count
    {
      get => this._Count;
      set
      {
        this._Count = value;
        this._isDirty = true;
      }
    }

    public int ID
    {
      get => this._ID;
      set
      {
        this._ID = value;
        this._isDirty = true;
      }
    }

    public bool IsExit
    {
      get => this._IsExit;
      set
      {
        this._IsExit = value;
        this._isDirty = true;
      }
    }

    public int pos
    {
      get => this._pos;
      set
      {
        this._pos = value;
        this._isDirty = true;
      }
    }

    public int TemplateID
    {
      get => this._TemplateID;
      set
      {
        this._TemplateID = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get => this._UserID;
      set
      {
        this._UserID = value;
        this._isDirty = true;
      }
    }

    public int ValidDate
    {
      get => this._ValidDate;
      set
      {
        this._ValidDate = value;
        this._isDirty = true;
      }
    }
  }
}
