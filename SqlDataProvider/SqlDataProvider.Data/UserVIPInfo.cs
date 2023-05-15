// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserVIPInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserVIPInfo : DataObject
  {
    private bool _CanTakeVipReward;
    private DateTime _LastVIPPackTime;
    private byte _typeVIP;
    private int _UserID;
    private int _VIPExp;
    private DateTime _VIPExpireDay;
    private DateTime _VIPLastdate;
    private int _VIPLevel;
    private int _VIPNextLevelDaysNeeded;
    private int _VIPOfflineDays;
    private int _VIPOnlineDays;

    public UserVIPInfo()
    {
    }

    public UserVIPInfo(int userId)
    {
      this.UserID = userId;
      this.typeVIP = (byte) 0;
      this.VIPLevel = 0;
      this.VIPExp = 0;
      this.VIPOnlineDays = 0;
      this.VIPOfflineDays = 0;
      this.VIPExpireDay = DateTime.Now;
      this.LastVIPPackTime = DateTime.Now;
      this.VIPLastdate = DateTime.Now;
      this.VIPNextLevelDaysNeeded = 0;
      this.CanTakeVipReward = false;
    }

    public void ContinousVIP(int days)
    {
      DateTime now = DateTime.Now;
      this.VIPExpireDay = !(this.VIPExpireDay < DateTime.Now) ? this.VIPExpireDay.AddDays((double) days) : DateTime.Now.AddDays((double) days);
      this.typeVIP = this.SetType(days);
    }

    public bool IsLastVIPPackTime() => this.StartOfWeek(this._LastVIPPackTime.Date, DayOfWeek.Monday) < this.StartOfWeek(DateTime.Now.Date, DayOfWeek.Monday);

    public bool IsVIP() => !this.IsVIPExpire() && this._typeVIP > (byte) 0;

    public bool IsVIPExpire() => this._VIPExpireDay.Date < DateTime.Now.Date;

    public void OpenVIP(int days)
    {
      DateTime dateTime = DateTime.Now.AddDays((double) days);
      this.typeVIP = this.SetType(days);
      this.VIPLevel = 1;
      this.VIPExp = 0;
      this.VIPExpireDay = dateTime;
      this.VIPLastdate = DateTime.Now;
      this.VIPNextLevelDaysNeeded = 0;
      this.CanTakeVipReward = true;
    }

    public void SetLastVIPPackTime()
    {
      this.LastVIPPackTime = DateTime.Now;
      this.CanTakeVipReward = false;
    }

    private byte SetType(int days)
    {
      byte num = 1;
      if (days / 31 >= 3)
        num = (byte) 2;
      return num;
    }

    public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
    {
      int num = dt.DayOfWeek - startOfWeek;
      if (num < 0)
        num += 7;
      return dt.AddDays((double) (-1 * num)).Date;
    }

    public bool CanTakeVipReward
    {
      get => this._CanTakeVipReward;
      set
      {
        this._CanTakeVipReward = value;
        this._isDirty = true;
      }
    }

    public DateTime LastVIPPackTime
    {
      get => this._LastVIPPackTime;
      set
      {
        this._LastVIPPackTime = value;
        this._isDirty = true;
      }
    }

    public byte typeVIP
    {
      get => this._typeVIP;
      set
      {
        this._typeVIP = value;
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

    public int VIPExp
    {
      get => this._VIPExp;
      set
      {
        this._VIPExp = value;
        this._isDirty = true;
      }
    }

    public DateTime VIPExpireDay
    {
      get => this._VIPExpireDay;
      set
      {
        this._VIPExpireDay = value;
        this._isDirty = true;
      }
    }

    public DateTime VIPLastdate
    {
      get => this._VIPLastdate;
      set
      {
        this._VIPLastdate = value;
        this._isDirty = true;
      }
    }

    public int VIPLevel
    {
      get => this._VIPLevel;
      set
      {
        this._VIPLevel = value;
        this._isDirty = true;
      }
    }

    public int VIPNextLevelDaysNeeded
    {
      get => this._VIPNextLevelDaysNeeded;
      set
      {
        this._VIPNextLevelDaysNeeded = value;
        this._isDirty = true;
      }
    }

    public int VIPOfflineDays
    {
      get => this._VIPOfflineDays;
      set
      {
        this._VIPOfflineDays = value;
        this._isDirty = true;
      }
    }

    public int VIPOnlineDays
    {
      get => this._VIPOnlineDays;
      set
      {
        this._VIPOnlineDays = value;
        this._isDirty = true;
      }
    }
  }
}
