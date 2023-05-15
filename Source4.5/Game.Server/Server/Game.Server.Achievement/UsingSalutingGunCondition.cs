// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.UsingSalutingGunCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class UsingSalutingGunCondition : BaseUserRecord
  {
    public UsingSalutingGunCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.AfterUsingItem += new GamePlayer.PlayerItemPropertyEventHandle(this.player_AfterUsingItem);

    private void player_AfterUsingItem(int templateID, int count)
    {
      if (templateID != 21002 && templateID != 21006 && (templateID != 11463 && templateID != 11528) && (templateID != 11539 && templateID != 11540 && (templateID != 11542 && templateID != 11549)) && (templateID != 200337 && templateID != 170145))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.AfterUsingItem -= new GamePlayer.PlayerItemPropertyEventHandle(this.player_AfterUsingItem);
  }
}
