// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.ChangeGradeCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class ChangeGradeCondition : BaseUserRecord
  {
    public ChangeGradeCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.LevelUp += new GamePlayer.PlayerEventHandle(this.player_LevelUp);

    private void player_LevelUp(GamePlayer player) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, player.Level, 1);

    public override void RemoveTrigger(GamePlayer player) => player.LevelUp -= new GamePlayer.PlayerEventHandle(this.player_LevelUp);
  }
}
