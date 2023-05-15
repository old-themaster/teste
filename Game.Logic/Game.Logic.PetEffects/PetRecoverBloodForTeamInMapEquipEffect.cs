﻿
// Type: Game.Logic.PetEffects.PetRecoverBloodForTeamInMapEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetRecoverBloodForTeamInMapEquipEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;
    private int m_value;

    public PetRecoverBloodForTeamInMapEquipEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.PetRecoverBloodForTeamInMapEquipEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if (skillId != 59)
      {
        if (skillId != 60)
          return;
        this.m_value = 3000;
      }
      else
        this.m_value = 1500;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerBuffSkillPet += new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerBuffSkillPet -= new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Living living)
    {
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      foreach (Player allTeamPlayer in living.Game.GetAllTeamPlayers(living))
      {
        allTeamPlayer.SyncAtTime = true;
        allTeamPlayer.AddBlood(this.m_value);
        allTeamPlayer.SyncAtTime = false;
      }
    }

    public override bool Start(Living living)
    {
      PetRecoverBloodForTeamInMapEquipEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.PetRecoverBloodForTeamInMapEquipEffect) as PetRecoverBloodForTeamInMapEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
