using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
	public class LivingAfterShootedAction : BaseAction
	{
		private Living VrtQboWger;

		public LivingAfterShootedAction(Living living, int delay)
			: base(delay, 0)
		{
			VrtQboWger = living;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			VrtQboWger.OnAfterTakedBomb();
			Finish(tick);
		}
	}
}
