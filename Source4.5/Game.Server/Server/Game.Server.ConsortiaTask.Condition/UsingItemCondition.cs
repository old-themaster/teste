using Game.Server.ConsortiaTask.Data;
using SqlDataProvider.Data;

namespace Game.Server.ConsortiaTask.Condition
{
	public class UsingItemCondition : BaseConsortiaTaskCondition
	{
		public UsingItemCondition(ConsortiaTaskUserDataInfo player, BaseConsortiaTask quest, ConsortiaTaskInfo info, int value)
			: base(player, quest, info, value)
		{
		}

		public override void AddTrigger(ConsortiaTaskUserDataInfo player)
		{
			player.Player.AfterUsingItem += method_0;
		}

		public override void RemoveTrigger(ConsortiaTaskUserDataInfo player)
		{
			player.Player.AfterUsingItem -= method_0;
		}

		private void method_0(int int_1, int int_2)
		{
			if (int_1 == m_info.Para1 && base.Value < m_info.Para2)
			{
				base.Value += int_2;
			}
		}
	}
}
