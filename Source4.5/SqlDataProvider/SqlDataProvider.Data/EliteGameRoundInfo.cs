// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.EliteGameRoundInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class EliteGameRoundInfo
  {
    private int int_0;
    private int int_1;
    private PlayerEliteGameInfo playerEliteGameInfo_0;
    private PlayerEliteGameInfo playerEliteGameInfo_1;
    private PlayerEliteGameInfo playerEliteGameInfo_2;

    public int RoundID
    {
      get => this.int_0;
      set => this.int_0 = value;
    }

    public int RoundType
    {
      get => this.int_1;
      set => this.int_1 = value;
    }

    public PlayerEliteGameInfo PlayerOne
    {
      get => this.playerEliteGameInfo_0;
      set => this.playerEliteGameInfo_0 = value;
    }

    public PlayerEliteGameInfo PlayerTwo
    {
      get => this.playerEliteGameInfo_1;
      set => this.playerEliteGameInfo_1 = value;
    }

    public PlayerEliteGameInfo PlayerWin
    {
      get => this.playerEliteGameInfo_2;
      set => this.playerEliteGameInfo_2 = value;
    }
  }
}
