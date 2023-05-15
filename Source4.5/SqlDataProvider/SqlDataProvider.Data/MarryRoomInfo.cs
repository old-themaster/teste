// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.MarryRoomInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class MarryRoomInfo
  {
    public int AvailTime { get; set; }

    public DateTime BeginTime { get; set; }

    public DateTime BreakTime { get; set; }

    public int BrideID { get; set; }

    public string BrideName { get; set; }

    public int GroomID { get; set; }

    public string GroomName { get; set; }

    public bool GuestInvite { get; set; }

    public int ID { get; set; }

    public bool IsGunsaluteUsed { get; set; }

    public bool IsHymeneal { get; set; }

    public int MapIndex { get; set; }

    public int MaxCount { get; set; }

    public string Name { get; set; }

    public int PlayerID { get; set; }

    public string PlayerName { get; set; }

    public string Pwd { get; set; }

    public string RoomIntroduction { get; set; }

    public int ServerID { get; set; }
  }
}
