// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.GiftInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class GiftInfo : DataObject
  {
    private DateTime _addDate;
    private int _count;
    private int _itemID;
    private Dictionary<string, object> _tempInfo = new Dictionary<string, object>();
    private ItemTemplateInfo _template;
    private int _templateId;
    private int _userID;

    internal GiftInfo(ItemTemplateInfo template)
    {
      this._template = template;
      if (this._template != null)
        this._templateId = this._template.TemplateID;
      if (this._tempInfo != null)
        return;
      this._tempInfo = new Dictionary<string, object>();
    }

    public bool CanStackedTo(GiftInfo to) => this._templateId == to.TemplateID && this.Template.MaxCount > 1;

    public static GiftInfo CreateFromTemplate(ItemTemplateInfo template, int count)
    {
      GiftInfo giftInfo = template != null ? new GiftInfo(template) : throw new ArgumentNullException(nameof (template));
      giftInfo.TemplateID = template.TemplateID;
      giftInfo.IsDirty = false;
      giftInfo.AddDate = DateTime.Now;
      giftInfo.Count = count;
      return giftInfo;
    }

    public static GiftInfo CreateWithoutInit(ItemTemplateInfo template) => new GiftInfo(template);

    public DateTime AddDate
    {
      get => this._addDate;
      set
      {
        if (this._addDate == value)
          return;
        this._addDate = value;
        this._isDirty = true;
      }
    }

    public int Count
    {
      get => this._count;
      set
      {
        if (this._count == value)
          return;
        this._count = value;
        this._isDirty = true;
      }
    }

    public int ItemID
    {
      get => this._itemID;
      set
      {
        this._itemID = value;
        this._isDirty = true;
      }
    }

    public Dictionary<string, object> TempInfo => this._tempInfo;

    public ItemTemplateInfo Template => this._template;

    public int TemplateID
    {
      get => this._templateId;
      set
      {
        if (this._templateId == value)
          return;
        this._templateId = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get => this._userID;
      set
      {
        if (this._userID == value)
          return;
        this._userID = value;
        this._isDirty = true;
      }
    }
  }
}
