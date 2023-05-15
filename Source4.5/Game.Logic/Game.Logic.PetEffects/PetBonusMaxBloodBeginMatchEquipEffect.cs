﻿
// Type: Game.Logic.PetEffects.PetBonusMaxBloodBeginMatchEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetBonusMaxBloodBeginMatchEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetBonusMaxBloodBeginMatchEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetBonusMaxBloodBeginMatchEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 152)
      {
        if (skillId != 153)
          return;
        this.m_value = 3000;
      }
      else
        this.m_value = 1500;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PetEffects.MaxBlood += this.m_value;
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PetEffects.MaxBlood -= this.m_value;
    }

    public override bool Start(Living living)
    {
      PetBonusMaxBloodBeginMatchEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetBonusMaxBloodBeginMatchEquipEffect) as PetBonusMaxBloodBeginMatchEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
