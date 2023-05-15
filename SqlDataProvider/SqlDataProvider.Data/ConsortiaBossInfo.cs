// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ConsortiaBossInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ConsortiaBossInfo
  {
    public int ConsortiaID { get; set; }

    public int typeBoss { get; set; }

    public int callBossCount { get; set; }

    public int BossLevel { get; set; }

    public int Blood { get; set; }

    public DateTime LastOpenBoss { get; set; }

    public DateTime BossOpenTime { get; set; }

    public int powerPoint { get; set; }
  }
}
