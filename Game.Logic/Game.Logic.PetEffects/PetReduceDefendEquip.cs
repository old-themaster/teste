﻿
// Type: Game.Logic.PetEffects.PetReduceDefendEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceDefendEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceDefendEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceDefendEquip, elementID)
    {
      this.m_count = count;
      if (skillId != 146)
      {
        if (skillId != 147)
          return;
        this.m_value = 500;
      }
      else
        this.m_value = 300;
    }

    public override void OnAttached(Living living)
    {
      living.BaseDamage -= (double) this.m_value;
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.BaseDamage += (double) this.m_value;
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetReduceDefendEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceDefendEquip) as PetReduceDefendEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
