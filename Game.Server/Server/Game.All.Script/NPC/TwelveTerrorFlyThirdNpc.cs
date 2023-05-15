using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class TwelveTerrorFlyThirdNpc : ABrain
    {
        private int m_attackTurn = 0;

        private PhysicalObj m_moive = null;
		
		private PhysicalObj moive = null;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            if (m_attackTurn == 0)
            {
                WalkBeat();
				m_attackTurn++;
            }
			else if (m_attackTurn == 1)
            {
                m_attackTurn++;
            }
			else if (m_attackTurn == 2)
            {
                Body.PlayMovie("beatA", 2000, 1000);
				Body.CurrentDamagePlus = 10;
				Body.RangeAttacking(Body.X - 50, Body.X + 50, "cry", 3000, null);
				Game.RemovePhysicalObj(moive, true);
                m_attackTurn++;
            }
			else if (m_attackTurn == 3)
            {
                WalkBeat();
				m_attackTurn++;
            }
			else if (m_attackTurn == 3)
            {
				m_attackTurn++;
            }
            else
            {
				Body.PlayMovie("beatA", 2000, 1000);
				Body.CurrentDamagePlus = 10;
				Body.RangeAttacking(Body.X - 50, Body.X + 50, "cry", 3000, null);
				Game.RemovePhysicalObj(moive, true);
                m_attackTurn = 0;
            }
        }

        private void WalkBeat()
        {
            int mtX = Game.Random.Next(100, 1800);
            int mtY = Game.Random.Next(400, 600);
			Body.MoveTo(mtX, mtY, "fly", 1000, "", 10, new LivingCallBack(PlayMovie));
        }
        
        private void PlayMovie()
        {
			Body.PlayMovie("beat", 4000, 1000);
			Body.CallFuction(new LivingCallBack(CallPhysicalObj), 4000);	
        }
		
		private void CallPhysicalObj()
        {
			moive = ((PVEGame)Game).Createlayer(Body.X, 910, "moive", "asset.game.nine.biaoji", "", 1, 0);
		}   
    }
}
