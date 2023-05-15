using Bussiness;
using Bussiness.Interface;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(1, "User Login handler")]
    public class UserLoginHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int result;
            try
            {
                if (client.Player == null)
                {
                    int version = packet.ReadInt();
                    packet.ReadInt();
                    byte[] array = new byte[8];
                    byte[] array2 = packet.ReadBytes();
                    try
                    {
                        array2 = WorldMgr.RsaCryptor.Decrypt(array2, false);
                    }
                    catch (System.Exception exception)
                    {
                        client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.RsaCryptorError", new object[0]));
                        client.Disconnect();
                        GameServer.log.Error("RsaCryptor", exception);
                        int num = 0;
                        result = num;
                        return result;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        array[i] = array2[i + 7];
                    }
                    client.setKey(array);
                    string[] array3 = System.Text.Encoding.UTF8.GetString(array2, 15, array2.Length - 15).Split(new char[]
                    {
                        ','
                    });
                    if (array3.Length == 2)
                    {
                        string text = array3[0];
                        string text2 = array3[1];
                        if (!LoginMgr.ContainsUser(text))
                        {
                            bool flag = false;
                            BaseInterface baseInterface = BaseInterface.CreateInterface();
                            PlayerInfo playerInfo = baseInterface.LoginGame(text, text2, ref flag);
                            if (playerInfo != null && playerInfo.ID != 0)
                            {
                                if (playerInfo.ID == -2)
                                {
                                    client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Forbid", new object[0]));
                                    client.Disconnect();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    System.Console.WriteLine("{0} Está banido e tentou se conectar....", text);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    int num = 0;
                                    result = num;
                                    return result;
                                }
                                if (!flag)
                                {
                                    client.Player = new GamePlayer(playerInfo.ID, text, client, playerInfo);
                                    LoginMgr.Add(playerInfo.ID, client);
                                    client.Server.LoginServer.SendAllowUserLogin(playerInfo.ID);
                                    client.Version = version;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    System.Console.WriteLine("Jogador {0} Entrando no servidor ....", text);
                                    client.ClientStep++;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else
                                {
                                    client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Register", new object[0]));
                                    client.Disconnect();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                System.Console.WriteLine("{0} Tentou se conectar {1} e falhou....", text, text2);
                                Console.ForegroundColor = ConsoleColor.Green;
                                client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.OverTime", new object[0]));
                                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("GameServer.entradafalhou.Msg"));
                                client.Disconnect();
                            }
                        }
                        else
                        {
                            client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.LoginError", new object[0]));
                            client.Disconnect();
                        }
                    }
                    else
                    {
                        client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.LengthError", new object[0]));
                        client.Disconnect();
                    }
                }
            }
            catch (System.Exception exception2)
            {
                client.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.ServerError", new object[0]));
                client.Disconnect();
                GameServer.log.Error(LanguageMgr.GetTranslation("UserLoginHandler.ServerError", new object[0]), exception2);
            }
            result = 1;
            return result;
        }
    }
}
