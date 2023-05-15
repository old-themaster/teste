using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1067A : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1067A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1067A, elementID)
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
            CASE1067A cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1067A) as CASE1067A;
            if (cE != null)
            {
                cE.int_2 = ((this.int_2 > cE.int_2) ? this.int_2 : cE.int_2);
                return true;
            }
            return base.Start(living);
        }
        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += new LivingEventHandle(this.method_2);
            player.AfterKilledByLiving += new KillLivingEventHanlde(this.method_1);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(this.method_0);
            this.IsTrigger = true;
        }
        private void method_0(Player player_0)
        {
            Stop();
        }

        private void method_1(Living living_0, Living living_1, int int_7, int int_8)
        {
            if (!IsTrigger)
            {
                return;
            }
            int_6 = int_7 * 30 / 100;
            living_1.SyncAtTime = true;
            living_1.AddBlood(-int_6, 1);
            living_1.SyncAtTime = false;
            if (living_1.Blood <= 0)
            {
                living_1.Die();
                if (living_0 != null && living_0 is Player)
                {
                    (living_0 as Player).PlayerDetail.OnKillingLiving(living_0.Game, 2, living_1.Id, living_1.IsLiving, int_6);
                }
            }
            IsTrigger = false;
        }

        private void method_2(Living living_0)
        {
            int_1--;
            IsTrigger = true;
            if (int_1 < 0)
            {
                int_6 = 0;
                Stop();
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BeginSelfTurn -= new LivingEventHandle(this.method_2);
            player.AfterKilledByLiving -= new KillLivingEventHanlde(this.method_1);
        }
    }
}
