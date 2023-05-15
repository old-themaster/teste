// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserExitRoomLogInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserExitRoomLogInfo
  {
    public int UserID { get; set; }

    public int TotalExitTime { get; set; }

    public DateTime LastLogout { get; set; }

    public DateTime TimeBlock { get; set; }

    public bool IsNoticed { get; set; }

    public bool CanNotice
    {
      get
      {
        if (this.TotalExitTime <= 3 || this.IsNoticed)
          return false;
        this.IsNoticed = true;
        return true;
      }
    }
  }
}
