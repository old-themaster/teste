// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.WorldMgrDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using ProtoBuf;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  [ProtoContract]
  public class WorldMgrDataInfo
  {
    [ProtoMember(1)]
    public Dictionary<int, ShopFreeCountInfo> ShopFreeCount;
  }
}
