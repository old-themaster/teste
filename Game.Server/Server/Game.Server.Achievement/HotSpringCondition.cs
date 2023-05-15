// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.HotSpringCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class HotSpringCondition : BaseUserRecord
  {
    public HotSpringCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.PlayerSpa += new GamePlayer.PlayerOwnSpaEventHandle(this.player_PlayerSpa);

    private void player_PlayerSpa(int onlineTimeSpa) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, onlineTimeSpa);

    public override void RemoveTrigger(GamePlayer player) => player.PlayerSpa -= new GamePlayer.PlayerOwnSpaEventHandle(this.player_PlayerSpa);
  }
}
