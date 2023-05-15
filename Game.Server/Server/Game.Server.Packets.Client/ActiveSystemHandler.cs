using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Buffer;
using Game.Server.Managers;
using Game.Server.Rooms;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Game.Server.Packets.Client
{
	[PacketHandler(145, "场景用户离开")]
	public class ActiveSystemHandler : IPacketHandler
	{
		public static ThreadSafeRandom random = new ThreadSafeRandom();
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			byte b = packet.ReadByte();
			GSPacketIn gSPacketIn = new GSPacketIn(145, client.Player.PlayerCharacter.ID);
			BaseChristmasRoom christmasRoom = RoomMgr.ChristmasRoom;
			UserChristmasInfo christmas = client.Player.Actives.Christmas;
			bool flag = false;
			if (b <= 10)
			{
				flag = client.Player.Actives.LoadPyramid();
			}
			PyramidInfo pyramid = client.Player.Actives.Pyramid;
			UserBoguAdventureInfo boguAdventure = client.Player.Actives.BoguAdventure;
			string title = "Evento Noel";
			byte b2 = b;
			if (b2 <= 49)
			{
				switch (b2)
				{
					case 1:
						if (flag && pyramid != null)
						{
							gSPacketIn.WriteByte(1);
							gSPacketIn.WriteInt(pyramid.currentLayer);
							gSPacketIn.WriteInt(pyramid.maxLayer);
							gSPacketIn.WriteInt(pyramid.totalPoint);
							gSPacketIn.WriteInt(pyramid.turnPoint);
							gSPacketIn.WriteInt(pyramid.pointRatio);
							gSPacketIn.WriteInt(pyramid.currentFreeCount);
							gSPacketIn.WriteInt(pyramid.currentReviveCount);
							gSPacketIn.WriteBoolean(pyramid.isPyramidStart);
							if (pyramid.isPyramidStart)
							{
								string[] lists = pyramid.LayerItems.Split(new char[]
								{
								'|'
								});
								int[] array = new int[]
								{
								8,
								7,
								6,
								5,
								4,
								3,
								2
								};
								gSPacketIn.WriteInt(array.Length);
								for (int i = 1; i <= array.Length; i++)
								{
									string[] layerItems = this.GetLayerItems(lists, i);
									gSPacketIn.WriteInt(i);
									gSPacketIn.WriteInt(layerItems.Length);
									for (int j = 0; j < layerItems.Length; j++)
									{
										int val = int.Parse(layerItems[j].Split(new char[]
										{
										'-'
										})[1]);
										int val2 = int.Parse(layerItems[j].Split(new char[]
										{
										'-'
										})[2]);
										gSPacketIn.WriteInt(val);
										gSPacketIn.WriteInt(val2);
									}
								}
							}
							client.Player.SendTCP(gSPacketIn);
							return 0;
						}
						client.Player.SendMessage("Download falhou. Por favor, tente novamente mais tarde.");
						return 0;
					case 2:
						{
							bool flag2 = packet.ReadBoolean();
							pyramid.isPyramidStart = flag2;
							gSPacketIn.WriteByte(2);
							gSPacketIn.WriteBoolean(flag2);
							if (!flag2)
							{
								pyramid.totalPoint += pyramid.totalPoint * pyramid.pointRatio / 100;
								pyramid.totalPoint += pyramid.turnPoint;
								pyramid.turnPoint = 0;
								pyramid.pointRatio = 0;
								pyramid.currentLayer = 1;
								pyramid.currentReviveCount = 0;
								pyramid.LayerItems = "";
								gSPacketIn.WriteInt(pyramid.totalPoint);
								gSPacketIn.WriteInt(pyramid.turnPoint);
								gSPacketIn.WriteInt(pyramid.pointRatio);
								gSPacketIn.WriteInt(pyramid.currentLayer);
							}
							client.Player.SendTCP(gSPacketIn);
							return 0;
						}
					case 3:
						{
							int num = packet.ReadInt();
							int num2 = packet.ReadInt();
							if (pyramid.currentFreeCount < client.Player.Actives.PyramidConfig.freeCount)
							{
								pyramid.currentFreeCount++;
							}
							else
							{
								int turnCardPrice = client.Player.Actives.PyramidConfig.turnCardPrice;
								if (!client.Player.MoneyDirect(turnCardPrice))
								{
									return 1;
								}
							}
							bool flag3 = true;
							string msg;
							if (num < 8)
							{
								List<ItemInfo> pyramidAward = ActiveSystemMgr.GetPyramidAward(num);
								int index = ActiveSystemHandler.random.Next(pyramidAward.Count);
								ItemInfo itemInfo = pyramidAward[index];
								int templateID = itemInfo.TemplateID;
								bool val3 = templateID == 201083;
								bool flag4 = templateID == 201082;
								msg = string.Format("Você Ganhou {0} x{1}.", itemInfo.Template.Name, itemInfo.Count);
								if (flag4)
								{
									msg = "Felizmente você chega ao próximo andar.";
									pyramid.currentLayer++;
									if (pyramid.currentLayer > pyramid.maxLayer)
									{
										pyramid.maxLayer++;
									}
									flag3 = false;
								}
								pyramid.totalPoint += 10;
								switch (templateID)
								{
									case 201077:
										pyramid.pointRatio += 5;
										msg = "Felizmente, você obtém 5 % a mais de pontos acumulados.";
										flag3 = false;
										break;
									case 201078:
										pyramid.pointRatio += 10;
										msg = "Felizmente, você obtém 10 % a mais de pontos acumulados.";
										flag3 = false;
										break;
									case 201079:
										pyramid.turnPoint += 20;
										msg = "Felizmente, você obtém 20 % a mais de pontos acumulados.";
										flag3 = false;
										break;
									case 201080:
										pyramid.turnPoint += 30;
										msg = "Felizmente, você obtém 30 % a mais de pontos acumulados.";
										flag3 = false;
										break;
									case 201081:
										pyramid.turnPoint += 40;
										msg = "Felizmente, você obtém 40 % a mais de pontos acumulados.";
										flag3 = false;
										break;
								}
								if (flag3)
								{
									client.Player.AddTemplate(itemInfo, "Pirâmide");
								}
								string text = string.Format("{0}-{1}-{2}", num, templateID, num2);
								if (pyramid.LayerItems == "")
								{
									pyramid.LayerItems = text;
								}
								else
								{
									PyramidInfo expr_799 = pyramid;
									expr_799.LayerItems = expr_799.LayerItems + "|" + text;
								}
								gSPacketIn.WriteByte(3);
								gSPacketIn.WriteInt(templateID);
								gSPacketIn.WriteInt(num2);
								gSPacketIn.WriteBoolean(val3);
								gSPacketIn.WriteBoolean(flag4);
								gSPacketIn.WriteInt(pyramid.currentLayer);
								gSPacketIn.WriteInt(pyramid.maxLayer);
								gSPacketIn.WriteInt(pyramid.totalPoint);
								gSPacketIn.WriteInt(pyramid.turnPoint);
								gSPacketIn.WriteInt(pyramid.pointRatio);
								gSPacketIn.WriteInt(pyramid.currentFreeCount);
								client.Player.SendTCP(gSPacketIn);
							}
							else
							{
								int num3 = ActiveSystemHandler.random.Next(49, 501);
								pyramid.turnPoint += num3;
								msg = string.Format("Felizmente você consegue mais {0} pontos cumulativos.", num3);
								pyramid.isPyramidStart = false;
								pyramid.currentLayer = 1;
								pyramid.currentReviveCount = 0;
								pyramid.totalPoint += pyramid.totalPoint * pyramid.pointRatio / 100;
								pyramid.totalPoint += pyramid.turnPoint;
								pyramid.turnPoint = 0;
								pyramid.pointRatio = 0;
								gSPacketIn.WriteByte(2);
								gSPacketIn.WriteBoolean(pyramid.isPyramidStart);
								gSPacketIn.WriteInt(pyramid.totalPoint);
								gSPacketIn.WriteInt(pyramid.turnPoint);
								gSPacketIn.WriteInt(pyramid.pointRatio);
								gSPacketIn.WriteInt(pyramid.currentLayer);
								client.Player.SendTCP(gSPacketIn);
							}
							client.Player.SendMessage(msg);
							return 0;
						}
					case 4:
						{
							bool flag5 = packet.ReadBoolean();
							if (!flag5)
							{
								pyramid.isPyramidStart = false;
								pyramid.currentLayer = 1;
								pyramid.currentReviveCount = 0;
								pyramid.totalPoint += pyramid.totalPoint * pyramid.pointRatio / 100;
								pyramid.totalPoint += pyramid.turnPoint;
								pyramid.turnPoint = 0;
								pyramid.pointRatio = 0;
								pyramid.LayerItems = "";
								gSPacketIn.WriteByte(4);
								gSPacketIn.WriteBoolean(pyramid.isPyramidStart);
								gSPacketIn.WriteInt(pyramid.currentLayer);
								gSPacketIn.WriteInt(pyramid.totalPoint);
								gSPacketIn.WriteInt(pyramid.turnPoint);
								gSPacketIn.WriteInt(pyramid.pointRatio);
								gSPacketIn.WriteInt(pyramid.currentReviveCount);
								client.Player.SendTCP(gSPacketIn);
								return 0;
							}
							int value = client.Player.Actives.PyramidConfig.revivePrice[pyramid.currentReviveCount];
							if (!client.Player.MoneyDirect(value))
							{
								return 1;
							}
							pyramid.currentReviveCount++;
							gSPacketIn.WriteByte(4);
							gSPacketIn.WriteBoolean(pyramid.isPyramidStart);
							gSPacketIn.WriteInt(pyramid.currentLayer);
							gSPacketIn.WriteInt(pyramid.totalPoint);
							gSPacketIn.WriteInt(pyramid.turnPoint);
							gSPacketIn.WriteInt(pyramid.pointRatio);
							gSPacketIn.WriteInt(pyramid.currentReviveCount);
							client.Player.SendTCP(gSPacketIn);
							return 0;
						}
					case 5:
					case 6:
					case 7:
						break;
					case 8:
						gSPacketIn.WriteByte(8);
						gSPacketIn.WriteInt(1);
						return 0;
					default:
						switch (b2)
						{
							case 17:
								{
									byte b3 = packet.ReadByte();
									if (b3 == 2)
									{
										int x = packet.ReadInt();
										int y = packet.ReadInt();
										client.Player.X = x;
										client.Player.Y = y;
										if (client.Player.CurrentRoom != null)
										{
											client.Player.CurrentRoom.RemovePlayerUnsafe(client.Player);
										}
										christmasRoom.AddMoreMonters();
										christmasRoom.SetMonterDie(client.Player.PlayerCharacter.ID);
										if (!client.Player.Actives.AvailTime())
										{
											client.Player.SendMessage("Tempo limite, você tem que pagar para continuar.");
											return 0;
										}
										gSPacketIn.WriteByte(22);
										gSPacketIn.WriteByte(0);
										gSPacketIn.WriteInt(christmasRoom.Monters.Count);
										foreach (MonterInfo current in christmasRoom.Monters.Values)
										{
											gSPacketIn.WriteInt(current.ID);
											gSPacketIn.WriteInt(current.type);
											gSPacketIn.WriteInt(current.state);
											gSPacketIn.WriteInt(current.MonsterPos.X);
											gSPacketIn.WriteInt(current.MonsterPos.Y);
										}
										client.Out.SendTCP(gSPacketIn);
										christmasRoom.ViewOtherPlayerRoom(client.Player);
										return 0;
									}
									else
									{
										if (b3 == 0)
										{
											client.Player.X = christmasRoom.DefaultPosX;
											client.Player.Y = christmasRoom.DefaultPosY;
											christmasRoom.AddPlayer(client.Player);
											int num4 = GameProperties.ChristmasMinute;
											if (!christmas.isEnter)
											{
												christmas.gameBeginTime = DateTime.Now;
												christmas.gameEndTime = DateTime.Now.AddMinutes((double)num4);
												christmas.isEnter = true;
												christmas.AvailTime = num4;
											}
											else
											{
												num4 = christmas.AvailTime;
												christmas.gameBeginTime = DateTime.Now;
												christmas.gameEndTime = DateTime.Now.AddMinutes((double)num4);
											}
											bool val4 = client.Player.Actives.AvailTime();
											gSPacketIn.WriteByte(17);
											gSPacketIn.WriteBoolean(val4);
											gSPacketIn.WriteDateTime(christmas.gameBeginTime);
											gSPacketIn.WriteDateTime(christmas.gameEndTime);
											gSPacketIn.WriteInt(christmas.count);
											client.Out.SendTCP(gSPacketIn);
											return 0;
										}
										if (b3 == 1)
										{
											christmasRoom.RemovePlayer(client.Player);
											return 0;
										}
										return 0;
									}
									break;
								}
							case 21:
								{
									int num5 = packet.ReadInt();
									int num6 = packet.ReadInt();
									string str = packet.ReadString();
									client.Player.X = num5;
									client.Player.Y = num6;
									gSPacketIn.WriteByte(21);
									gSPacketIn.WriteInt(client.Player.PlayerId);
									gSPacketIn.WriteInt(num5);
									gSPacketIn.WriteInt(num6);
									gSPacketIn.WriteString(str);
									christmasRoom.SendToALL(gSPacketIn);
									return 0;
								}
							case 22:
								{
									int num7 = packet.ReadInt();
									if (client.Player.MainWeapon == null)
									{
										client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip", new object[0]));
										return 0;
									}
									if (!client.Player.Actives.AvailTime())
									{
										client.Player.SendMessage("Tempo limite, você tem que pagar para continuar.");
										return 0;
									}
									bool flag6 = christmasRoom.SetFightMonter(num7, client.Player.PlayerCharacter.ID);
									if (flag6 && christmasRoom.Monters.ContainsKey(num7))
									{
										MonterInfo monterInfo = christmasRoom.Monters[num7];
										gSPacketIn.WriteByte(22);
										gSPacketIn.WriteByte(3);
										gSPacketIn.WriteInt(num7);
										gSPacketIn.WriteInt(monterInfo.state);
										christmasRoom.SendToALL(gSPacketIn);
										RoomMgr.CreateRoom(client.Player, "Christmas", "Christmas", eRoomType.Christmas, 3);
										return 0;
									}
									return 0;
								}
							case 24:
								gSPacketIn.WriteByte(24);
								gSPacketIn.WriteInt(christmas.count);
								gSPacketIn.WriteInt(christmas.exp);
								gSPacketIn.WriteInt(christmas.awardState);
								gSPacketIn.WriteInt(christmas.packsNumber);
								client.Out.SendTCP(gSPacketIn);
								return 0;
							case 25:
								{
									int templateId = 201144;
									int num8 = packet.ReadInt();
									bool flag7 = packet.ReadBoolean();
									int itemCount = client.Player.GetItemCount(templateId);
									if (num8 > itemCount)
									{
										client.Player.SendMessage("Número de fantasmas, operação falhou.");
										return 0;
									}
									if (num8 > 5)
									{
										num8 = 5;
									}
									bool val5 = false;
									int num9 = num8;
									int val6 = 0;
									int num10 = 10;
									if (flag7 && client.Player.MoneyDirect(GameProperties.ChristmasBuildSnowmanDoubleMoney))
									{
										num9 = num8 * 2;
									}
									christmas.exp += num9;
									if (christmas.exp >= num10)
									{
										christmas.exp -= num10;
										val5 = true;
										christmas.count++;
										val6 = 1;
									}
									client.Player.RemoveTemplate(templateId, num8);
									gSPacketIn.WriteByte(25);
									gSPacketIn.WriteBoolean(val5);
									gSPacketIn.WriteInt(christmas.count);
									gSPacketIn.WriteInt(christmas.exp);
									gSPacketIn.WriteInt(num9);
									gSPacketIn.WriteInt(val6);
									client.Out.SendTCP(gSPacketIn);
									return 0;
								}
							case 26:
								{
									int num11 = packet.ReadInt();
									if (DateTime.Compare(client.Player.LastOpenChristmasPackage.AddSeconds(1.0), DateTime.Now) > 0)
									{
										return 0;
									}
									if (christmas.packsNumber >= GameProperties.ChristmasGiftsMaxNum - 1)
									{
										client.Player.SendMessage("Número de bônus expirou.");
										return 0;
									}
									string[] array2 = GameProperties.ChristmasGifts.Split(new char[]
									{
							'|'
									});
									string text2 = "";
									int num12 = array2.Length;
									string[] array3 = array2;
									for (int k = 0; k < array3.Length; k++)
									{
										string text3 = array3[k];
										if (text3.Split(new char[]
										{
								','
										})[0] == num11.ToString())
										{
											text2 = text3;
											break;
										}
									}
									if (!(text2 != ""))
									{
										client.Player.SendMessage("Inelegível, a operação falhou.");
										return 0;
									}
									int num13 = int.Parse(text2.Split(new char[]
									{
							','
									})[1]);
									if (christmas.packsNumber >= num12 - 2)
									{
										int num14 = int.Parse(array2[num12 - 2].Split(new char[]
										{
								','
										})[1]);
										num13 = num14 + num13 * (christmas.packsNumber + 1);
									}
									if (num13 <= christmas.count)
									{
										christmas.packsNumber++;
										christmas.awardState |= 1 << christmas.packsNumber;
										client.Player.SendMessage("Seja recompensado pelo sucesso.");
										client.Player.SendItemToMail(num11, "", title);
										if (christmas.packsNumber == num12 - 2 && christmas.count < christmas.lastPacks)
										{
											christmas.count += int.Parse(array2[num12 - 1].Split(new char[]
											{
									','
											})[1]) * christmas.packsNumber;
										}
										gSPacketIn.WriteByte(26);
										gSPacketIn.WriteInt(christmas.awardState);
										gSPacketIn.WriteInt(christmas.packsNumber);
										gSPacketIn.WriteInt(num11);
										client.Out.SendTCP(gSPacketIn);
										client.Player.LastOpenChristmasPackage = DateTime.Now;
										return 0;
									}
									client.Player.SendMessage("Bonecos de neve insuficientes, a manipulação falhou.");
									return 0;
								}
							case 27:
								{
									byte b4 = packet.ReadByte();
									int[] array4 = new int[]
									{
							201146,
							201147
									};
									int num15 = ActiveSystemHandler.random.Next(array4.Length);
									if (DateTime.Compare(client.Player.LastOpenChristmasPackage.AddSeconds(1.0), DateTime.Now) > 0)
									{
										return 0;
									}
									if (b4 == 1 && christmas.dayPacks < 2)
									{
										christmas.dayPacks++;
										client.Player.SendMessage("Seja recompensado pelo sucesso.");
										client.Player.SendItemToMail(array4[num15], "", title);
									}
									else
									{
										if (christmas.count < 3)
										{
											client.Player.SendMessage("Acumule 3 bonecos de neve para receber presentes");
										}
										else
										{
											gSPacketIn.WriteByte(27);
											gSPacketIn.WriteBoolean(true);
											gSPacketIn.WriteInt(christmas.dayPacks);
											gSPacketIn.WriteInt(0);
											gSPacketIn.WriteInt(0);
											client.Out.SendTCP(gSPacketIn);
										}
									}
									client.Player.LastOpenChristmasPackage = DateTime.Now;
									return 0;
								}
							case 29:
								{
									int christmasBuyTimeMoney = GameProperties.ChristmasBuyTimeMoney;
									if (!client.Player.MoneyDirect(christmasBuyTimeMoney))
									{
										return 1;
									}
									int christmasBuyMinute = GameProperties.ChristmasBuyMinute;
									client.Player.Actives.AddTime(christmasBuyMinute);
									client.Player.SendMessage("manipulação de sucesso!");
									return 0;
								}
							case 33:
								{
									client.Player.Actives.YearMonterValidate();
									gSPacketIn.WriteByte(33);
									gSPacketIn.WriteInt(client.Player.Actives.Info.ChallengeNum);
									gSPacketIn.WriteInt(client.Player.Actives.Info.BuyBuffNum);
									gSPacketIn.WriteInt(GameProperties.YearMonsterBuffMoney);
									gSPacketIn.WriteInt(client.Player.Actives.Info.DamageNum);
									string[] array5 = GameProperties.YearMonsterBoxInfo.Split(new char[]
									{
							'|'
									});
									gSPacketIn.WriteInt(array5.Length);
									for (int l = 0; l < array5.Length; l++)
									{
										string[] array6 = array5[l].Split(new char[]
										{
								','
										});
										string[] array7 = client.Player.Actives.Info.BoxState.Split(new char[]
										{
								'-'
										});
										gSPacketIn.WriteInt(int.Parse(array6[0]));
										gSPacketIn.WriteInt(int.Parse(array6[1]) * 10000);
										gSPacketIn.WriteInt(int.Parse(array7[l]));
									}
									client.Out.SendTCP(gSPacketIn);
									return 0;
								}
							case 34:
								if (client.Player.MainWeapon == null)
								{
									client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip", new object[0]));
									return 0;
								}
								if (client.Player.Actives.Info.ChallengeNum > 0)
								{
									client.Player.Actives.Info.ChallengeNum--;
									RoomMgr.CreateCatchBeastRoom(client.Player);
									return 0;
								}
								return 0;
							case 35:
								{
									packet.ReadBoolean();
									if (!client.Player.MoneyDirect(GameProperties.YearMonsterBuffMoney))
									{
										return 0;
									}
									if (client.Player.Actives.Info.BuyBuffNum > 0)
									{
										client.Player.Actives.Info.BuyBuffNum--;
									}
									gSPacketIn.WriteByte(35);
									gSPacketIn.WriteInt(client.Player.Actives.Info.BuyBuffNum);
									client.Out.SendTCP(gSPacketIn);
									client.Player.SendMessage("Operação realizada com sucesso! ");
									AbstractBuffer abstractBuffer = BufferList.CreatePayBuffer(400, 30000, 1);
									if (abstractBuffer != null)
									{
										abstractBuffer.Start(client.Player);
									}
									abstractBuffer = BufferList.CreatePayBuffer(406, 30000, 1);
									if (abstractBuffer != null)
									{
										abstractBuffer.Start(client.Player);
										return 0;
									}
									return 0;
								}
							case 36:
								{
									int num16 = packet.ReadInt();
									DateTime.Compare(client.Player.LastOpenYearMonterPackage.AddSeconds(1.5), DateTime.Now);
									string[] array8 = GameProperties.YearMonsterBoxInfo.Split(new char[]
									{
							'|'
									});
									bool flag8 = this.CanGetGift(client.Player.Actives.Info.DamageNum, num16, array8);
									if (flag8)
									{
										int dateId = int.Parse(array8[num16].Split(new char[]
										{
								','
										})[0]);
										gSPacketIn.WriteByte(36);
										gSPacketIn.WriteBoolean(flag8);
										gSPacketIn.WriteInt(num16);
										client.Out.SendTCP(gSPacketIn);
										client.Player.Actives.SetYearMonterBoxState(num16);
										List<ItemInfo> list = new List<ItemInfo>();
										int num17 = 0;
										int num18 = 0;
										int num19 = 0;
										int num20 = 0;
										int num21 = 0;
										int num22 = 0;
										int num23 = 0;
										int num24 = 0;
										int num25 = 0;
										int num26 = 0;
										int num27 = 0;
										int num28 = 0;
										ItemBoxMgr.CreateItemBox(dateId, list, ref num18, ref num17, ref num19, ref num20, ref num21, ref num22, ref num23, ref num24, ref num25, ref num26, ref num27, ref num28);
										StringBuilder stringBuilder = new StringBuilder();
										foreach (ItemInfo current2 in list)
										{
											stringBuilder.Append(current2.Template.Name + " x" + current2.Count.ToString() + ", ");
										}
										client.Out.SendMessage(eMessageType.Normal, stringBuilder.ToString());
										client.Player.AddTemplate(list);
									}
									client.Player.LastOpenYearMonterPackage = DateTime.Now;
									return 0;
								}
							case 38:
								{
									client.Player.Actives.StopLightriddleTimer();
									LanternriddlesInfo lanternriddlesInfo = ActiveSystemMgr.EnterLanternriddles(client.Player.PlayerCharacter.ID);
									client.Player.Actives.SendLightriddleQuestion(lanternriddlesInfo);
									if (lanternriddlesInfo.CanNextQuest)
									{
										client.Player.Actives.BeginLightriddleTimer();
										return 0;
									}
									return 0;
								}
							case 40:
								{
									packet.ReadInt();
									packet.ReadInt();
									int option = packet.ReadInt();
									ActiveSystemMgr.LanternriddlesAnswer(client.Player.PlayerCharacter.ID, option);
									return 0;
								}
							case 41:
								{
									packet.ReadInt();
									packet.ReadInt();
									int num28 = packet.ReadInt();
									packet.ReadBoolean();
									LanternriddlesInfo lanternriddles = ActiveSystemMgr.GetLanternriddles(client.Player.PlayerCharacter.ID);
									if (lanternriddles == null)
									{
										client.Player.SendMessage("Erro de servidor de dados.");
										return 0;
									}
									bool flag9 = false;
									if (num28 == 0)
									{
										if (lanternriddles.HitFreeCount > 0)
										{
											lanternriddles.HitFreeCount--;
											lanternriddles.IsHint = true;
											flag9 = true;
										}
										else
										{
											int value2 = lanternriddles.HitPrice;
											if (client.Player.ActiveMoneyEnable(value2))
											{
												lanternriddles.IsHint = true;
												flag9 = true;
											}
										}
									}
									else
									{
										if (lanternriddles.DoubleFreeCount > 0)
										{
											lanternriddles.DoubleFreeCount--;
											lanternriddles.IsDouble = true;
											flag9 = true;
										}
										else
										{
											int value2 = lanternriddles.DoublePrice;
											if (client.Player.ActiveMoneyEnable(value2))
											{
												lanternriddles.IsDouble = true;
												flag9 = true;
											}
										}
									}
									if (flag9)
									{
										gSPacketIn.WriteByte(41);
										gSPacketIn.WriteBoolean(flag9);
										client.Out.SendTCP(gSPacketIn);
										client.Player.SendMessage("Operação realizada com sucesso! ");
										return 0;
									}
									return 0;
								}
							case 42:
								GameServer.Instance.LoginServer.SendLightriddleRank(client.Player.PlayerCharacter.NickName, client.Player.PlayerCharacter.ID);
								return 0;
							case 50:
								Console.WriteLine("50");
								break;
							case 51:
								Console.WriteLine("51");
								break;
							case 52:
								Console.WriteLine("52");
								break;
							case 53:
								Console.WriteLine("53");
								break;
							case 54:
								Console.WriteLine("54");
								break;
							case 55:
								Console.WriteLine("55");
								break;
							case 57:
								Console.WriteLine("57");
								break;
							case 49:
								Console.WriteLine("49");
								gSPacketIn.WriteByte(49);
								gSPacketIn.WriteByte(1);
								gSPacketIn.WriteInt(1);
								gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
								gSPacketIn.WriteBoolean(client.Player.PlayerCharacter.typeVIP == 1);
								gSPacketIn.WriteBoolean(client.Player.PlayerCharacter.Sex);
								gSPacketIn.WriteBoolean(true);
								gSPacketIn.WriteByte((byte)client.Player.PlayerCharacter.Grade);
								gSPacketIn.WriteByte(32);
								gSPacketIn.WriteByte(16);
								gSPacketIn.WriteByte(8);
								gSPacketIn.WriteByte(4);
								gSPacketIn.WriteByte(2);
								gSPacketIn.WriteByte(1);
								gSPacketIn.WriteByte(0);
								gSPacketIn.WriteByte(0);
								gSPacketIn.WriteByte(0);
								gSPacketIn.WriteByte(0);
								gSPacketIn.WriteByte(0);
								gSPacketIn.WriteByte(0);
								gSPacketIn.WriteInt(0);
								gSPacketIn.WriteDateTime(DateTime.Now.AddDays(7.0));
								gSPacketIn.WriteInt(1);
								client.Out.SendTCP(gSPacketIn);
								return 0;
						}
						break;
				}
			}
			else
			{
				switch (b2)
				{
					case 97:
						{
							//client.Player.ActiveQuestInventory.SyncNewPlayerActivity();

							return 0;
						}
					case 81:
						{
							//client.Player.ActiveQuestInventory.SyncSevenDayTarget();
							return 0;
						}
					case 87:
						{
							//client.Player.ActiveQuestInventory.SyncActivityQuest();
							return 0;
						}
					case 88:
						{
							//client.Player.ActiveQuestInventory.GetRewardActivityQuest(packet, b2);
							return 0;
						}
					case 82:
						{
							//client.Player.ActiveQuestInventory.GetRewardSevenDayTarget(packet);
							return 0;
						}
					case 98:
						{
						//	client.Player.ActiveQuestInventory.GetRewardNewPlayerActivity(packet);
							return 0;
						}
					case 0x47:
						if (!client.Player.isUpdateEnterModePoint)
						{
							client.Player.isUpdateEnterModePoint = true;
							WorldMgr.ChatEntertamentModeGetPoint(client.Player.PlayerCharacter);
						}
						gSPacketIn = new GSPacketIn(0x91);
						gSPacketIn.WriteByte(0x48);
						gSPacketIn.WriteInt(client.Player.PlayerCharacter.EntertamentPoint);
						client.Player.SendTCP(gSPacketIn);
						return 0;
					case 75:
						this.SendDDPlayInfo(client);
						return 0;
					case 76:
						{
							gSPacketIn = new GSPacketIn(145);
							gSPacketIn.WriteByte(76);
							int itemCount2 = client.Player.PropBag.GetItemCount(201310);
							if (itemCount2 <= 0)
							{
								client.Player.SendMessage("Você não tem Moedas Felizes suficientes para girar!");
								return 0;
							}
							client.Player.PropBag.RemoveTemplate(201310, 1);
							client.Player.PlayerCharacter.DDPlayPoint += 10;
							int num29 = ActiveSystemHandler.random.Next(1, 50);
							int[] source = new int[]
							{
						2,
						3,
						5,
						10
							};
							if (!source.Contains(num29))
							{
								num29 = 0;
							}
							gSPacketIn.WriteInt(num29);
							gSPacketIn.WriteInt(client.Player.PlayerCharacter.DDPlayPoint);
							client.Player.SendTCP(gSPacketIn);
							if (num29 > 0)
							{
								int money = GameProperties.DDPlayActivityMoney * num29;
								using (PlayerBussiness playerBussiness = new PlayerBussiness())
								{
									client.Player.SendMoneyMailToUser(playerBussiness, "Parabéns x" + num29 + "Moedas do evento Find Pictures.", "Evento Winning Coin Encontrar imagem", money, eMailType.BuyItem);
								}
								GSPacketIn packet2 = WorldMgr.SendSysNotice(string.Format("Jogador [{0}] no evento \"DDT ALEGRE\" teve a sorte de receber X{1} Premio!", client.Player.PlayerCharacter.NickName, num29));
								GameServer.Instance.LoginServer.SendPacket(packet2);
								return 0;
							}
							return 0;
						}
					case 77:
						{
							int num30 = packet.ReadInt();
							if (num30 <= 0 || num30 > 999)
							{
								return 0;
							}
							int num31 = num30 * 100;
							if (client.Player.PlayerCharacter.DDPlayPoint >= num31)
							{
								client.Player.RemoveDDPlayPoint(num31);
								using (new PlayerBussiness())
								{
									ItemInfo itemInfo2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(201310), num30, 105);
									itemInfo2.IsBinds = true;
									List<ItemInfo> items = new List<ItemInfo>
							{
								itemInfo2
							};
									client.Player.SendItemsToMail(items, "Você Ganhou " + num30 + " Happy Coins para resgatar pontos.", "Feliz Redenção de Moedas", eMailType.BuyItem);
								}
								client.Player.SendMessage("Alterado com sucesso! Itens enviados para o correio.");
								this.SendDDPlayInfo(client);
								return 0;
							}
							client.Player.SendMessage("Você não tem pontos suficientes para resgatar");
							return 0;
						}
					default:
						switch (b2)
						{
							case 90:
								this.SendEnterBoguAdventure(client, packet);
								return 0;
							case 91:
								{
									int num32 = packet.ReadInt();
									int num33 = 0;
									if (num32 != 3)
									{
										num33 = packet.ReadInt();
									}
									BoguCeilInfo boguCeilInfo = client.Player.Actives.FindCeilBoguMap(num33);
									int val7 = -2;
									switch (num32)
									{
										case 1:
											if (boguCeilInfo != null && boguCeilInfo.State == 3)
											{
												boguCeilInfo.State = 1;
												client.Player.Actives.UpdateCeilBoguMap(boguCeilInfo);
											}
											break;
										case 2:
											if (boguCeilInfo != null && boguCeilInfo.State == 1)
											{
												boguCeilInfo.State = 3;
												client.Player.Actives.UpdateCeilBoguMap(boguCeilInfo);
											}
											break;
										case 3:
											if (boguAdventure.HP > 0 && boguAdventure.CurrentPostion > 0 && client.Player.PlayerCharacter.Money >= client.Player.Actives.BoguAdventureMoney[0])
											{
												BoguCeilInfo[] totalMineAroundNotOpen = client.Player.Actives.GetTotalMineAroundNotOpen(boguAdventure.CurrentPostion);
												if (totalMineAroundNotOpen.Length > 0)
												{
													int num34 = ActiveSystemHandler.random.Next(0, totalMineAroundNotOpen.Length - 1);
													boguCeilInfo = totalMineAroundNotOpen[num34];
													if (boguCeilInfo != null)
													{
														client.Player.RemoveMoney(client.Player.Actives.BoguAdventureMoney[0]);
														boguCeilInfo.State = 2;
														client.Player.Actives.UpdateCeilBoguMap(boguCeilInfo);
														num33 = boguCeilInfo.Index;
														val7 = -1;
													}
												}
												else
												{
													client.Player.SendMessage("Você abriu todas as bombas ao seu redor!");
												}
											}
											else
											{
												client.Player.SendMessage("Atualmente não há bombas fechadas ao redor!");
											}
											break;
										case 4:
											if (boguCeilInfo != null)
											{
												if (boguCeilInfo.State == 3 && boguAdventure.HP > 0)
												{
													if (boguCeilInfo.Result == -1)
													{
														boguAdventure.HP--;
														val7 = -1;
													}
													else
													{
														boguAdventure.OpenCount++;
													}
													boguCeilInfo.State = 2;
													client.Player.Actives.UpdateCeilBoguMap(boguCeilInfo);
												}
												boguAdventure.CurrentPostion = boguCeilInfo.Index;
											}
											break;
									}
									gSPacketIn = new GSPacketIn(145);
									gSPacketIn.WriteByte(91);
									gSPacketIn.WriteInt(num32);
									gSPacketIn.WriteInt(num33);
									gSPacketIn.WriteInt(val7);
									gSPacketIn.WriteInt(client.Player.Actives.BoguAdventureMoney[0]);
									if (num32 == 4)
									{
										gSPacketIn.WriteInt(boguAdventure.HP);
										gSPacketIn.WriteInt(boguAdventure.OpenCount);
									}
									client.Player.SendTCP(gSPacketIn);
									return 0;
								}
							case 92:
								{
									int num35 = packet.ReadInt();
									gSPacketIn = new GSPacketIn(145);
									if (num35 == 1)
									{
										if (boguAdventure.HP <= 0 && client.Player.PlayerCharacter.Money >= client.Player.Actives.BoguAdventureMoney[1])
										{
											client.Player.RemoveMoney(client.Player.Actives.BoguAdventureMoney[1]);
											boguAdventure.HP = 2;
										}
										else
										{
											client.Player.SendMessage("Suas moedas não são suficientes");
										}
										gSPacketIn.WriteByte(92);
										gSPacketIn.WriteInt(boguAdventure.HP);
										client.Player.SendTCP(gSPacketIn);
										return 0;
									}
									if (boguAdventure.CurrentPostion > 0 && client.Player.PlayerCharacter.Money >= client.Player.Actives.BoguAdventureMoney[2] && boguAdventure.ResetCount > 0)
									{
										if (boguAdventure.GetAward()[0] == "1" && boguAdventure.GetAward()[1] == "1" && boguAdventure.GetAward()[2] == "1")
										{
											client.Player.SendMessage("Você completou sua jornada hoje.");
										}
										else
										{
											client.Player.RemoveMoney(client.Player.Actives.BoguAdventureMoney[1]);
											boguAdventure.ResetCount--;
											client.Player.Actives.ResetBoguAdventureInfo();
										}
									}
									else
									{
										client.Player.SendMessage("Suas moedas não são suficientes");
									}
									this.SendEnterBoguAdventure(client, packet);
									return 0;
								}
							case 93:
								{
									int num36 = packet.ReadInt();
									string[] award = boguAdventure.GetAward();
									if (award[num36] == "0")
									{
										boguAdventure.SetAward(num36, 1);
										List<EventAwardInfo> boGuBoxAward = EventAwardMgr.GetBoGuBoxAward(num36);
										List<ItemInfo> list2 = new List<ItemInfo>();
										List<string> list3 = new List<string>();
										foreach (EventAwardInfo current3 in boGuBoxAward)
										{
											ItemInfo itemInfo3 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(current3.TemplateID), current3.Count, 105);
											itemInfo3.AttackCompose = current3.AttackCompose;
											itemInfo3.DefendCompose = current3.DefendCompose;
											itemInfo3.AgilityCompose = current3.AgilityCompose;
											itemInfo3.LuckCompose = current3.LuckCompose;
											itemInfo3.IsBinds = current3.IsBinds;
											itemInfo3.ValidDate = current3.ValidDate;
											if (!client.Player.AddTemplate(itemInfo3))
											{
												list2.Add(itemInfo3);
											}
											list3.Add(itemInfo3.Template.Name + "x" + itemInfo3.Count);
										}
										if (list2.Count > 0)
										{
											client.Player.SendItemsToMail(list2, "Os presentes do evento de aventura do bugou devolvidos ao correio porque o seu bolso está cheio", "Presente de evento Bugou Adventure", eMailType.BuyItem);
										}
										if (list3.Count > 0)
										{
											client.Player.SendMessage("Você Ganhou " + string.Join(", ", list3.ToArray()));
										}
										if (boguAdventure.GetAward()[0] == "1" && boguAdventure.GetAward()[1] == "1" && boguAdventure.GetAward()[2] == "1")
										{
											GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(string.Format("Jogador [{0}] concluiu Aventura Bugou com Exito !", client.Player.PlayerCharacter.NickName)));
										}
									}
									else
									{
										client.Player.SendMessage("Você recebeu esta caixa de presente.");
									}
									gSPacketIn = new GSPacketIn(145);
									gSPacketIn.WriteByte(93);
									gSPacketIn.WriteInt(int.Parse(boguAdventure.GetAward()[0]));
									gSPacketIn.WriteInt(int.Parse(boguAdventure.GetAward()[1]));
									gSPacketIn.WriteInt(int.Parse(boguAdventure.GetAward()[2]));
									client.Player.SendTCP(gSPacketIn);
									return 0;
								}
							case 94:
								client.Player.Actives.SaveToDatabase();
								return 0;
						}
						break;
				}
			}
			Console.WriteLine("activeSystem_cmd: " + b);
			return 0;
		}
		private void SendDDPlayInfo(GameClient client)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(145);
			gSPacketIn.WriteByte(75);
			gSPacketIn.WriteInt(client.Player.PlayerCharacter.DDPlayPoint);
			client.Player.SendTCP(gSPacketIn);
		}
		private void SendEnterBoguAdventure(GameClient client, GSPacketIn packet)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(145);
			gSPacketIn.WriteByte(90);
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventure.CurrentPostion);
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventure.HP);
			gSPacketIn.WriteInt(int.Parse(client.Player.Actives.BoguAdventure.GetAward()[0]));
			gSPacketIn.WriteInt(int.Parse(client.Player.Actives.BoguAdventure.GetAward()[1]));
			gSPacketIn.WriteInt(int.Parse(client.Player.Actives.BoguAdventure.GetAward()[2]));
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventure.OpenCount);
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventureMoney[0]);
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventureMoney[1]);
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventureMoney[2]);
			gSPacketIn.WriteBoolean(false);
			gSPacketIn.WriteInt(client.Player.Actives.BoguAdventure.ResetCount);
			gSPacketIn.WriteInt(70);
			foreach (BoguCeilInfo current in client.Player.Actives.BoguAdventure.MapData)
			{
				gSPacketIn.WriteInt(current.Index);
				gSPacketIn.WriteInt(current.State);
				gSPacketIn.WriteInt((current.State == 2) ? current.Result : -2);
				gSPacketIn.WriteInt(client.Player.Actives.GetTotalMineAround(current.Index).Length);
			}
			for (int i = 0; i < 3; i++)
			{
				gSPacketIn.WriteInt(client.Player.Actives.CountOpenCanTakeBoxGoguAdventure(i));
				List<EventAwardInfo> boGuBoxAward = EventAwardMgr.GetBoGuBoxAward(i);
				gSPacketIn.WriteInt(boGuBoxAward.Count);
				foreach (EventAwardInfo current2 in boGuBoxAward)
				{
					gSPacketIn.WriteInt(current2.TemplateID);
					gSPacketIn.WriteInt(current2.Count);
				}
			}
			client.Player.SendTCP(gSPacketIn);
		}
		private bool CanGetGift(int damageNum, int id, string[] YearMonsterBoxInfo)
		{
			if (id > 4 || id < 0)
			{
				return false;
			}
			int num = int.Parse(YearMonsterBoxInfo[id].Split(new char[]
			{
				','
			})[1]) * 10000;
			return num <= damageNum;
		}
		private string[] GetLayerItems(string[] lists, int layer)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < lists.Length; i++)
			{
				string text = lists[i];
				string a = text.Split(new char[]
				{
					'-'
				})[0];
				if (a == layer.ToString())
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}
	}
}
