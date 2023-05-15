﻿using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1153 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1153(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1153, elementID)
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
            CASE1153 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1153) as CASE1153;
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
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_5)
            {
                player_0.AddPetEffect(new CASE1153A(3, this.int_2, this.int_0, this.int_5, this.int_3, base.Info.ID.ToString()), 0);
                Console.WriteLine("Dano Base + 15 % ");
            }
        }
    }
}
