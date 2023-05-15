using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class TwelveTerrorFlyFristBoss : ABrain
	{
        private int turnIndex = 0;

        private int NpcID1 = 12317;

        private int NpcID2 = 12318;

        private int NpcID3 = 12320;

        private List<SimpleNpc> Bombs = new List<SimpleNpc>();

        private LayerTop boomEffect;

        private SimpleNpc BombLeft;

        private SimpleNpc BombCenterToLeft;

        private SimpleNpc BombCenterLeft;
        
        private SimpleNpc BombCenterRight;

        private SimpleNpc BombCenterToRight;

        private SimpleNpc BombRight;

        private void LaserAttack()
		{
            Body.CurrentDamagePlus = 1.5f;
            Body.PlayMovie("beatA", 1000, 3000);
            Body.CallFuction(EffectShexianAllPlayer, 3000);
            Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "", 3500, null);
            ((PVEGame)Game).SendFreeFocus(Body.X, 900, 1, 3000, 1);
		}
        private void EffectShexianAllPlayer()
        {
            foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
            {
                if (!allLivingPlayer.IsLiving)
                {
                    continue;
                }
                ((PVEGame)base.Game).Createlayer(allLivingPlayer.X, allLivingPlayer.Y, "", "asset.game.nine.shexian", "", 1, 1);
            }
        }
        
        private void BombCreate()
        {
            SimpleBoss body = ((SimpleBoss)Body);
            LivingConfig livingConfig = ((PVEGame)Game).BaseConfig();
            livingConfig.IsFly = true;
            livingConfig.CanCountKill = false;
            livingConfig.isShowBlood = false;
            livingConfig.isShowSmallMapPoint = false;
            livingConfig.HasTurn = false;
            BombLeft = body.CreateChild(NpcID3, 550, 320, -1, false, livingConfig);
            BombLeft.SetRect(100, -190, -20, 100);
            BombLeft.Properties1 = 5;
            BombCenterToLeft = body.CreateChild(NpcID1,758, 383, -1, false, livingConfig);
            BombCenterToLeft.SetRect(10, -165, -20, 100);
            BombCenterToLeft.Properties1 = 5;
            BombCenterLeft = body.CreateChild(NpcID2, 872, 383, -1, false, livingConfig);
            BombCenterLeft.SetRect(10, -165, -20, 100);
            BombCenterLeft.Properties1 = 5;
            BombCenterRight = body.CreateChild(NpcID2, 1045, 383, 1, false, livingConfig);
            BombCenterRight.SetRect(-10, -165, 20, 100);
            BombCenterRight.Properties1 = 5;
            BombCenterToRight = body.CreateChild(NpcID1, 1155, 383, 1, false, livingConfig);
            BombCenterToRight.SetRect(-10, -165, 20, 100);
            BombCenterToRight.Properties1 = 5;
            BombRight = body.CreateChild(NpcID3, 1371, 320, 1, false, livingConfig);
            BombRight.SetRect(-100, -190, 20, 100);
            BombRight.Properties1 = 5;
            //Bomb1 = ((PVEGame)Game).CreateBoss(NpcID3, 548, 320, -1, 0, "born", livingConfig);
            //Bomb2 = body.CreateChild(NpcID3, 1371, 320, 1, false, livingConfig);
            Body.CallFuction(ss, 3000);
        }
        private void BombAttack()
        {
            ((PVEGame)Game).SendFreeFocus(Body.X, 600, 1, 1, 1);
            if (BombLeft.IsLiving)
            {
                BombLeft.PlayMovie("beat", 1000, 1000);
                Bombs.Add(BombLeft);
            }
            if(BombCenterToLeft.IsLiving)
            {
                BombCenterToLeft.PlayMovie("beatA", 500, 1500);
                Bombs.Add(BombCenterToLeft);
            }
            if (BombCenterLeft.IsLiving)
            {
                BombCenterLeft.PlayMovie("beatA", 500, 1500);
                Bombs.Add(BombCenterLeft);
            }
            if (BombCenterRight.IsLiving)
            {
                BombCenterRight.PlayMovie("beatA", 500, 1500);
                Bombs.Add(BombCenterRight);
            }
            if (BombCenterToRight.IsLiving)
            {
                BombCenterToRight.PlayMovie("beatA", 500, 1500);
                Bombs.Add(BombCenterToRight);
            }
            if (BombRight.IsLiving)
            {
                BombRight.PlayMovie("beat", 1000, 1000);
                Bombs.Add(BombRight);
            }
            Body.CurrentDamagePlus = 10f * Bombs.Count;
            Body.CallFuction(BoomEffect, 2000);
            Body.CallFuction(RemoveAllObjects, 2500);
            Body.CallFuction(RemoveBombEffect, 2900);
        }
        
        private void BoomEffect()
        {
           boomEffect = ((PVEGame)Game).CreateLayerTop(500, 300, "top", "asset.game.nine.daodan", "", 1, 0);
        }
        
        private void RemoveBombEffect()
        {            
            Body.RangeAttacking(Body.X - 1000, Body.Y + 1000, "", 1, null);
            ((PVEGame)Game).RemovePhysicalObj(boomEffect, true);
        }
        private void RemoveAllObjects()
        {
            foreach (SimpleNpc bomb in Bombs)
            {
                bomb.Die();
                bomb.Dispose();
            }
            Bombs.Clear();
        }
        private void ss()
        {

        }
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
            switch(turnIndex)
            {
                case 0:
                    LaserAttack();
                    turnIndex++;
                    break;
                case 1:
                    if (Body.Blood < (Body.MaxBlood * 0.60))
                        BombCreate();
                    else
                        LaserAttack();
                    turnIndex++;
                    break;
                case 2:
                    if (((PVEGame)Game).GetNPCLivingWithProperties1(5).Count == 0)
                        LaserAttack();
                    else
                        BombAttack();
                    turnIndex = 0;
                    break;
            }
            
        }

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}