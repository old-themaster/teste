using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class TwelveTerrorGunBoss : ABrain
	{
        int cannonball = 12307;

        private int BossID = 12308;

        public override void OnBeginNewTurn()
		{
		}

		public override void OnBeginSelfTurn()
		{
		}

		public override void OnCreated()
		{
		}

		public override void OnStartAttacking()
		{
            if (((PVEGame)Game).GetNPCLivingWithID(cannonball).Length == 0 && Body.ShootCount > 0)
            {
                Body.CurrentDamagePlus = 2f;
                ((PVEGame)Game).SendFreeFocus(Body.X, Body.Y, 1, 1, 1);
                SimpleBoss boss = ((PVEGame)Game).FindBossWithID(BossID);
                boss.Properties3 = int.Parse(boss.Properties3.ToString()) + Body.ShootCount;
                for(int i = 0; i < Body.ShootCount; i++)
                {
                    if (base.Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 4000))
                    {
                        Body.Say("Огонь!", 0, 3000);
                        base.Body.PlayMovie("beatA", 1000, 6000);
                    }
                }
                Game.ClearAllChild();
                Body.ShootCount = 0;
                /*switch (Body.ShootCount)
                {
                    case 1:
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 4000);
                        break;
                    case 2:
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 4000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 6000);
                        break;
                    case 3:
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 4000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 6000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 7000);
                        break;
                    case 4:
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 4000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 6000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 7000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 8000);
                        break;
                    case 5:
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 4000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 6000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 7000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 8000);
                        Body.ShootPoint(boss.X, boss.Y - 125, 88, 1000, 10000, 1, 2f, 9000);
                        break;
                }*/
            }
        }

		public override void OnStopAttacking()
		{
		}
	}
}