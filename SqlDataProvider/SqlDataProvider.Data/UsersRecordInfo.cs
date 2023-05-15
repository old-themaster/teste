// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UsersRecordInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class UsersRecordInfo : DataObject
  {
    private int _userID;
    private int _recordID;
    private int _total;

    public int UserID
    {
      get => this._userID;
      set
      {
        this._userID = value;
        this._isDirty = true;
      }
    }

    public int RecordID
    {
      get => this._recordID;
      set
      {
        this._recordID = value;
        this._isDirty = true;
      }
    }

    public int Total
    {
      get => this._total;
      set
      {
        this._total = value;
        this._isDirty = true;
      }
    }
  }
}
