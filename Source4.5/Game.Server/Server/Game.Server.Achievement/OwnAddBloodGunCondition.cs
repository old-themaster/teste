﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.OwnAddBloodGunCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    public class OwnAddBloodGunCondition : BaseUserRecord
  {
    public OwnAddBloodGunCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, player.GetItemCount(17002));
  }
}
