﻿// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserRank
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserRank
  {
    public DateTime EndDate { get; set; }

    public string Rank { get; set; }

    public DateTime StartDate { get; set; }

    public int Type { get; set; }

    public int UserID { get; set; }
  }
}