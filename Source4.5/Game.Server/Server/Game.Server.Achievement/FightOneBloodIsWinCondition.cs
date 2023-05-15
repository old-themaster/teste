// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightOneBloodIsWinCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class FightOneBloodIsWinCondition : BaseUserRecord
  {
    public FightOneBloodIsWinCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.FightOneBloodIsWin += new GamePlayer.PlayerFightOneBloodIsWin(this.player_OneBloodIsWin);

    private void player_OneBloodIsWin(eRoomType roomType, bool isWin)
    {
      if (!(roomType == eRoomType.Match & isWin))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.FightOneBloodIsWin -= new GamePlayer.PlayerFightOneBloodIsWin(this.player_OneBloodIsWin);
  }
}
