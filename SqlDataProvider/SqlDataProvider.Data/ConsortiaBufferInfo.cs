// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ConsortiaBufferInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ConsortiaBufferInfo
  {
    public DateTime BeginDate { get; set; }

    public int BufferID { get; set; }

    public int ConsortiaID { get; set; }

    public bool IsOpen { get; set; }

    public int Type { get; set; }

    public int ValidDate { get; set; }

    public int Value { get; set; }

    public int Group { get; set; }
  }
}
