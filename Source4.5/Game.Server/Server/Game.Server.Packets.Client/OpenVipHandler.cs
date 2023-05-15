// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.OpenVipHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(92, "VIP")]
    public class OpenVipHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            string nickName = packet.ReadString();
            int num1 = packet.ReadInt();
            int num2 = 1179;
            int num3 = 2394;
            int num4 = 4200;
            string message1 = LanguageMgr.GetTranslation("OpenVipHandler.Msg1");
            int days = num1;
            int num5;
            switch (days)
            {
                case 31:
                    num5 = 399;
                    break;
                case 93:
                    num5 = num2;
                    break;
                case 186:
                    num5 = num3;
                    break;
                case 365:
                    num5 = num4;
                    break;
                default:
                    num5 = num1 / 31 * 399;
                    break;
            }
            GamePlayer byPlayerNickName = WorldMgr.GetClientByPlayerNickName(nickName);
            DailyRecordInfo info = new DailyRecordInfo();
            info.UserID = client.Player.PlayerCharacter.ID;
            info.Type = 6;
            info.Value = "VIP";
            if (client.Player.MoneyDirect(num5, false))
            {
                DateTime now = DateTime.Now;
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    int typeVIP = (int)client.Player.SetTypeVIP(num1);
                    playerBussiness.VIPRenewal(nickName, num1, typeVIP, ref now);
                    if (byPlayerNickName == null)
                        message1 = "O jogador " + nickName + " não foi encontrado ou não está online!";
                    else if (client.Player.PlayerCharacter.NickName == nickName)
                    {
                        if (client.Player.PlayerCharacter.typeVIP == (byte)0)
                        {
                            client.Player.OpenVIP(days, now);
                            client.Player.PlayerCharacter.VIPNextLevelDaysNeeded = 10;
                            playerBussiness.AddDailyRecord(info);
                        }
                        else
                        {
                            client.Player.ContinuousVIP(days, now);
                            message1 = LanguageMgr.GetTranslation("OpenVipHandler.Msg3");
                        }
                        client.Out.SendOpenVIP(client.Player);
                    }
                    else
                    {
                        string message2;
                        if (byPlayerNickName.PlayerCharacter.typeVIP == (byte)0)
                        {
                            byPlayerNickName.OpenVIP(days, now);
                            byPlayerNickName.PlayerCharacter.VIPNextLevelDaysNeeded = 10;
                            message1 = "O VIP do jogador " + nickName + " foi aberto com sucesso!";
                            message2 = client.Player.PlayerCharacter.NickName + " abriu o VIP para você!";
                        }
                        else
                        {
                            byPlayerNickName.ContinuousVIP(days, now);
                            message1 = "O VIP do jogador " + nickName + " foi renovado com sucesso!";
                            message2 = client.Player.PlayerCharacter.NickName + " renovou o seu VIP!";
                        }
                        byPlayerNickName.Out.SendOpenVIP(byPlayerNickName);
                        byPlayerNickName.Out.SendMessage(eMessageType.GM_NOTICE, message2);
                    }
                    client.Player.AddExpVip(num5);
                    client.Out.SendMessage(eMessageType.GM_NOTICE, message1);
                    if (client.Player.PlayerCharacter.typeVIP != (byte)0)
                        client.Player.PlayerCharacter.VIPNextLevelDaysNeeded = client.Player.GetVIPNextLevelDaysNeeded(client.Player.PlayerCharacter.VIPLevel, client.Player.PlayerCharacter.VIPExp);
                }
            }
            return 0;
        }
    }
}
