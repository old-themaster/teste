﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.MissionKillChiefCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class MissionKillChiefCondition : BaseUserRecord
  {
    public MissionKillChiefCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.AfterKillingLiving += new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);

    private void player_AfterKillingLiving(
      AbstractGame game,
      int type,
      int id,
      bool isLiving,
      int demage,
      bool isSpanArea)
    {
      if (game.GameType != eGameType.Dungeon || isLiving || (type != 2 || id != 3317))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
  }
}
