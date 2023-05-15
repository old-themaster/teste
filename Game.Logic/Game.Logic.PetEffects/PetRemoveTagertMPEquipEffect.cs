﻿
// Type: Game.Logic.PetEffects.PetRemoveTagertMPEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetRemoveTagertMPEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetRemoveTagertMPEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetRemoveTagertMPEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 183)
      {
        if (skillId != 184)
          return;
        this.m_value = 20;
      }
      else
        this.m_value = 10;
    }

    private void ChangeProperty(Player player)
    {
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability || player.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      this.IsTrigger = true;
      player.PetEffectTrigger = true;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBuffSkillPet += new PlayerEventHandle(this.ChangeProperty);
      player.AfterKillingLiving += new KillLivingEventHanlde(this.player_AfterKillingLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBuffSkillPet -= new PlayerEventHandle(this.ChangeProperty);
      player.AfterKillingLiving -= new KillLivingEventHanlde(this.player_AfterKillingLiving);
    }

    private void player_AfterKillingLiving(
      Living living,
      Living target,
      int damageAmount,
      int criticalAmount)
    {
      if (!this.IsTrigger || living == target || !(target is Player))
        return;
      ((TurnedLiving) target).RemovePetMP(this.m_value);
      this.IsTrigger = false;
    }

    public override bool Start(Living living)
    {
      PetRemoveTagertMPEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetRemoveTagertMPEquipEffect) as PetRemoveTagertMPEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
