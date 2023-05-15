// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.ChangeColorsCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Achievement
{
    public class ChangeColorsCondition : BaseUserRecord
  {
    public ChangeColorsCondition(GamePlayer player, int type)
      : base(player, type)
      => this.AddTrigger(player);

    public override void AddTrigger(GamePlayer player) => player.PlayerPropertyChanged += new GamePlayer.PlayerPropertyChangedEventHandel(this.player_PlayerPropertyChanged);

    private void player_PlayerPropertyChanged(PlayerInfo character)
    {
      string[] strArray = character.Colors.Split(',');
      int num = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].ToString() != "" && strArray[index].ToString() != "|")
          ++num;
      }
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, num, 1);
    }

    public override void RemoveTrigger(GamePlayer player) => player.PlayerPropertyChanged -= new GamePlayer.PlayerPropertyChangedEventHandel(this.player_PlayerPropertyChanged);
  }
}
