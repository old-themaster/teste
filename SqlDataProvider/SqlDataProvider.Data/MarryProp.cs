// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.MarryProp
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class MarryProp
  {
    private bool _isCreatedMarryRoom;
    private bool _isGotRing;
    private bool _isMarried;
    private int _selfMarryRoomID;
    private int _spouseID;
    private string _spouseName;

    public bool IsCreatedMarryRoom
    {
      get => this._isCreatedMarryRoom;
      set => this._isCreatedMarryRoom = value;
    }

    public bool IsGotRing
    {
      get => this._isGotRing;
      set => this._isGotRing = value;
    }

    public bool IsMarried
    {
      get => this._isMarried;
      set => this._isMarried = value;
    }

    public int SelfMarryRoomID
    {
      get => this._selfMarryRoomID;
      set => this._selfMarryRoomID = value;
    }

    public int SpouseID
    {
      get => this._spouseID;
      set => this._spouseID = value;
    }

    public string SpouseName
    {
      get => this._spouseName;
      set => this._spouseName = value;
    }
  }
}
