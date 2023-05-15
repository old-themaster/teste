using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Bussiness;
using Game.Server.Rooms;

namespace GameServerScript.AI.Messions
{
    public class AC30004 : AMissionControl
    {
        private SimpleBoss boss = null;

        private int totalDamage = 0;

        private int bossID = 30004;

        private int kill = 0;

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
            int[] resources = { bossID };
            int[] gameOverResource = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.AddLoadingFile(1, "bombs/56.swf", "tank.resource.bombs.Bomb56");
            Game.AddLoadingFile(2, "image/bomb/bullet/bullet261.swf", "blastOutMovie261");
            Game.SetMap(1303);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            boss = Game.CreateBoss(bossID, 1350, -1500, -1, 1, "");
            boss.FallFrom(boss.X, boss.Y, "", 0, 1, 30, null);
            long worldBossBlood = RoomMgr.WorldBossRoom.Blood;
            if (worldBossBlood < int.MaxValue)
            {
                boss.Blood = (int)worldBossBlood;
            }
            boss.SetRelateDemagemRect(boss.NpcInfo.X, boss.NpcInfo.Y, boss.NpcInfo.Width, boss.NpcInfo.Height);
            boss.Say(LanguageMgr.GetTranslation("Futebol é meu esporte favorito...."), 0, 3000, 0);
            boss.AddDelay(3000);
        }

        public override void OnNewTurnStarted()   
        {
            base.OnNewTurnStarted();
            List<Player> players = Game.GetAllFightPlayers();
            foreach (Player player in players)
            {
                player.AfterKillingLiving += new KillLivingEventHanlde(OnTakedamage);
            }

        }
        public void OnTakedamage(Living living, Living target, int damageAmount, int criticalAmount)
        {
            if (boss == null)
                return;

            int total = damageAmount + criticalAmount;
            totalDamage += total;
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (boss != null && boss.IsLiving == false)
            {
                kill++;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            if (boss != null && boss.IsLiving == false)
            {
                RoomMgr.WorldBossRoom.FightOverAll();
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            List<Player> players = Game.GetAllFightPlayers();
            foreach (Player p in players)
            {
                RoomMgr.WorldBossRoom.ReduceBlood(p.TotalHurt);//totalDamage);
                int damageScore = p.TotalHurt / 100;
                int honor = p.TotalHurt / 1200;
                string name = p.PlayerDetail.PlayerCharacter.NickName;
                RoomMgr.WorldBossRoom.UpdateRank(damageScore, honor, name);
                p.PlayerDetail.AddDamageScores(damageScore);
                p.PlayerDetail.AddHonor(honor);
                p.PlayerDetail.SendMessage(string.Format("Este ataque ganha {0} Pontos e {1} pontos de honra.", damageScore, honor));

            }
        }
    }
}