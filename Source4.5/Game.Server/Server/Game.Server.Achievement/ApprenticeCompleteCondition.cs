﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.ApprenticeCompleteCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class ApprenticeCompleteCondition : BaseUserRecord
  {
    public ApprenticeCompleteCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.AcademyEvent += new GamePlayer.PlayerAcademyEventHandle(this.academyEvent);

    private void academyEvent(GamePlayer friendly, int type)
    {
      if (type != 2)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.AcademyEvent -= new GamePlayer.PlayerAcademyEventHandle(this.academyEvent);
  }
}
