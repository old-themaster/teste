﻿using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1027A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1027A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1027A, elementID)
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
            CASE1027A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1027A) as CASE1027A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_1);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            this.Stop();
        }
        private void method_1(Living living_0)
        {
            this.int_1--;
            if (this.int_1 < 0)
            {
                this.Stop();
                return;
            }
            this.int_6 = living_0.MaxBlood * 3 / 100;
            this.int_6 += 1000;
            living_0.SyncAtTime = true;
            living_0.AddBlood(this.int_6);
            living_0.SyncAtTime = false;
            foreach (Living expr_93 in living_0.Game.Map.FindAllNearestSameTeam(living_0.X, living_0.Y, 250.0, living_0))
            {
                expr_93.SyncAtTime = true;
                expr_93.AddBlood(this.int_6);
                expr_93.SyncAtTime = false;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_7(player, base.Info, false);
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}
