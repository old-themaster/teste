// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.QuestGoodManCardCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Server.Quests;

namespace Game.Server.Achievement
{
    internal class QuestGoodManCardCondition : BaseUserRecord
  {
    public QuestGoodManCardCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.PlayerQuestFinish += new GamePlayer.PlayerQuestFinishEventHandel(this.player_PlayerQuestFinish);

    private void player_PlayerQuestFinish(BaseQuest baseQuest)
    {
      if (baseQuest.Info.ID != 86)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.PlayerQuestFinish -= new GamePlayer.PlayerQuestFinishEventHandel(this.player_PlayerQuestFinish);
  }
}
