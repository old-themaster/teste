using Bussiness;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class OwnConsortiaCondition : BaseActiveQuestCondition
    {

        public OwnConsortiaCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.GuildChanged += new GamePlayer.PlayerOwnConsortiaEventHandle(this.onOwnConsortia);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.GuildChanged -= new GamePlayer.PlayerOwnConsortiaEventHandle(this.onOwnConsortia);
        }

        private void onOwnConsortia()
        {
            int num = 0;

            this.Value = 0;

            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
            {
                ConsortiaInfo consortiaSingle = consortiaBussiness.GetConsortiaSingle(this.m_quest.Owner.PlayerCharacter.ConsortiaID);
                
                if (consortiaSingle == null)
                {
                    return;
                }

                switch (this.info.Para1)
                {
                    case 0:
                        num = consortiaSingle.Count;
                        break;
                    case 1:
                        num = m_quest.Owner.PlayerCharacter.Riches;
                        break;
                    case 2:
                        num = consortiaSingle.SmithLevel;
                        break;
                    case 3:
                        num = consortiaSingle.ShopLevel;
                        break;
                    case 4:
                        num = consortiaSingle.StoreLevel;
                        break;
                }

                if (num >= this.info.Para2)
                {
                    this.Value = this.info.Para2;
                }
                else
                {
                    this.Value = num;
                }
            }
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
