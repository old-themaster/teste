using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class UnknowQuestCondition : BaseCondition
	{
		public UnknowQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
			: base(quest, info, value)
		{
		}

		public override bool IsCompleted(GamePlayer player)
		{
			return false;
		}
	}
}
