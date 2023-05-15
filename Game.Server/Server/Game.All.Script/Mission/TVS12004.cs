using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace Game.Server.GameServerScript.AI.Messions
{
    public class TVS12004 : AMissionControl
    {
        private int int_0;

        private int ChickenFriendID = 12013;

        private int BossID1 = 12014;

        private int BossID2 = 12015;

        private int BossID3 = 12016;

        private int NpcID1 = 12017;

        private int NpcID2 = 12018;

        private int NpcID3 = 12020;

        private int NpcID4 = 12019;

        private SimpleNpc chickenFriend;

        private SimpleBoss Boss2;

        private SimpleBoss Boss1;

        private SimpleBoss EwNszFbhHut;

        private PhysicalObj physicalObj_0;

        private PhysicalObj physicalObj_1;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            if (score > 825)
            {
                return 2;
            }
            if (score <= 725)
            {
                return 0;
            }
            return 1;
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            return false;
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }
        public override void OnDied()
        {
            base.OnDied();
            if (Boss1 != null & !Boss1.IsLiving)
            {
                Game.ClearAllChild();
                Boss1.CallFuction(CreateSecondBoss, 6000);
            }
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            return;
            Game.IsWin = false;
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            Game.AddLoadingFile(2, "image/game/effect/9/daodan.swf", "asset.game.nine.daodan");
            Game.AddLoadingFile(2, "image/game/effect/9/diancipao.swf", "asset.game.nine.diancipao");
            Game.AddLoadingFile(2, "image/game/effect/9/fengyin.swf", "asset.game.nine.fengyin");
            Game.AddLoadingFile(2, "image/game/effect/9/siwang.swf", "asset.game.nine.siwang");
            Game.AddLoadingFile(2, "image/game/effect/9/shexian.swf", "asset.game.nine.shexian");
            Game.AddLoadingFile(2, "image/game/effect/9/biaoji.swf", "asset.game.nine.biaoji");
            Game.AddLoadingFile(2, "image/game/effect/9/heidong.swf", "asset.game.nine.heidong");
            int[] int1 = new int[] { BossID1, BossID2, BossID3, NpcID1, NpcID2, NpcID3, ChickenFriendID, NpcID4 };
            Game.LoadResources(int1);
            Game.LoadNpcGameOverResources(int1);
            Game.SetMap(1210);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            CreateChickenFriend();
            CreateFirstBoss();
            Game.SendFreeFocus(950, 300, 1, 1, 1);
        }
        private void CreateChickenFriend()
        {
            LivingConfig livingConfig = base.Game.BaseConfig();
            livingConfig.IsFly = true;
            livingConfig.CanTakeDamage = false;
            livingConfig.IsHelper = true;
            livingConfig.HasTurn = false;
            chickenFriend = Game.CreateNpc(ChickenFriendID, 219, 750, 1, 1, "", livingConfig);

        }
        private void CreateSecondBoss()
        {
            Game.SendFreeFocus(950, 300, 1, 1, 1);
            Boss1 = null;
            LivingConfig livingConfig = base.Game.BaseConfig();
            livingConfig.IsFly = true;
            livingConfig.CanCountKill = false;
            livingConfig.isBotom = 0;
            Boss2 = Game.CreateBoss(BossID2, 950, 300, -1, 1, "born", livingConfig);
        }
        private void CreateBoss2Npc()
        {

        }
        private void CreateFirstBoss()
        {
            LivingConfig livingConfig = base.Game.BaseConfig();
            livingConfig.IsFly = true;
            livingConfig.CanCountKill = false;
            livingConfig.isBotom = 0;
            Boss1 = Game.CreateBoss(BossID1, 950, 400, -1, 1, "born", livingConfig);
            Boss1.SetRelateDemagemRect(Boss1.NpcInfo.X, Boss1.NpcInfo.Y, Boss1.NpcInfo.Width, Boss1.NpcInfo.Height);
            //Boss1.PlayMovie("", 4000, 4000);
        }

        public override int UpdateUIData()
        {
            return base.UpdateUIData();
        }
    }
}