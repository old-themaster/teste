using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerPetUpCondition : BaseActiveQuestCondition
    {

        public PlayerPetUpCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.UpLevelPetEvent += new GamePlayer.PlayerUpLevelPetEventHandle(this.onUpPet);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.UpLevelPetEvent -= new GamePlayer.PlayerUpLevelPetEventHandle(this.onUpPet);
        }

        private void onUpPet()
        {
            if (0 >= base.info.Para2)
            {
                base.Value = base.info.Para2;
                return;
            }

            base.Value = 0;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
