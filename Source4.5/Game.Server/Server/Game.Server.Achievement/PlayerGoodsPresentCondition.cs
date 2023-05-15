// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.PlayerGoodsPresentCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Achievement
{
    internal class PlayerGoodsPresentCondition : BaseUserRecord
  {
    public PlayerGoodsPresentCondition(GamePlayer player, int type)
      : base(player, type)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
    }

    private void player_PlayerGoodsPresent(int count)
    {
    }

    public override void RemoveTrigger(GamePlayer player)
    {
    }
  }
}
