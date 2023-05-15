// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightWithWeaponCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class FightWithWeaponCondition : BaseUserRecord
  {
    public FightWithWeaponCondition(GamePlayer player, int type)
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
      if (game.GameType != eGameType.Free && game.GameType != eGameType.Guild && game.GameType != eGameType.ALL || !(this.m_player.MainWeapon != null & isWin))
        return;
      int templateId = this.m_player.MainWeapon.TemplateID;
      if ((templateId == 7023 || templateId == 70231 || (templateId == 70232 || templateId == 70233) || templateId == 70234) && this.m_type == 64)
        this.m_player.AchievementInventory.UpdateUserAchievement(64, 1);
      if ((templateId == 7016 || templateId == 70161 || (templateId == 70162 || templateId == 70163) || templateId == 70164) && this.m_type == 65)
        this.m_player.AchievementInventory.UpdateUserAchievement(65, 1);
      if ((templateId == 7017 || templateId == 70171 || (templateId == 70172 || templateId == 70173) || templateId == 70174) && this.m_type == 66)
        this.m_player.AchievementInventory.UpdateUserAchievement(66, 1);
      if ((templateId == 7015 || templateId == 70151 || (templateId == 70152 || templateId == 70153) || templateId == 70154) && this.m_type == 67)
        this.m_player.AchievementInventory.UpdateUserAchievement(67, 1);
      if ((templateId == 7019 || templateId == 70191 || (templateId == 70192 || templateId == 70193) || templateId == 70194) && this.m_type == 68)
        this.m_player.AchievementInventory.UpdateUserAchievement(68, 1);
      if ((templateId == 7022 || templateId == 70221 || (templateId == 70222 || templateId == 70223) || templateId == 70224) && this.m_type == 69)
        this.m_player.AchievementInventory.UpdateUserAchievement(69, 1);
      if ((templateId == 7020 || templateId == 70201 || (templateId == 70202 || templateId == 70203) || templateId == 70204) && this.m_type == 70)
        this.m_player.AchievementInventory.UpdateUserAchievement(70, 1);
      if ((templateId == 7021 || templateId == 70211 || (templateId == 70212 || templateId == 70213) || templateId == 70214) && this.m_type == 71)
        this.m_player.AchievementInventory.UpdateUserAchievement(71, 1);
      if (templateId != 7018 && templateId != 70181 && (templateId != 70182 && templateId != 70183) && templateId != 70184 || this.m_type != 72)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(72, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);
  }
}
