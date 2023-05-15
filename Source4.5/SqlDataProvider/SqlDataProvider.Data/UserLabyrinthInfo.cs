// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserLabyrinthInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserLabyrinthInfo : DataObject
  {
    private int int_0;
    private int int_1;
    private int int_2;
    private int int_3;
    private bool bool_0;
    private bool bool_1;
    private int int_4;
    private int int_5;
    private int int_6;
    private int int_7;
    private int int_8;
    private int int_9;
    private bool bool_2;
    private bool bool_3;
    private bool bool_4;
    private bool bool_5;
    private DateTime dateTime_0;
    private string string_0;

    public int UserID
    {
      get => this.int_0;
      set
      {
        this.int_0 = value;
        this._isDirty = true;
      }
    }

    public int sType
    {
      get => this.int_1;
      set
      {
        this.int_1 = value;
        this._isDirty = true;
      }
    }

    public int myProgress
    {
      get => this.int_2;
      set
      {
        this.int_2 = value;
        this._isDirty = true;
      }
    }

    public int myRanking
    {
      get => this.int_3;
      set
      {
        this.int_3 = value;
        this._isDirty = true;
      }
    }

    public bool completeChallenge
    {
      get => this.bool_0;
      set
      {
        this.bool_0 = value;
        this._isDirty = true;
      }
    }

    public bool isDoubleAward
    {
      get => this.bool_1;
      set
      {
        this.bool_1 = value;
        this._isDirty = true;
      }
    }

    public int currentFloor
    {
      get => this.int_4;
      set
      {
        this.int_4 = value;
        this._isDirty = true;
      }
    }

    public int accumulateExp
    {
      get => this.int_5;
      set
      {
        this.int_5 = value;
        this._isDirty = true;
      }
    }

    public int remainTime
    {
      get => this.int_6;
      set
      {
        this.int_6 = value;
        this._isDirty = true;
      }
    }

    public int currentRemainTime
    {
      get => this.int_7;
      set
      {
        this.int_7 = value;
        this._isDirty = true;
      }
    }

    public int cleanOutAllTime
    {
      get => this.int_8;
      set
      {
        this.int_8 = value;
        this._isDirty = true;
      }
    }

    public int cleanOutGold
    {
      get => this.int_9;
      set
      {
        this.int_9 = value;
        this._isDirty = true;
      }
    }

    public bool tryAgainComplete
    {
      get => this.bool_2;
      set
      {
        this.bool_2 = value;
        this._isDirty = true;
      }
    }

    public bool isInGame
    {
      get => this.bool_3;
      set
      {
        this.bool_3 = value;
        this._isDirty = true;
      }
    }

    public bool isCleanOut
    {
      get => this.bool_4;
      set
      {
        this.bool_4 = value;
        this._isDirty = true;
      }
    }

    public bool serverMultiplyingPower
    {
      get => this.bool_5;
      set
      {
        this.bool_5 = value;
        this._isDirty = true;
      }
    }

    public DateTime LastDate
    {
      get => this.dateTime_0;
      set
      {
        this.dateTime_0 = value;
        this._isDirty = true;
      }
    }

    public string ProcessAward
    {
      get => this.string_0;
      set
      {
        this.string_0 = value;
        this._isDirty = true;
      }
    }

    public bool isValidDate() => this.dateTime_0.Date < DateTime.Now.Date;
  }
}
