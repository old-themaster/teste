using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class MoneyAccumulateCondition : BaseActiveQuestCondition
    {
        public MoneyAccumulateCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.OnPropertiesChangedEvent += Change;
        }

        private void Change(GamePlayer player)
        {
            if (this.Value >= this.info.Para2)
            {
                this.OnRemoved();
                return;
            }

            if (this.info.CondictionType == 38)
            {
                if (player.PlayerCharacter.Money > this.info.Para2)
                {
                    this.Value = this.info.Para2;
                }
                else
                {
                    this.Value = player.PlayerCharacter.Money;
                }
            }

            if (this.info.CondictionType == -1)
            {
                if (player.PlayerCharacter.Gold > this.info.Para2)
                {
                    this.Value = this.info.Para2;
                }
                else
                {
                    this.Value = player.PlayerCharacter.Gold;
                }
            }
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.OnPropertiesChangedEvent -= Change;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
