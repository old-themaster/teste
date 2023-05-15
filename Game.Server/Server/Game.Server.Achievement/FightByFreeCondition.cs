// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightByFreeCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class FightByFreeCondition : BaseUserRecord
  {
    public FightByFreeCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.GameOver += new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);

    private void player_GameOver(
      AbstractGame game,
      bool isWin,
      int gainXp,
      bool isSpanArea,
      bool isCouple)
    {
      if (!(game.GameType == eGameType.Free & isWin))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);
  }
}
