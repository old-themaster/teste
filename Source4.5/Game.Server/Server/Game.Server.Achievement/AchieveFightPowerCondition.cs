// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.AchieveFightPowerCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class AchieveFightPowerCondition : BaseUserRecord
  {
    public AchieveFightPowerCondition(GamePlayer player, int type)
      : base(player, type)
    {
    }

    public override void AddTrigger(GamePlayer player) => player.ItemStrengthen += new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);

    private void player_ItemStrengthen(int categoryID, int level) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);

    public override void RemoveTrigger(GamePlayer player) => player.ItemStrengthen -= new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
  }
}
