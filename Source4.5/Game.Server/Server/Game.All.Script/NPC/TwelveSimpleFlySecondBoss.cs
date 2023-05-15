using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class TwelveSimpleFlySecondBoss : ABrain
	{
        private int BombID = 12019;

        private int turnIndex = 0;

        private List<SimpleNpc> Bombs = new List<SimpleNpc>();

        private SimpleNpc Bomb1;

        private SimpleNpc Bomb2;

        private SimpleNpc Bomb3;

        private SimpleNpc Bomb4;

        private SimpleNpc Bomb5;

        private SimpleNpc Bomb6;

        private SimpleNpc Bomb7;

        private SimpleNpc Bomb8;

        private SimpleNpc Bomb9;

        private SimpleNpc Bomb10;

        private SimpleNpc Bomb11;

        private SimpleNpc Bomb12;  

        public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			if (base.Body.Direction == -1)
			{
				base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
				return;
			}
			base.Body.SetRect(-((SimpleBoss)base.Body).NpcInfo.X - ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
		}

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnCreated()
		{
			base.OnCreated();
            LivingConfig livingConfig = ((PVEGame)Game).BaseConfig();
            livingConfig.IsFly = true;
            livingConfig.CanTakeDamage = false;
            livingConfig.CanCountKill = false;
            livingConfig.isShowSmallMapPoint = false; 
            Bombs.Add(Bomb1 = ((SimpleBoss)Body).CreateChild(BombID, 250, 200, false, livingConfig));
            Bombs.Add(Bomb2 = ((SimpleBoss)Body).CreateChild(BombID, 250, 400, false, livingConfig));
            Bombs.Add(Bomb3 = ((SimpleBoss)Body).CreateChild(BombID, 250, 600, false, livingConfig));
            Bombs.Add(Bomb4 = ((SimpleBoss)Body).CreateChild(BombID, 550, 200, false, livingConfig));
            Bombs.Add(Bomb5 = ((SimpleBoss)Body).CreateChild(BombID, 550, 400, false, livingConfig));
            Bombs.Add(Bomb6 = ((SimpleBoss)Body).CreateChild(BombID, 550, 600, false, livingConfig));
            Bombs.Add(Bomb7 = ((SimpleBoss)Body).CreateChild(BombID, 1400, 200, false, livingConfig));
            Bombs.Add(Bomb8 = ((SimpleBoss)Body).CreateChild(BombID, 1400, 400, false, livingConfig));
            Bombs.Add(Bomb9 = ((SimpleBoss)Body).CreateChild(BombID, 1400, 600, false, livingConfig));
            Bombs.Add(Bomb10 = ((SimpleBoss)Body).CreateChild(BombID, 1700, 200, false, livingConfig));
            Bombs.Add(Bomb11 = ((SimpleBoss)Body).CreateChild(BombID, 1700, 400, false, livingConfig));
            Bombs.Add(Bomb12 = ((SimpleBoss)Body).CreateChild(BombID, 1700, 600, false, livingConfig));
        }

        private void BombRandomMove()
        {
            foreach (SimpleNpc bomb in Bombs)
            {
                bomb.MoveTo(Game.Random.Next(200, 1700), Game.Random.Next(200, 600), "fly", 1, 12);
            }
            Body.CallFuction(CreateBombBeatState, 3000);
        }

        private void CreateBombBeatState()
        {
            foreach (SimpleNpc bomb in Bombs)
            {
                ((PVEGame)Game).LivingChangeAngle(bomb, 45, 3, "beat");
                //bomb.PlayMovie("beat", 1, 1000);
            }
        }
		public override void OnStartAttacking()
		{
         switch(turnIndex)
            {
                case 0:
                    BombRandomMove();
                    turnIndex++;
                    break;
            }
		}
        private void ss()
        { }

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}