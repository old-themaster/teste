// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ShopFreeCountInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using ProtoBuf;
using System;

namespace SqlDataProvider.Data
{
  [ProtoContract]
  public class ShopFreeCountInfo
  {
    [ProtoMember(1)]
    public int ShopID { get; set; }

    [ProtoMember(2)]
    public int Count { get; set; }

    [ProtoMember(3)]
    public DateTime CreateDate { get; set; }
  }
}
