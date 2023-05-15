﻿
// Type: Game.Logic.PetEffects.PetReduceBaseDamageEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetReduceBaseDamageEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_value;

    public PetReduceBaseDamageEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetReduceBaseDamageEquip, elementID)
    {
      this.m_count = count;
      switch (skillId)
      {
        case 143:
          this.m_value = 100;
          break;
        case 144:
          this.m_value = 200;
          break;
        case 145:
          this.m_value = 300;
          break;
      }
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
      PetReduceBaseDamageEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetReduceBaseDamageEquip) as PetReduceBaseDamageEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
