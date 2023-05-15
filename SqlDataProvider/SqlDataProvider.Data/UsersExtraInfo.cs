// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UsersExtraInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UsersExtraInfo : DataObject
  {
    private int int_0;
    private DateTime dateTime_0;
    private DateTime dateTime_1;
    private int int_1;
    private DateTime dateTime_2;
    private int int_2;
    private int int_3;
    private int int_4;
    private int int_5;
    private DateTime dateTime_3;
    private int int_6;
    private int int_7;
    private int int_8;
    private bool bool_0;
    private bool bool_1;
    private int int_9;
    private int int_10;
    private int int_11;
    private int int_12;
    private bool bool_2;
    private int int_13;
    private int int_14;
    private int int_15;
    private int int_16;
    private int int_17;
    private int int_18;
    private DateTime dateTime_4;
    private int int_19;
    private int int_20;
    private float float_0;
    private bool[] _GodsRoadPeriodAward;

        private int _freeSendMailCount;

        public int FreeSendMailCount
        {
            get { return _freeSendMailCount; }
            set
            {
                _freeSendMailCount = value;
                _isDirty = true;
            }
        }

        public int UserID
    {
      get => this.int_0;
      set
      {
        this.int_0 = value;
        this._isDirty = true;
      }
    }

    public string NickName { get; set; }

    public DateTime LastTimeHotSpring
    {
      get => this.dateTime_0;
      set
      {
        this.dateTime_0 = value;
        this._isDirty = true;
      }
    }

    public DateTime LastFreeTimeHotSpring
    {
      get => this.dateTime_1;
      set
      {
        this.dateTime_1 = value;
        this._isDirty = true;
      }
    }

    public int MinHotSpring
    {
      get => this.int_1;
      set
      {
        this.int_1 = value;
        this._isDirty = true;
      }
    }

    public int coupleBossEnterNum
    {
      get => this.int_2;
      set
      {
        this.int_2 = value;
        this._isDirty = true;
      }
    }

    public int coupleBossHurt
    {
      get => this.int_3;
      set
      {
        this.int_3 = value;
        this._isDirty = true;
      }
    }

    public int coupleBossBoxNum
    {
      get => this.int_4;
      set
      {
        this.int_4 = value;
        this._isDirty = true;
      }
    }

    public int TotalCaddyOpen
    {
      get => this.int_5;
      set
      {
        this.int_5 = value;
        this._isDirty = true;
      }
    }

    public bool isGetAwardMarry
    {
      get => this.bool_0;
      set
      {
        this.bool_0 = value;
        this._isDirty = true;
      }
    }

    public bool isFirstAwardMarry
    {
      get => this.bool_1;
      set
      {
        this.bool_1 = value;
        this._isDirty = true;
      }
    }

    public int LeftRoutteCount
    {
      get => this.int_20;
      set
      {
        this.int_20 = value;
        this._isDirty = true;
      }
    }

    public float LeftRoutteRate
    {
      get => this.float_0;
      set
      {
        this.float_0 = value;
        this._isDirty = true;
      }
    }
        public bool[] GodsRoadPeriodAward
        {
            get
            {
                return _GodsRoadPeriodAward;
            }
            set
            {
                _GodsRoadPeriodAward = value;
                base.IsDirty = true;
            }
        }
        public bool IsVaildFreeHotSpring() => this.dateTime_1.Date < DateTime.Now.Date;
  }
}
