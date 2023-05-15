// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RingstationBattleFieldInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class RingstationBattleFieldInfo : DataObject
  {
    public DateTime BattleTime { get; set; }

    public bool DareFlag { get; set; }

    public int ID { get; set; }

    public int Level { get; set; }

    public bool SuccessFlag { get; set; }

    public int UserID { get; set; }

    public string UserName { get; set; }
  }
}
