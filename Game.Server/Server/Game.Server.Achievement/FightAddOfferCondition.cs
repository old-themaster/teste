// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightAddOfferCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    internal class FightAddOfferCondition : BaseUserRecord
  {
    public FightAddOfferCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.FightAddOfferEvent += new GamePlayer.PlayerFightAddOffer(this.player_FightAddOfferEvent);

    private void player_FightAddOfferEvent(int offer) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, offer);

    public override void RemoveTrigger(GamePlayer player) => player.FightAddOfferEvent -= new GamePlayer.PlayerFightAddOffer(this.player_FightAddOfferEvent);
  }
}
