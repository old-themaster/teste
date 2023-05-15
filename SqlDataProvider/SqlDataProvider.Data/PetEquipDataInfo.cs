// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PetEquipDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class PetEquipDataInfo : DataObject
  {
    private int _eqtemplateID;
    private int _eqType;
    private int _ID;
    private bool _isExit;
    private int _petID;
    private DateTime _startTime;
    private ItemTemplateInfo _template;
    private int _userID;
    private int _validDate;

    public PetEquipDataInfo(ItemTemplateInfo temp) => this._template = temp;

    public PetEquipDataInfo addTempalte(ItemTemplateInfo Template) => new PetEquipDataInfo(Template)
    {
      _ID = this._ID,
      _userID = this._userID,
      _petID = this._petID,
      _eqType = this._eqType,
      _eqtemplateID = this._eqtemplateID,
      _validDate = this._validDate,
      _startTime = this._startTime,
      _isExit = this._isExit
    };

    public bool IsValidate() => this._validDate == 0 || DateTime.Compare(this._startTime.AddDays((double) this._validDate), DateTime.Now) > 0;

    public int eqTemplateID
    {
      get => this._eqtemplateID;
      set
      {
        this._eqtemplateID = value;
        this._isDirty = true;
      }
    }

    public int eqType
    {
      get => this._eqType;
      set
      {
        this._eqType = value;
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
      get => this._isExit;
      set
      {
        this._isExit = value;
        this._isDirty = true;
      }
    }

    public int PetID
    {
      get => this._petID;
      set
      {
        this._petID = value;
        this._isDirty = true;
      }
    }

    public DateTime startTime
    {
      get => this._startTime;
      set
      {
        this._startTime = value;
        this._isDirty = true;
      }
    }

    public ItemTemplateInfo Template => this._template;

    public int UserID
    {
      get => this._userID;
      set
      {
        this._userID = value;
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
  }
}
