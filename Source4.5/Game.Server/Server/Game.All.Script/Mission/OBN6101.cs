﻿using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class OBN6101 : AMissionControl
    {
        private SimpleBoss m_boss;

        private int bossID = 6321;

        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private PhysicalObj m_leftPoint;

        private PhysicalObj m_rightPoint;

        private PhysicalObj m_leftPoint1;

        private PhysicalObj m_leftPoint2;

        private PhysicalObj m_rightPoint1;

        private PhysicalObj m_rightPoint2;

        private PhysicalObj a1;

        private PhysicalObj a2;

        private PhysicalObj a3;

        private PhysicalObj a4;

        private PhysicalObj a5;

        private PhysicalObj a6;

        private PhysicalObj t1;

        private PhysicalObj t2;

        private PhysicalObj t3;

        private PhysicalObj t4;

        private PhysicalObj t5;

        private PhysicalObj t6;

        private int turn = 0;

        private int[] birthX = { 450, 550, 650, 750, 850, 950, 1050, 1150, 1250, 455, 555, 655, 755, 855, 955, 1055, 1155, 1255 };//Toa do X

        private int[] birthY = { 184, 259, 335, 420, 504 };//Toa do Y

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            else if (score > 825)
            {
                return 2;
            }
            else if (score > 725)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.AddLoadingFile(2, "image/game/thing/bossborn6.swf", "game.asset.living.GuizeAsset");
            Game.AddLoadingFile(2, "image/game/effect/6/ball.swf", "asset.game.six.ball");
            Game.AddLoadingFile(2, "image/game/effect/6/jifenpai.swf", "asset.game.six.fenshu");
            Game.AddLoadingFile(2, "image/game/effect/6/jifenpai.swf", "asset.game.six.shuzi");
            Game.SetMap(1165);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

            LivingConfig config = Game.BaseLivingConfig();
            config.CanTakeDamage = false;
            config.IsHelper = true;
            config.IsTurn = false;

            Game.TotalCount = 99;
            Game.TotalTurn = Game.PlayerCount * 10;
            Game.SendMissionInfo();

            m_leftPoint = Game.Createlayer(100, 669, "movie", "asset.game.six.fenshu", "Z", 1, 1);
            m_rightPoint = Game.Createlayer(1411, 669, "movie", "asset.game.six.fenshu", "Z", 1, 1);

            m_leftPoint1 = Game.Createlayer(185, 669, "movie", "asset.game.six.shuzi", "z0", 1, 1);
            m_leftPoint2 = Game.Createlayer(255, 669, "movie", "asset.game.six.shuzi", "z0", 1, 1);

            m_rightPoint1 = Game.Createlayer(1575, 669, "movie", "asset.game.six.shuzi", "z0", 1, 1);
            m_rightPoint2 = Game.Createlayer(1647, 669, "movie", "asset.game.six.shuzi", "z0", 1, 1);

            m_boss = Game.CreateBoss(bossID, 345, 860, -1, 10, 0, "", config);

            m_boss.PlayMovie("standC", 0, 0);
            m_boss.PlayMovie("go", 1000, 0);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.6101.Mgs1"), 0, 1000);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.6101.Mgs2"), 0, 4000);

            m_boss.Die(7000);

            m_boss.CallFuction(new LivingCallBack(NextAttack2), 8000);
            m_boss.CallFuction(new LivingCallBack(NextAttack), 9000);
        }

        private void NextAttack2()
        {
            m_kingMoive = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 0);

            ((PVEGame)Game).SendGameFocus(900, 500, 1, 0, 1000);

            m_kingMoive.PlayMovie("in", 0, 0);
            m_kingMoive.PlayMovie("out", 5000, 0);

            m_boss.CallFuction(new LivingCallBack(CreatBall), 6000);
        }

        private void NextAttack()
        {
            m_kingFront = Game.Createlayer(900, 450, "font", "game.asset.living.GuizeAsset", "out", 1, 0);
        }

        private void CreatBall()
        {
            int num1 = this.Game.Random.Next(-2, 6);
            int num2 = this.Game.Random.Next(-2, 6);
            int num3 = this.Game.Random.Next(-2, 6);
            int num4 = this.Game.Random.Next(-3, 6);
            int num5 = this.Game.Random.Next(-1, 6);
            int num6 = this.Game.Random.Next(-1, 6);
            int num7 = this.Game.Random.Next(-1, 6);
            int num8 = this.Game.Random.Next(-4, 6);
            int num9 = this.Game.Random.Next(1, 6);
            int num10 = this.Game.Random.Next(1, 6);
            int num11 = this.Game.Random.Next(1, 6);
            int num12 = this.Game.Random.Next(-6, 6);

            a1 = Game.CreateBall(850, 300, "shield" + num1, "s-" + num1, 1, 0);
            a2 = Game.CreateBall(750, 400, "shield" + num2, "s-" + num2, 1, 0);
            a3 = Game.CreateBall(650, 300, "shield" + num3, "s-" + num3, 1, 0);
            a4 = Game.CreateBall(950, 400, "shield" + num4, "s-" + num4, 1, 0);
            a5 = Game.CreateBall(1050, 300, "shield" + num5, "s-" + num5, 1, 0);
            a6 = Game.CreateBall(1150, 400, "shield" + num6, "s-" + num6, 1, 0);

            t1 = Game.CreateBall(850, 500, "shield" + num7, "s" + num7, 1, 0);
            t2 = Game.CreateBall(750, 600, "shield" + num8, "s" + num8, 1, 0);
            t3 = Game.CreateBall(650, 500, "shield" + num9, "s" + num9, 1, 0);
            t4 = Game.CreateBall(950, 600, "shield" + num10, "s" + num10, 1, 0);
            t5 = Game.CreateBall(1050, 500, "shield" + num11, "s" + num11, 1, 0);
            t6 = Game.CreateBall(1150, 600, "shield" + num12, "s" + num12, 1, 0);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            Game.RemovePhysicalObj(a1, true);
            Game.RemovePhysicalObj(a2, true);
            Game.RemovePhysicalObj(a3, true);
            Game.RemovePhysicalObj(a4, true);
            Game.RemovePhysicalObj(a5, true);
            Game.RemovePhysicalObj(a6, true);
            Game.RemovePhysicalObj(t1, true);
            Game.RemovePhysicalObj(t2, true);
            Game.RemovePhysicalObj(t3, true);
            Game.RemovePhysicalObj(t4, true);
            Game.RemovePhysicalObj(t5, true);
            Game.RemovePhysicalObj(t6, true);

            if (Game.TotalKillCount < 10)
            {
                m_leftPoint1.PlayMovie("z" + 0, 0, 0);
                m_rightPoint1.PlayMovie("z" + 0, 0, 0);

                m_leftPoint2.PlayMovie("z" + Game.TotalKillCount, 0, 0);
                m_rightPoint2.PlayMovie("z" + Game.TotalKillCount, 0, 0);
            }
            else
            {
                m_leftPoint1.PlayMovie("z" + Game.TotalKillCount.ToString()[0], 0, 0);
                m_rightPoint1.PlayMovie("z" + Game.TotalKillCount.ToString()[0], 0, 0);

                m_leftPoint2.PlayMovie("z" + Game.TotalKillCount.ToString()[1], 0, 0);
                m_rightPoint2.PlayMovie("z" + Game.TotalKillCount.ToString()[1], 0, 0);
            }
            m_boss.CallFuction(new LivingCallBack(CreatBall), 500);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (Game.TurnIndex > turn + 1)
            {
                if (m_kingMoive != null)
                {
                    Game.RemovePhysicalObj(m_kingMoive, true);
                    m_kingMoive = null;
                }
                if (m_kingFront != null)
                {
                    Game.RemovePhysicalObj(m_kingFront, true);
                    m_kingFront = null;
                }
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (Game.TotalKillCount >= 99)
            {
                Game.TotalKillCount = 99;
                return true;
            }

            if (Game.TotalTurn > Game.PlayerCount * 10)
            {
                return false;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            return Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (Game.TotalKillCount >= 99)
            {
                Game.IsWin = true;
            }
            else if (Game.TotalTurn > Game.PlayerCount * 10)
            {
                Game.IsWin = false;
            }
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
            if (Game.TotalKillCount >= 99)
            {
                Game.IsWin = true;
            }
            else if (Game.TotalTurn > Game.PlayerCount * 10)
            {
                Game.IsWin = false;
            }
        }
    }
}
