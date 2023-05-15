// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.StartMissionCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class StartMissionCondition : BaseUserRecord
  {
    public StartMissionCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.MissionTurnOver += new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionTurnOver);

    private void player_MissionTurnOver(AbstractGame game, int missionId, int turnNum)
    {
      if (game.RoomType == eRoomType.Freshman || game.RoomType == eRoomType.FightLab)
        return;
      ++this.m_player.missionPlayed;
      if (this.m_player.missionPlayed != 2)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.MissionTurnOver -= new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionTurnOver);
  }
}
