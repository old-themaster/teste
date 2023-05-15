﻿using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1025 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int CxnAljSyIW;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        public CASE1025(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1025, elementID)
        {
            this.int_1 = count;
            this.int_3 = count;
            this.CxnAljSyIW = ((probability == -1) ? 10000 : probability);
            this.int_0 = type;
            this.int_2 = delay;
            this.int_4 = skillId;
        }
        public override bool Start(Living living)
        {
            CASE1025 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1025) as CASE1025;
            if (aE != null)
            {
                aE.CxnAljSyIW = ((this.CxnAljSyIW > aE.CxnAljSyIW) ? this.CxnAljSyIW : aE.CxnAljSyIW);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerBuffSkillPet += new PlayerEventHandle(this.method_1);
            player.PlayerAnyShellThrow += new PlayerEventHandle(this.method_0);
        }
        private void method_0(Player player_0)
        {
            if (this.IsTrigger)
            {
                foreach (Player current in player_0.Game.GetAllTeamPlayers(player_0))
                {
                    if (current.PlayerDetail != player_0.PlayerDetail)
                    {
                        current.Game.method_7(current, base.Info, true);
                        current.AddPetEffect(new CASE1025A(2, this.CxnAljSyIW, this.int_0, this.int_4, this.int_2, base.Info.ID.ToString()), 0);
                        Console.WriteLine("Aumentar 300 de dano para companheiros de equipe");
                    }
                }
                this.IsTrigger = false;
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_1);
            player.PlayerAnyShellThrow -= new PlayerEventHandle(this.method_0);
        }
        private void method_1(Player player_0)
        {
            if (player_0.PetEffects.CurrentUseSkill == this.int_4)
            {
                this.IsTrigger = true;
            }
        }
    }
}
namespace Game.Logic.PetEffects
{
    public class CASE1025A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1025A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1025A, elementID)
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
            CASE1025A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1025A) as CASE1025A;
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
            if (this.int_6 == 0)
            {
                this.int_6 = 300;
                player.BaseDamage += (double)this.int_6;
            }
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
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.method_7(player, base.Info, false);
            player.BaseDamage -= (double)this.int_6;
            this.int_6 = 0;
            player.BeginSelfTurn -= new LivingEventHandle(this.method_1);
        }
    }
}