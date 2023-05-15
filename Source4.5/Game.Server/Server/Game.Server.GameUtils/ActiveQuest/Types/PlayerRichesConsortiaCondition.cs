using Bussiness;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerRichesConsortiaCondition : BaseActiveQuestCondition
    {

        public PlayerRichesConsortiaCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.GuildChanged += new GamePlayer.PlayerOwnConsortiaEventHandle(this.onRichesConsortia);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.GuildChanged -= new GamePlayer.PlayerOwnConsortiaEventHandle(this.onRichesConsortia);
        }

        private void onRichesConsortia()
        {
            int riches = 0;

            this.Value = 0;

            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
            {
                ConsortiaInfo consortiaSingle = consortiaBussiness.GetConsortiaSingle(this.m_quest.Owner.PlayerCharacter.ConsortiaID);
                
                if (consortiaSingle == null)
                {
                    return;
                }

                riches = m_quest.Owner.PlayerCharacter.Riches;

                if (riches >= this.info.Para2)
                {
                    this.Value = this.info.Para2;
                }
                else
                {
                    this.Value = riches;
                }
            }
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
