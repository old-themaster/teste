﻿
// Type: Game.Logic.PetEffects.PetClearHellIceEquipEffectcs
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetClearHellIceEquipEffectcs : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetClearHellIceEquipEffectcs(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetClearHellIceEquipEffectcs, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBeginMoving += new PlayerEventHandle(this.player_BeginMoving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBeginMoving -= new PlayerEventHandle(this.player_BeginMoving);
    }

    private void player_BeginMoving(Living living)
    {
      (living.PetEffectList.GetOfType(ePetEffectType.PetReduceBloodAllBattleEquip) as PetReduceBloodAllBattleEquip)?.Stop();
      (living.PetEffectList.GetOfType(ePetEffectType.PetAddGuardEquip) as PetAddGuardEquip)?.Stop();
    }

    public override bool Start(Living living)
    {
      PetClearHellIceEquipEffectcs ofType = living.PetEffectList.GetOfType(ePetEffectType.PetClearHellIceEquipEffectcs) as PetClearHellIceEquipEffectcs;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
