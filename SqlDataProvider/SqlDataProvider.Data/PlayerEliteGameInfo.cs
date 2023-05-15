// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PlayerEliteGameInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class PlayerEliteGameInfo
  {
    private string yfgFrdHamEK;

    public int UserID { get; set; }

    public string NickName
    {
      get => this.yfgFrdHamEK;
      set => this.yfgFrdHamEK = value;
    }

    public int GameType { get; set; }

    public int Rank { get; set; }

    public int CurrentPoint { get; set; }

    public int Status { get; set; }

    public int Winer { get; set; }

    public bool ReadyStatus { get; set; }
  }
}
