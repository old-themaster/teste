// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.HotSpringRoomInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class HotSpringRoomInfo
  {
    public bool CanEnter() => this.curCount < this.maxCount;

    public int curCount { get; set; }

    public int effectiveTime { get; set; }

    public DateTime endTime { get; set; }

    public int maxCount { get; set; }

    public int playerID { get; set; }

    public string playerName { get; set; }

    public int roomID { get; set; }

    public string roomIntroduction { get; set; }

    public string roomName { get; set; }

    public int roomNumber { get; set; }

    public string roomPassword { get; set; }

    public int roomType { get; set; }

    public DateTime startTime { get; set; }
  }
}
