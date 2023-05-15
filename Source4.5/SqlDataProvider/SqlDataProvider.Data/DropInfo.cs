// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DropInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class DropInfo
  {
    public DropInfo(int id, int time, int count, int maxCount)
    {
      this.ID = id;
      this.Time = time;
      this.Count = count;
      this.MaxCount = maxCount;
    }

    public int Count { get; set; }

    public int ID { get; set; }

    public int MaxCount { get; set; }

    public int Time { get; set; }
  }
}
