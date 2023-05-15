﻿using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Effects;

namespace GameServerScript.AI.Messions
{
    public class DKT4301 : AMissionControl
    {
        private List<SimpleNpc> npcs = new List<SimpleNpc>();

        private SimpleBoss m_boss = null;

        private SimpleBoss m_helper = null;

        private int npcID2 = 4301;

        private int npcID = 4303;

        private int bossID = 4304;

        private int m_bloodReduce = 2100;

        private int m_totalNpc = 3;

        private int m_kill = 0;

        public int CountKill
        {
            get { return m_kill; }
            set { m_kill = value; }
        }

        public SimpleBoss Helper
        {
            get { return m_helper; }
            set { m_helper = value; }
        }
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
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
            int[] resources = { bossID, npcID, npcID2 };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.AddLoadingFile(2, "image/game/effect/4/gate.swf", "game.asset.Gate");
            Game.SetMap(1142);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig config = Game.BaseLivingConfig();
            config.CanTakeDamage = false;

            m_boss = Game.CreateBoss(bossID, 1520, 350, -1, 0, 100, "", config);
            //Game.SendHideBlood(m_boss, 0);
            CreateHelper();
            Game.SendFreeFocus(1500, 250, 1, 2000, 3000);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            // check can create npc
            if (m_helper != null && m_helper.IsLiving && m_helper.X >= 600 && Game.FindAllNpcLiving().Length <= 0)
            {
                CreateNpc();
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (m_helper != null && m_helper.IsLiving == false)
            {
                Game.RemoveLiving(m_helper, true);
                CreateHelper();
            }

            if ((Game.CurrentTurnLiving is Player) && m_helper != null && m_helper.IsLiving && (int)m_boss.Properties1 == 1)
            {
                m_helper.PlayMovie("standB", 1200, 0);
            }

        }

        private void CreateHelper()
        {
            LivingConfig config = Game.BaseLivingConfig();
            config.IsHelper = true;

            m_helper = Game.CreateBoss(npcID2, 321, 746, 1, 0, 100, "", config);
            m_helper.AddEffect(new ContinueReduceBloodEffect(3, m_bloodReduce, m_helper), 0);
        }

        private void CreateNpc()
        {
            int countCanNext = 0;
            npcs = new List<SimpleNpc>();
            for (int i = 0; i < m_totalNpc; i++)
            {
                if (countCanNext > 0)
                {
                    int rand = Game.Random.Next(5, 110);
                    npcs.Add(Game.CreateNpc(npcID, m_helper.X - rand, m_helper.Y, 0));
                }
                else
                {
                    npcs.Add(Game.CreateNpc(npcID, m_helper.X - 20, m_helper.Y, 0));
                }
                countCanNext++;
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (m_kill >= Game.MissionInfo.TotalCount)
                return true;

            if (Game.TotalTurn > Game.MissionInfo.TotalTurn)
                return true;

            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return m_kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (m_kill >= Game.MissionInfo.TotalCount)
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
