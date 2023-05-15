using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.Cmd
{
	[GameCommand(143, "BOT Turn")]
	public class BotCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (!(game is PVPGame))
			{
				return;
			}
			Player[] allPlayers = (game as PVPGame).GetAllPlayers();
			List<Player> list = new List<Player>();
			Player[] array = allPlayers;
			foreach (Player player2 in array)
			{
				if (player2.Team != player.Team)
				{
					list.Add(player2);
				}
			}
			int num = 0;
			int num2 = 0;
			if (new Random().Next(0, 10) < 7)
			{
				num = new Random().Next(80, 280);
			}
			int index = new Random().Next(0, list.Count);
			Player player3 = list.ElementAt(index);
			if (player3.X > player.X)
			{
				player.ChangeDirection(1, 500);
			}
			else
			{
				player.ChangeDirection(-1, 500);
			}
			int num3 = new Random().Next(0, 3);
			int num4 = 10001;
			int num5 = 10004;
			int num6 = 10004;
			int num7 = 0;
			int num8 = 0;
			int num9 = 1;
			float time = 1f;
			int num10 = 1;
			if (Math.Abs(player.X - player3.X) > 60)
			{
				if (num3 == 0)
				{
					num10 = 1;
					num4 = 10002;
					num5 = 10004;
					num6 = 10004;
					num7 = player3.X;
					num8 = player3.Y;
					num9 = 2;
				}
				else if (player3.X < player.X && player.X - player3.X > 200 && player.X - player3.X < 800)
				{
					num10 = 3;
					num4 = 10001;
					num5 = 10003;
					num6 = 10004;
					num7 = player3.X + 20;
					num8 = player3.Y;
					num9 = 3;
				}
				else if (Math.Abs(player.X - player3.X) > 1200)
				{
					num7 = ((player.X <= player3.X) ? (player3.X - 300) : (player3.X + 300));
					num10 = 1;
					num4 = 0;
					num5 = 10016;
					num6 = 10010;
					num8 = player3.Y - 100;
					num9 = 1;
				}
				else
				{
					num10 = 1;
					num4 = 10001;
					num5 = 10004;
					num6 = 10004;
					num7 = player3.X;
					num8 = player3.Y;
					num9 = 3;
				}
				if (num4 != 0)
				{
					ItemTemplateInfo item = ItemMgr.FindItemTemplate(num4);
					player.UseItem(item);
				}
				if (num5 != 0)
				{
					ItemTemplateInfo item2 = ItemMgr.FindItemTemplate(num5);
					player.UseItem(item2);
				}
				ItemTemplateInfo item3 = ItemMgr.FindItemTemplate(num6);
				player.UseItem(item3);
				time = ((Math.Abs(player.X - player3.X) < 200) ? 1f : ((Math.Abs(player.X - player3.X) < 400) ? 1.5f : ((Math.Abs(player.X - player3.X) < 700) ? 2f : ((Math.Abs(player.X - player3.X) < 1000) ? 2.5f : ((Math.Abs(player.X - player3.X) >= 1100) ? 3.5f : 3f)))));
			}
			else if (num3 != 0)
			{
				num10 = 1;
				num5 = 10010;
				num6 = 10016;
				num8 = player.Y;
				num9 = 1;
				time = 4f;
				num7 = ((player.X <= 700) ? (player.X + 600) : (player.X - 600));
				ItemTemplateInfo item2 = ItemMgr.FindItemTemplate(num5);
				player.UseItem(item2);
				ItemTemplateInfo item3 = ItemMgr.FindItemTemplate(num6);
				player.UseItem(item3);
			}
			else
			{
				num10 = 1;
				num4 = 10001;
				num5 = 10004;
				num6 = 10004;
				num7 = player3.X;
				num8 = player3.Y;
				num9 = 3;
				ItemTemplateInfo item2 = ItemMgr.FindItemTemplate(num4);
				player.UseItem(item2);
				ItemTemplateInfo item3 = ItemMgr.FindItemTemplate(num5);
				player.UseItem(item3);
				ItemTemplateInfo item4 = ItemMgr.FindItemTemplate(num6);
				player.UseItem(item4);
			}
			for (int j = 0; j < num9; j++)
			{
				player.ShootPoint(num7 + num, num8 + num2, player.CurrentBall.ID, 1001, 10001, num10, time, 2000);
			}
			if (player.IsAttacking)
			{
				player.StopAttacking();
			}
			GSPacketIn gSPacketIn = new GSPacketIn(91, player.Id);
			gSPacketIn.WriteByte(143);
			game.SendToAll(gSPacketIn);
		}
	}
}
