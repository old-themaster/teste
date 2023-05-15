﻿
// Type: Game.Logic.PetEffects.PetEnemyAttackBurningBloodEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetEnemyAttackBurningBloodEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetEnemyAttackBurningBloodEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetEnemyAttackBurningBloodEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBuffSkillPet += new PlayerEventHandle(this.player_AfterBuffSkill);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBuffSkillPet -= new PlayerEventHandle(this.player_AfterBuffSkill);
    }

    private void player_AfterBuffSkill(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      new PetBurningBloodShootingEquip(this.m_count, this.m_currentId, this.Info.ID.ToString()).Start(living);
    }

    public override bool Start(Living living)
    {
      PetEnemyAttackBurningBloodEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetEnemyAttackBurningBloodEquipEffect) as PetEnemyAttackBurningBloodEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
