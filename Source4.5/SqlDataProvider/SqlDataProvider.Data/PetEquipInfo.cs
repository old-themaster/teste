// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PetEquipInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class PetEquipInfo
  {
    private ItemTemplateInfo itemTemplateInfo_0;

    public PetEquipInfo(ItemTemplateInfo temp) => this.itemTemplateInfo_0 = temp;

    public ItemTemplateInfo Template => this.itemTemplateInfo_0;

    public int eqType { set; get; }

    public int eqTemplateID { set; get; }

    public DateTime startTime { set; get; }

    public int ValidDate { set; get; }

    public bool IsValidItem() => this.ValidDate == 0 || DateTime.Compare(this.startTime.AddDays((double) this.ValidDate), DateTime.Now) > 0;
  }
}
