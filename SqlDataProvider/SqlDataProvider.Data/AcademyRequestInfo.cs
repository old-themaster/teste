// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AcademyRequestInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AcademyRequestInfo
  {
    public int SenderID { get; set; }

    public int ReceiderID { get; set; }

    public int Type { get; set; }

    public DateTime CreateTime { get; set; }
  }
}
