// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.ItemStrengthenCondition
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B480679-DF24-46B7-834E-821AA9A4FB3F
// Assembly location: C:\Users\Anderson\Desktop\Source 4.2\Game.Server.dll

namespace Game.Server.Achievement
{
    public class ItemStrengthenCondition : BaseUserRecord
    {
        public ItemStrengthenCondition(GamePlayer player, int type)
          : base(player, type)
        {
            this.AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.ItemStrengthen += new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
        }

        private void player_ItemStrengthen(int categoryID, int level)
        {
            this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, level, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.ItemStrengthen -= new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
        }
    }
}
