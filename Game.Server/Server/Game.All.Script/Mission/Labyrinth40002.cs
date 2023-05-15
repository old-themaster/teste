﻿using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
  public class Labyrinth40002 : AMissionControl
  {
    private SimpleBoss simpleBoss_0;
    private int int_0;
    private int int_1;
    private int int_2;

    public override int CalculateScoreGrade(int score)
    {
      base.CalculateScoreGrade(score);
      if (score > 1870)
        return 3;
      if (score > 1825)
        return 2;
      return score > 1780 ? 1 : 0;
    }

    public override void OnPrepareNewSession()
    {
      base.OnPrepareNewSession();
      int[] npcIds1 = new int[2]{ this.int_2, this.int_1 };
      int[] npcIds2 = new int[1]{ this.int_2 };
      this.Game.LoadResources(npcIds1);
      this.Game.LoadNpcGameOverResources(npcIds2);
      this.Game.AddLoadingFile(2, "image/bomb/blastOut/blastOut51.swf", "bullet51");
      this.Game.AddLoadingFile(2, "image/bomb/bullet/bullet51.swf", "bullet51");
      this.Game.SetMap(1237);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      this.simpleBoss_0 = this.Game.CreateBoss(this.int_2, 1316, 444, -1, 1, "");
      this.simpleBoss_0.SetRelateDemagemRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
    }

    public override void OnNewTurnStarted()
    {
      base.OnNewTurnStarted();
      if (this.simpleBoss_0 == null || this.simpleBoss_0.IsLiving)
        return;
      this.Game.ClearAllChild();
    }

    public override void OnBeginNewTurn()
    {
      base.OnBeginNewTurn();
    }

    public override bool CanGameOver()
    {
      if (this.Game.TurnIndex > this.Game.MissionInfo.TotalTurn - 1)
        return true;
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
      {
        if (this.Game.CanEnterGate)
          return true;
        ++this.int_0;
        this.Game.CanShowBigBox = true;
      }
      return false;
    }

    public override int UpdateUIData()
    {
      base.UpdateUIData();
      return this.int_0;
    }

    public override void OnGameOverMovie()
    {
      base.OnGameOverMovie();
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
        this.Game.IsWin = true;
      else
        this.Game.IsWin = false;
    }

    public override void OnGameOver()
    {
      base.OnGameOver();
    }

    public Labyrinth40002()
    {
      this.int_1 = 2004;
      this.int_2 = 40002;
    }
  }
}
