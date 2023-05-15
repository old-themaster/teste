using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class TimeVortexTerrorGame : APVEGameControl
	{
		public override void OnCreated()
		{
			base.Game.SetupMissions("12304");
			base.Game.TotalMissionCount = 4;
		}

		public override void OnPrepated()
		{
		}

		public override int CalculateScoreGrade(int score)
		{
			int result;
			if (score > 800)
			{
				result = 3;
			}
			else if (score > 725)
			{
				result = 2;
			}
			else if (score > 650)
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
		}
	}
}
