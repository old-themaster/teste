﻿
// Type: Game.Logic.PetEffects.PetShootedAddDamageForTeamEquip
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetShootedAddDamageForTeamEquip : AbstractPetEffect
  {
    private int m_count;
    private int m_currentId;

    public PetShootedAddDamageForTeamEquip(int count, int skillId, string elementID)
      : base(ePetEffectType.PetShootedAddDamageForTeamEquip, elementID)
    {
      this.m_count = count;
      this.m_currentId = skillId;
    }

    public override void OnAttached(Living living)
    {
      (living as Player).PlayerShoot += new PlayerEventHandle(this.player_Shoot);
    }

    public override void OnRemoved(Living living)
    {
      (living as Player).PlayerShoot -= new PlayerEventHandle(this.player_Shoot);
    }

    private void player_Shoot(Player living)
    {
      foreach (Player allTeamPlayer in living.Game.GetAllTeamPlayers((Living) living))
      {
        if (allTeamPlayer != living)
          allTeamPlayer.AddPetEffect((AbstractPetEffect) new PetActiveDamageEquip(2, this.m_currentId, this.Info.ID.ToString()), 0);
      }
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      PetShootedAddDamageForTeamEquip ofType = living.PetEffectList.GetOfType(ePetEffectType.PetShootedAddDamageForTeamEquip) as PetShootedAddDamageForTeamEquip;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
