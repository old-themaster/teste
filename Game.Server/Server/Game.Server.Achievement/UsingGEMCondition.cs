// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.UsingGEMCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class UsingGEMCondition : BaseUserRecord
  {
    public UsingGEMCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.AfterUsingItem += new GamePlayer.PlayerItemPropertyEventHandle(this.player_AfterUsingItem);

    private void player_AfterUsingItem(int templateID, int count)
    {
      if (templateID != 311000 && templateID != 311999 && (templateID != 312000 && templateID != 312999) && (templateID != 313000 && templateID != 313999))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.AfterUsingItem -= new GamePlayer.PlayerItemPropertyEventHandle(this.player_AfterUsingItem);
  }
}
