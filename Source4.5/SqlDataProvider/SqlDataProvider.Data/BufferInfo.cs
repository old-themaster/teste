// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.BufferInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class BufferInfo : DataObject
  {
    private DateTime _beginDate;
    private string _data;
    private bool _isExist;
    private int _templateID;
    private int _type;
    private int _userID;
    private int _validCount;
    private int _validDate;
    private int _value;

    public DateTime GetEndDate() => this._beginDate.AddMinutes((double) this._validDate);

    public bool IsValid() => this._beginDate.AddMinutes((double) this._validDate) > DateTime.Now;

    public DateTime BeginDate
    {
      get => this._beginDate;
      set
      {
        this._beginDate = value;
        this._isDirty = true;
      }
    }

    public string Data
    {
      get => this._data;
      set
      {
        this._data = value;
        this._isDirty = true;
      }
    }

    public bool IsExist
    {
      get => this._isExist;
      set
      {
        this._isExist = value;
        this._isDirty = true;
      }
    }

    public int TemplateID
    {
      get => this._templateID;
      set
      {
        this._templateID = value;
        this._isDirty = true;
      }
    }

    public int Type
    {
      get => this._type;
      set
      {
        this._type = value;
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

    public int ValidCount
    {
      get => this._validCount;
      set
      {
        this._validCount = value;
        this._isDirty = true;
      }
    }

    public int ValidDate
    {
      get => this._validDate;
      set
      {
        this._validDate = value;
        this._isDirty = true;
      }
    }

    public int Value
    {
      get => this._value;
      set
      {
        this._value = value;
        this._isDirty = true;
      }
    }
  }
}
