// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementData
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AchievementData : DataObject
  {
    private bool bool_0;
    private DateTime dateTime_0;
    private int int_0;
    private int int_1;

    public AchievementData()
    {
      this.UserID = 0;
      this.AchievementID = 0;
      this.IsComplete = false;
      this.CompletedDate = DateTime.Now;
    }

    public int AchievementID
    {
      get => this.int_1;
      set
      {
        this.int_1 = value;
        this._isDirty = true;
      }
    }

    public DateTime CompletedDate
    {
      get => this.dateTime_0;
      set
      {
        this.dateTime_0 = value;
        this._isDirty = true;
      }
    }

    public bool IsComplete
    {
      get => this.bool_0;
      set
      {
        this.bool_0 = value;
        this._isDirty = true;
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
  }
}
