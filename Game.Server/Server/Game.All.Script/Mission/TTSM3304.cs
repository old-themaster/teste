﻿using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;
namespace GameServerScript.AI.Messions
{
    public class TTSM3304 : AMissionControl
    {
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 2045)
            {
                return 3;
            }
            else if (score > 2035)
            {
                return 2;
            }
            else if (score > 2025)
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
            Game.SetMap(1141);
        }
        public override void OnPrepareStartGame()
        {
            base.OnPrepareStartGame();
            Game.TotalTurn = Game.PlayerCount * 3;
            Game.SendMissionInfo();

        }


        public override void OnStartGame()
        {
            base.OnStartGame();


        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();


            #region 生成箱子
            List<ItemInfo> items = new List<ItemInfo>();
            for (int boxCount = 0; boxCount <= 80; boxCount++)
            {
                List<ItemInfo> infos = null;
                if (boxCount > 8)
                {
                    DropInventory.SpecialDrop(Game.MissionInfo.Id, 1, ref infos);  //黄箱（9~80）
                }
                else
                {
                    DropInventory.SpecialDrop(Game.MissionInfo.Id, 2, ref infos);  //红箱（0~8）
                }
                if (infos != null)
                {
                    foreach (ItemInfo info in infos)
                    {
                        items.Add(info);
                    }
                }
                else
                {
                    items.Add(null);
                }
            }
            #endregion

            //"1"黄色箱子、"2"红色箱子
            Game.CreateBox(450, 184, "2", items[0]);
            Game.CreateBox(550, 549, "2", items[1]);
            Game.CreateBox(650, 184, "2", items[2]);
            Game.CreateBox(750, 549, "2", items[3]);
            Game.CreateBox(850, 184, "2", items[4]);
            Game.CreateBox(950, 549, "2", items[5]);
            Game.CreateBox(1050, 184, "2", items[6]);
            Game.CreateBox(1150, 549, "2", items[7]);
            Game.CreateBox(1250, 184, "2", items[8]);

            Game.CreateBox(450, 234, "1", items[10]);
            Game.CreateBox(450, 285, "1", items[11]);
            Game.CreateBox(450, 335, "1", items[12]);
            Game.CreateBox(450, 394, "1", items[13]);
            Game.CreateBox(450, 444, "1", items[14]);
            Game.CreateBox(450, 499, "1", items[15]);
            Game.CreateBox(450, 549, "1", items[16]);

            Game.CreateBox(550, 184, "1", items[17]);
            Game.CreateBox(550, 234, "1", items[18]);
            Game.CreateBox(550, 285, "1", items[19]);
            Game.CreateBox(550, 335, "1", items[20]);
            Game.CreateBox(550, 394, "1", items[21]);
            Game.CreateBox(550, 444, "1", items[22]);
            Game.CreateBox(550, 499, "1", items[23]);

            Game.CreateBox(650, 234, "1", items[26]);
            Game.CreateBox(650, 285, "1", items[27]);
            Game.CreateBox(650, 335, "1", items[28]);
            Game.CreateBox(650, 394, "1", items[29]);
            Game.CreateBox(650, 444, "1", items[30]);
            Game.CreateBox(650, 499, "1", items[31]);
            Game.CreateBox(650, 549, "1", items[32]);

            Game.CreateBox(750, 184, "1", items[33]);
            Game.CreateBox(750, 234, "1", items[34]);
            Game.CreateBox(750, 285, "1", items[35]);
            Game.CreateBox(750, 335, "1", items[36]);
            Game.CreateBox(750, 394, "1", items[37]);
            Game.CreateBox(750, 444, "1", items[38]);
            Game.CreateBox(750, 499, "1", items[39]);

            Game.CreateBox(850, 234, "1", items[42]);
            Game.CreateBox(850, 285, "1", items[43]);
            Game.CreateBox(850, 335, "1", items[44]);
            Game.CreateBox(850, 394, "1", items[45]);
            Game.CreateBox(850, 444, "1", items[46]);
            Game.CreateBox(850, 499, "1", items[47]);
            Game.CreateBox(850, 549, "1", items[48]);

            Game.CreateBox(950, 184, "1", items[49]);
            Game.CreateBox(950, 234, "1", items[50]);
            Game.CreateBox(950, 285, "1", items[51]);
            Game.CreateBox(950, 335, "1", items[52]);
            Game.CreateBox(950, 394, "1", items[53]);
            Game.CreateBox(950, 444, "1", items[54]);
            Game.CreateBox(950, 499, "1", items[55]);

            Game.CreateBox(1050, 234, "1", items[58]);
            Game.CreateBox(1050, 285, "1", items[59]);
            Game.CreateBox(1050, 335, "1", items[60]);
            Game.CreateBox(1050, 394, "1", items[61]);
            Game.CreateBox(1050, 444, "1", items[62]);
            Game.CreateBox(1050, 499, "1", items[63]);
            Game.CreateBox(1050, 549, "1", items[64]);

            Game.CreateBox(1150, 184, "1", items[65]);
            Game.CreateBox(1150, 234, "1", items[66]);
            Game.CreateBox(1150, 285, "1", items[67]);
            Game.CreateBox(1150, 335, "1", items[68]);
            Game.CreateBox(1150, 394, "1", items[69]);
            Game.CreateBox(1150, 444, "1", items[70]);
            Game.CreateBox(1150, 499, "1", items[71]);

            Game.CreateBox(1250, 234, "1", items[74]);
            Game.CreateBox(1250, 285, "1", items[75]);
            Game.CreateBox(1250, 335, "1", items[76]);
            Game.CreateBox(1250, 394, "1", items[77]);
            Game.CreateBox(1250, 444, "1", items[78]);
            Game.CreateBox(1250, 499, "1", items[79]);
            Game.CreateBox(1250, 549, "1", items[80]);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();

            ((Player)Game.CurrentLiving).Seal((Player)Game.CurrentLiving, 0, 0);

        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            ((Player)Game.CurrentLiving).SetBall(3);

        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            return Game.TurnIndex > Game.TotalTurn - 1;
        }

        public override int UpdateUIData()
        {
            return base.UpdateUIData();
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            Game.IsWin = true;

            foreach (Player player in Game.GetAllFightPlayers())
            {
                player.OffSeal(player, 0);
            }

            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/show5.jpg", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
