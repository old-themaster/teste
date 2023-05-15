// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.VIPCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    internal class VIPCondition : BaseUserRecord
  {
    public VIPCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.Event_0 += new GamePlayer.PlayerVIPUpgrade(this.player_PlayerVIPUpgrade);

    private void player_PlayerVIPUpgrade(int level, int exp) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, level, 1);

    public override void RemoveTrigger(GamePlayer player) => player.Event_0 -= new GamePlayer.PlayerVIPUpgrade(this.player_PlayerVIPUpgrade);
  }
}
