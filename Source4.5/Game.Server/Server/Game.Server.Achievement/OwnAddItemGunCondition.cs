// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.OwnAddItemGunCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class OwnAddItemGunCondition : BaseUserRecord
  {
    public OwnAddItemGunCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.AfterKillingLiving += new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);

    private void player_AfterKillingLiving(
      AbstractGame game,
      int type,
      int id,
      bool isLiving,
      int demage,
      bool isSpanArea) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 0 + this.m_player.GetItemCount(7015) + this.m_player.GetItemCount(7016) + this.m_player.GetItemCount(7017) + this.m_player.GetItemCount(7018) + this.m_player.GetItemCount(7019) + this.m_player.GetItemCount(7020) + this.m_player.GetItemCount(7021) + this.m_player.GetItemCount(7022) + this.m_player.GetItemCount(7023));

    public override void RemoveTrigger(GamePlayer player) => player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
  }
}
