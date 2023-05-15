using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1054 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1054(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1054, elementID)
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
            CASE1054 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1054) as CASE1054;
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
                foreach (Player arg_3A_0 in player_0.Game.GetAllTeamPlayers(player_0))
                {
                    this.int_6 = 800;
                    arg_3A_0.SyncAtTime = true;
                    arg_3A_0.AddBlood(this.int_6);
                    arg_3A_0.SyncAtTime = false;
                    Console.WriteLine("+800 HP Equipe Cho Ca");
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
