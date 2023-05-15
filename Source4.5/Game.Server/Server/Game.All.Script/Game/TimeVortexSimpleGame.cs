using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    internal class TimeVortexSimpleGame : APVEGameControl
	{
		public override void OnCreated()
		{
			base.OnCreated();
			base.Game.SetupMissions("12004");
			base.Game.TotalMissionCount = 4;
		}

		public override void OnPrepated()
		{
			base.OnPrepated();
		}

		public override int CalculateScoreGrade(int score)
		{
			base.CalculateScoreGrade(score);
			int result;
			if (score > 900)
			{
				result = 3;
			}
			else if (score > 825)
			{
				result = 2;
			}
			else if (score > 725)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public override void OnGameOverAllSession()
		{
			base.OnGameOverAllSession();
		}
	}
}
