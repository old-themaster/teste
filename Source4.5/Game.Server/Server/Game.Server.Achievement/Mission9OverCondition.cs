// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.Mission9OverCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class Mission9OverCondition : BaseUserRecord
  {
    public Mission9OverCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.MissionOver += new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);

    private void player_MissionOver(AbstractGame game, int missionId, bool isWin)
    {
      if (!(game.GameType == eGameType.Dungeon & isWin) || missionId != 4103 && missionId != 4203 && missionId != 4303)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.MissionOver -= new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);
  }
}
