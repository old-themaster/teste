using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
namespace Game.Server.Packets.Client
{
	[PacketHandler(84, "场景用户离开")]
	public class ActivityPackageHandler : IPacketHandler
	{
		private readonly int[] m_gradeList = new int[]
		{
			10,
			20,
			30,
			40,
			45,
			50,
			55,
			60,
			65
		};
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			int arg_17_0 = client.Player.PlayerCharacter.ID;
			ActiveSystemInfo info = client.Player.Actives.Info;
			if (num == 1)
			{
				int num2 = num;
				if (num2 == 1)
				{
					int availTime = info.AvailTime;
					if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
					{
						client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked", new object[0]));
						return 0;
					}
					if (DateTime.Compare(client.Player.LastOpenGrowthPackage.AddMilliseconds(500.0), DateTime.Now) > 0)
					{
						client.Player.Out.SendGrowthPackageUpadte(client.Player.PlayerCharacter.ID, info.AvailTime);
						return 0;
					}
					int promotePackagePrice = GameProperties.PromotePackagePrice;
					if (client.Player.MoneyDirect(promotePackagePrice))
					{
						string translation = LanguageMgr.GetTranslation("UserBuyItemHandler.Success", new object[0]);
						if (this.CanGetAward(client.Player.PlayerCharacter.Grade, availTime))
						{
							info.AvailTime++;
							client.Player.Out.SendGrowthPackageUpadte(client.Player.PlayerCharacter.ID, info.AvailTime);
							int num3 = client.Player.PlayerCharacter.Sex ? 1 : 2;
							List<ActivitySystemItemInfo> list = ActiveSystemMgr.FindGrowthPackage(availTime);
							List<ItemInfo> list2 = new List<ItemInfo>();
							foreach (ActivitySystemItemInfo current in list)
							{
								ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(current.TemplateID);
								if (itemTemplateInfo != null && (itemTemplateInfo.NeedSex == 0 || itemTemplateInfo.NeedSex == num3))
								{
									ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, current.Count, 102);
									itemInfo.Count = current.Count;
									itemInfo.IsBinds = current.IsBinds;
									itemInfo.ValidDate = current.ValidDate;
									itemInfo.StrengthenLevel = current.StrengthenLevel;
									itemInfo.AttackCompose = current.AttackCompose;
									itemInfo.DefendCompose = current.DefendCompose;
									itemInfo.AgilityCompose = current.AgilityCompose;
									itemInfo.LuckCompose = current.LuckCompose;
									list2.Add(itemInfo);
								}
							}
							if (list2.Count > 0)
							{
								WorldEventMgr.SendItemsToMail(list2, client.Player.PlayerCharacter.ID, client.Player.PlayerCharacter.NickName, string.Format(" Presente de nível  {0} ", this.m_gradeList[availTime]));
							}
							client.Out.SendMessage(eMessageType.Normal, translation);
						}
						else
						{
							client.Player.Out.SendGrowthPackageUpadte(client.Player.PlayerCharacter.ID, info.AvailTime);
							client.Out.SendMessage(eMessageType.Normal, "Nível insuficiente, falha na operação.");
						}
					}
					client.Player.LastOpenGrowthPackage = DateTime.Now;
				}
			}
			return 0;
		}
		private bool CanGetAward(int grace, int index)
		{
			switch (index)
			{
				case 0:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 1:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 2:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 3:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 4:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 5:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 6:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 7:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
				case 8:
					if (this.m_gradeList[index] <= grace)
					{
						return true;
					}
					break;
			}
			return false;
		}
	}
}
