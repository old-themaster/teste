// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.QuestDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class QuestDataInfo : DataObject
  {
    private DateTime _completeDate;
    private int _condition1;
    private int _condition2;
    private int _condition3;
    private int _condition4;
    private bool _isComplete;
    private bool _isExist;
    private int _questID;
    private int _randDobule;
    private int _repeatFinish;
    private int _userID;

    public int GetConditionValue(int index)
    {
      switch (index)
      {
        case 0:
          return this.Condition1;
        case 1:
          return this.Condition2;
        case 2:
          return this.Condition3;
        case 3:
          return this.Condition4;
        default:
          throw new Exception("Quest condition index out of range.");
      }
    }

    public void SaveConditionValue(int index, int value)
    {
      switch (index)
      {
        case 0:
          this.Condition1 = value;
          break;
        case 1:
          this.Condition2 = value;
          break;
        case 2:
          this.Condition3 = value;
          break;
        case 3:
          this.Condition4 = value;
          break;
        default:
          throw new Exception("Quest condition index out of range.");
      }
    }

    public DateTime CompletedDate
    {
      get => this._completeDate;
      set
      {
        this._completeDate = value;
        this._isDirty = true;
      }
    }

    public int Condition1
    {
      get => this._condition1;
      set
      {
        if (value == this._condition1)
          return;
        this._condition1 = value;
        this._isDirty = true;
      }
    }

    public int Condition2
    {
      get => this._condition2;
      set
      {
        if (value == this._condition2)
          return;
        this._condition2 = value;
        this._isDirty = true;
      }
    }

    public int Condition3
    {
      get => this._condition3;
      set
      {
        if (value == this._condition3)
          return;
        this._condition3 = value;
        this._isDirty = true;
      }
    }

    public int Condition4
    {
      get => this._condition4;
      set
      {
        if (value == this._condition4)
          return;
        this._condition4 = value;
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

    public bool IsExist
    {
      get => this._isExist;
      set
      {
        this._isExist = value;
        this._isDirty = true;
      }
    }

    public int QuestID
    {
      get => this._questID;
      set
      {
        this._questID = value;
        this._isDirty = true;
      }
    }

    public int RandDobule
    {
      get => this._randDobule;
      set
      {
        this._randDobule = value;
        this._isDirty = true;
      }
    }

    public int RepeatFinish
    {
      get => this._repeatFinish;
      set
      {
        this._repeatFinish = value;
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
