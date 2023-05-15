﻿using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class FiveTerrorThirdBoss : ABrain
    {
        private int m_attackTurn = 0;

        protected Player m_targer;

        private List<PhysicalObj> m_objects = new List<PhysicalObj>();

        private PhysicalObj m_effectTop;

        private SimpleNpc m_npc1;

        private SimpleNpc m_npc2;

        private SimpleNpc m_npc3;

        private int m_npcId1 = 5322;

        private int m_npcId2 = 5323;

        private int m_npcId3 = 5324;

        private int m_countForzenCanKill = 3;

        private int isSay = 0;

        private List<Point> m_pointCreateNpc2 = new List<Point> { new Point(1518, 699), new Point(500, 699) };

        private string[] PersonAttackSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg3")
        };

        private string[] CallNpc3Say =
        {
            "Quero torturar sem piedade <span class='red'>{0}</span>, sem piedade!", // GameServerScript.AI.NPC.FiveTerrorBossThird.msg4
            "<span class='red'>{0}</span>, já chegou a sua vez!", // GameServerScript.AI.NPC.FiveTerrorBossThird.msg5
            "Feche, quero fechar <span class='red'>{0}</span>!", // GameServerScript.AI.NPC.FiveTerrorBossThird.msg6
            "<span class='red'>{0}</span>, Veja isso", // GameServerScript.AI.NPC.FiveTerrorBossThird.msg7
            "Todo o mundo olhê, <span class='red'>{0}</span> já será executado." // GameServerScript.AI.NPC.FiveTerrorBossThird.msg8
        };

        private string[] GlobalAttackSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg9"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg10")
        };

        private string[] CallNpc1Say =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg11"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg12")
        };

        private string[] OnShootedChat =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg13"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg14"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg15")
        };

        private string[] KillPlayerChat =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg16"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg17"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FiveTerrorBossThird.msg18")
        };

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
            isSay = 0;

            foreach (PhysicalObj obj in m_objects)
            {
                Game.RemovePhysicalObj(obj, true);
            }

            m_objects = new List<PhysicalObj>();
        }

        public override void OnCreated()
        {
            base.OnCreated();
            Body.MaxBeatDis = 200;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();

            m_attackTurn++;
            switch (m_attackTurn)
            {
                case 1:
                    PersonAttack();
                    break;

                case 2:
                    if (m_npc2 != null && m_npc2.IsLiving)
                        PersonAttack();
                    else
                        CallNpc2Attack();
                    break;

                case 3:
                    CallGlobalAttack();
                    break;

                case 4:
                    if (m_npc3 != null && m_npc3.IsLiving)
                        PersonAttack();
                    else
                        CallNpc3Attack();
                    break;

                case 5:
                    if (m_npc1 != null && m_npc1.IsLiving)
                        CallGlobalAttack();
                    else
                        CallNpc1Attack();
                    m_attackTurn = 0;
                    break;
            }
        }

        private void CallGlobalAttack()
        {
            Body.ChangeDirection(-1, 500);
            Body.MoveTo(1000, 641, "fly", 1000, new LivingCallBack(RunGlobalAttack), 10);
        }

        private void RunGlobalAttack()
        {
            Body.CurrentDamagePlus = 2f;
            ((SimpleBoss)Body).RandomSay(GlobalAttackSay, 0, 1000, 0);
            Body.PlayMovie("beatA", 1200, 0);
            Body.CallFuction(new LivingCallBack(CreateGlobalAttackEffect), 2500);
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 4500, true);
            Body.PlayMovie("beatC", 4000, 2000);
        }

        private void CreateGlobalAttackEffect()
        {
            m_effectTop = ((PVEGame)Game).CreateLayerTop(500, 300, "", "asset.game.4.heip", "", 1, 1);
            Body.CallFuction(new LivingCallBack(RemoveGlobalAttackEffect), 2400);
        }

        private void RemoveGlobalAttackEffect()
        {
            if (m_effectTop != null)
                Game.RemovePhysicalObj(m_effectTop, true);
        }

        private void PersonAttack()
        {
            m_targer = Game.FindRandomPlayer();
            if (m_targer != null)
            {
                ((SimpleBoss)Body).RandomSay(PersonAttackSay, 0, 1000, 0);
                Body.MoveTo(m_targer.X, m_targer.Y - 100, "fly", 2000, new LivingCallBack(BeatE), 10);
            }
        }

        private void CallNpc1Attack()
        {
            ((SimpleBoss)Body).RandomSay(CallNpc1Say, 0, 1000, 0);
            Body.PlayMovie("beatD", 1200, 0);
            Body.CallFuction(new LivingCallBack(CreateNpc1), 3800);
        }

        private void CreateNpc1()
        {
            int randX = Game.Random.Next(440, 1570);
            LivingConfig config = ((PVEGame)Game).BaseLivingConfig();
            config.DamageForzen = true;
            config.CanTakeDamage = false;
            config.IsFly = true;
            m_npc1 = ((SimpleBoss)Body).CreateChild(m_npcId1, randX, 580, 0, 1, false, config);
            m_npc1.Properties2 = m_countForzenCanKill;
        }

        private void CallNpc3Attack()
        {
            m_targer = Game.FindRandomPlayer();
            if (m_targer != null)
            {
                Body.ChangeDirection(m_targer, 500);
                int randSay = Game.Random.Next(CallNpc3Say.Length);
                Body.Say(string.Format(CallNpc3Say[randSay], m_targer.PlayerDetail.PlayerCharacter.NickName), 0, 1000);
                Body.PlayMovie("beatD", 3000, 0);
                Body.CallFuction(new LivingCallBack(CreateNpc3), 5000);
            }
        }

        private void CreateNpc3()
        {
            LivingConfig config = ((PVEGame)Game).BaseLivingConfig();
            config.IsFly = true;
            m_npc3 = ((SimpleBoss)Body).CreateChild(m_npcId3, 114, 453, 0, 1, false, config);

            m_npc3.Properties1 = m_targer.Id;
            m_npc3.Properties2 = new Point(m_targer.X, m_targer.Y);

            int maxDelay = Game.GetHighDelayTurn();

            ((PVEGame)Game).SendObjectFocus(m_targer, 1, 1500, 0);

            Body.CallFuction(new LivingCallBack(CreateEffectMovePlayer), 2500);
            Body.CallFuction(new LivingCallBack(BlockAndHidePlayer), 3500);
            m_targer.BoltMove(m_npc3.X, m_npc3.Y, 3900);

            ((PVEGame)Game).SendObjectFocus(m_npc3, 1, 4000, 0);

            m_npc3.PlayMovie("in", 5000, 4000);

            ((PVEGame)Game).PveGameDelay = maxDelay + 1;
        }

        private void CallNpc2Attack()
        {
            m_targer = Game.FindRandomPlayer();
            if (m_targer != null)
            {
                Body.ChangeDirection(m_targer, 500);
                int randSay = Game.Random.Next(CallNpc3Say.Length);
                Body.Say(string.Format(CallNpc3Say[randSay], m_targer.PlayerDetail.PlayerCharacter.NickName), 0, 1000);
                Body.PlayMovie("beatD", 3000, 0);
                Body.CallFuction(new LivingCallBack(CreateNpc2), 5000);
            }
        }

        private void CreateNpc2()
        {
            int createPoint = Game.Random.Next(m_pointCreateNpc2.Count);
            m_npc2 = ((SimpleBoss)Body).CreateChild(m_npcId2, m_pointCreateNpc2[createPoint].X, m_pointCreateNpc2[createPoint].Y, 0, -1, true, ((PVEGame)Game).BaseLivingConfig());
            m_npc2.Properties1 = m_targer.Id;

            int maxDelay = Game.GetHighDelayTurn();

            ((PVEGame)Game).SendObjectFocus(m_targer, 1, 4000, 0);

            Body.CallFuction(new LivingCallBack(CreateEffectMovePlayer), 5000);
            Body.CallFuction(new LivingCallBack(BlockAndHidePlayer), 6000);
            m_targer.BoltMove(m_npc2.X, m_npc2.Y, 6100);

            ((PVEGame)Game).SendObjectFocus(m_npc2, 1, 6800, 0);

            m_npc2.PlayMovie("AtoB", 7500, 0);

            m_npc2.PlayMovie("beatA", 10000, 2000);

            Body.BeatDirect(m_targer, "", 11000, 1, 1);

            ((PVEGame)Game).PveGameDelay = maxDelay + 1;
        }

        private void CreateEffectMovePlayer()
        {
            m_objects.Add(((PVEGame)Game).Createlayer(m_targer.X, m_targer.Y, "", "asset.game.4.lanhuo", "", 1, 1));
        }

        private void BlockAndHidePlayer()
        {
            m_targer.SetVisible(false);
            m_targer.BlockTurn = true;
        }

        private void BeatE()
        {
            Body.Beat(m_targer, "beatE", 100, 1, 500, 1, 1);
            Body.CallFuction(new LivingCallBack(BackToDefaultPoint), 3500);
        }

        private void BackToDefaultPoint()
        {
            int randX = Game.Random.Next(376, 1643);
            int randY = Game.Random.Next(112, 593);
            Body.MoveTo(randX, randY, "fly", 500, 10);
        }

        public override void OnDie()
        {
            base.OnDie();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnAfterTakedBomb()
        {
            if (Body.IsLiving == false)
            {
                //Body.MoveTo(1000, 299, "fly", 1000, CreateEffectEndGame, 10);
            }
        }

        public override void OnShootedSay(int delay)
        {
            base.OnShootedSay(delay);
            int index = Game.Random.Next(0, OnShootedChat.Length);
            if (isSay == 0 && Body.IsLiving == true)
            {
                Body.Say(OnShootedChat[index], 0, 1000 + delay, 0);
                isSay = 1;
            }
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            ((SimpleBoss)Body).RandomSay(KillPlayerChat, 0, 0, 2000);
        }
    }
}
