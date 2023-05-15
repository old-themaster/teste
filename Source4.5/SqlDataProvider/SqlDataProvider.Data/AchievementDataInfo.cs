// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AchievementDataInfo : DataObject
  {
    private int _achievementID;
    private DateTime _completedDate;
    private bool _isComplete;
    private int _userID;

    public int AchievementID
    {
      get => this._achievementID;
      set
      {
        this._achievementID = value;
        this._isDirty = true;
      }
    }

    public DateTime CompletedDate
    {
      get => this._completedDate;
      set
      {
        this._completedDate = value;
        this._isDirty = true;
      }
    }

    public bool IsComplete
    {
      get => this._isComplete;
      set
      {
        this._isComplete = value;
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
