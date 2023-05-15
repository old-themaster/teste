﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class CPN7104 : AMissionControl
    {
        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private PhysicalObj m_npc2;

        private SimpleBoss boss = null;

        private SimpleNpc npc = null;

        private int m_kill = 0;

        private int bossId = 7131; // chicken

        private int npcId = 7132; // long ga

        private int npcId2 = 7133; // trung thoi

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1150)
            {
                return 3;
            }
            else if (score > 925)
            {
                return 2;
            }
            else if (score > 700)
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
            Game.AddLoadingFile(1, "bombs/83.swf", "tank.resource.bombs.Bomb83");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.choudanbenbenAsset");
            Game.AddLoadingFile(2, "image/game/effect/7/choud.swf", "asset.game.seven.choud");
            Game.AddLoadingFile(2, "image/game/effect/7/jinqucd.swf", "asset.game.seven.jinqucd");
            Game.AddLoadingFile(2, "image/game/effect/7/du.swf", "asset.game.seven.du");
            int[] resources = { bossId, npcId, npcId2 };
            Game.LoadResources(resources);
            int[] gameOverResources = { bossId };
            Game.LoadNpcGameOverResources(gameOverResources);

            Game.SetMap(1164);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            m_kingMoive = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_kingFront = Game.Createlayer(300, 595, "font", "game.asset.living.choudanbenbenAsset", "out", 1, 0);

            // create npc2
            //Game.CreateNpc(npcId2, 2170, 636, 0, -1, config);
            m_npc2 = Game.Createlayer(2170, 636, "", "game.living.Living178", "stand", 1, 0);
            LivingConfig config = Game.BaseLivingConfig();
            config.IsTurn = false;
            config.IsFly = true;
            config.CanTakeDamage = false;

            npc = Game.CreateNpc(npcId, 1920, 900, 1, -1, config);

            npc.PlayMovie("stand", 1000, 0);
            npc.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhCageNpc.msg3"), 0, 4000);

            npc.CallFuction(new LivingCallBack(CreateBossEffect), 8000);
        }

        private void CreateBossEffect()
        {
            boss = Game.CreateBoss(bossId, 200, 590, 1, 1, "born");
            boss.SetRelateDemagemRect(boss.NpcInfo.X, boss.NpcInfo.Y, boss.NpcInfo.Width, boss.NpcInfo.Height);
            m_kingMoive.PlayMovie("in", 1000, 0);
            m_kingFront.PlayMovie("in", 2000, 0);
            m_kingMoive.PlayMovie("out", 5000, 0);
            m_kingFront.PlayMovie("out", 5400, 0);
            boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg8"), 0, 6000);
            boss.PlayMovie("skill", 8000, 0);
            boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg9"), 0, 8000);
            Game.SendObjectFocus(npc, 1, 9000, 0);
            npc.PlayMovie("standB", 10000, 0);
            npc.Config.CanTakeDamage = false;
            npc.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhCageNpc.msg5"), 0, 8000);
            Game.SendObjectFocus(boss, 1, 13000, 0);
            boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhSimpleSecondBoss.msg10"), 0, 14000, 3000);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (Game.TurnIndex > 1)
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
            if (boss != null && boss.IsLiving == false && npc != null && npc.IsLiving == false)
            {
                m_kill++;
                return true;
            }

            if (Game.TotalTurn > Game.MissionInfo.TotalTurn)
                return true;

            return false;
        }

        public override void OnDied()
        {
            base.OnDied();

            if (boss != null && boss.IsLiving == false && npc.IsLiving)
            {
                int waitTime = (int)Game.GetWaitTimerLeft();
                Game.SendObjectFocus(npc, 1, waitTime + 500, 500);
                npc.PlayMovie("out", waitTime + 1000, 0);
                npc.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhCageNpc.msg6"), 0, waitTime + 1500, 3500);
                npc.Config.CanTakeDamage = true;
            }
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return m_kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (boss != null && boss.IsLiving == false && npc != null && npc.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }
    }
}
    