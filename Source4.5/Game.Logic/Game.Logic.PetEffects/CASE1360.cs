﻿using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1360 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1360(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1360, elementID)
        {
            this.int_1 = count;
            this.int_4 = count;
            this.int_2 = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_3 = delay;
            this.int_5 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1360 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1360) as CASE1360;
            if (aE != null)
            {
                aE.int_2 = ((this.int_2 > aE.int_2) ? this.int_2 : aE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                player_0.Game.method_7(player_0, base.Info, true);
                player_0.AddPetEffect(new CASE1360A(2, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                Console.WriteLine("Restaura 6 % HP por turno");
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
