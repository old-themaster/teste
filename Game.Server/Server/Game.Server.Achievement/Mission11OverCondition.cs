// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.Mission11OverCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Achievement
{
    public class Mission11OverCondition : BaseUserRecord
  {
    public Mission11OverCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.MissionOver += new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);

    private void player_MissionOver(AbstractGame game, int missionId, bool isWin)
    {
      if (!(game.GameType == eGameType.Dungeon & isWin) || missionId != 6104 && missionId != 6204 && missionId != 6304)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.MissionOver -= new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);
  }
}
