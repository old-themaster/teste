using Game.Logic.Phy.Object;
using System;
namespace Game.Logic.PetEffects
{
    public class CASE1055 : BasePetEffect
    {
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        public CASE1055(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1055, elementID)
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
            CASE1055 aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1055) as CASE1055;
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
                    this.int_6 = 1000;
                    arg_3A_0.SyncAtTime = true;
                    arg_3A_0.AddBlood(this.int_6);
                    arg_3A_0.SyncAtTime = false;
                    Console.WriteLine("+1000 HP Equipe Cho Ca");
                }
            }
        }
        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(this.method_0);
        }
    }
}
