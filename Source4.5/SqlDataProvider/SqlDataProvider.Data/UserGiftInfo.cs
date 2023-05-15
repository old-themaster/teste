// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserGiftInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class UserGiftInfo
  {
    public int ID { get; set; }

    public int SenderID { get; set; }

    public int ReceiverID { get; set; }

    public int TemplateID { get; set; }

    public int Count { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdate { get; set; }
  }
}
