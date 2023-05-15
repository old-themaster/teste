// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RateInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class RateInfo
  {
    public DateTime BeginDay { get; set; }

    public DateTime BeginTime { get; set; }

    public DateTime EndDay { get; set; }

    public DateTime EndTime { get; set; }

    public float Rate { get; set; }

    public int ServerID { get; set; }

    public int Type { get; set; }
  }
}
