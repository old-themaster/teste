// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.AddMedalCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    internal class AddMedalCondition : BaseUserRecord
  {
    public AddMedalCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.PlayerAddItem += new GamePlayer.PlayerAddItemEventHandel(this.player_PlayerAddItem);

    private void player_PlayerAddItem(string type, int value)
    {
      if (!(type == "Medal"))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, value);
    }

    public override void RemoveTrigger(GamePlayer player) => player.PlayerAddItem -= new GamePlayer.PlayerAddItemEventHandel(this.player_PlayerAddItem);
  }
}
