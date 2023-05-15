// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementProcessInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class AchievementProcessInfo : DataObject
  {
    private int int_0;
    private int int_1;

    public AchievementProcessInfo()
    {
    }

    public AchievementProcessInfo(int type, int value)
    {
      this.CondictionType = type;
      this.Value = value;
    }

    public int CondictionType
    {
      get => this.int_0;
      set
      {
        this.int_0 = value;
        this._isDirty = true;
      }
    }

    public int Value
    {
      get => this.int_1;
      set
      {
        this.int_1 = value;
        this._isDirty = true;
      }
    }
  }
}
