// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserEquipListHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(74, "获取用户装备")]
    public class UserEquipListHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = 0;
            string nickName = (string)null;
            bool flag = packet.ReadBoolean();
            PlayerInfo player = (PlayerInfo)null;
            List<SqlDataProvider.Data.ItemInfo> items = (List<SqlDataProvider.Data.ItemInfo>)null;
            List<UserGemStone> UserGemStone = (List<UserGemStone>)null;
            GamePlayer gamePlayer;
            if (!flag)
            {
                nickName = packet.ReadString();
                gamePlayer = WorldMgr.GetClientByPlayerNickName(nickName);
            }
            else
            {
                num = packet.ReadInt();
                gamePlayer = WorldMgr.GetPlayerById(num);
            }
            if (gamePlayer != null)
            {
                player = gamePlayer.PlayerCharacter;
                items = gamePlayer.EquipBag.GetItems(0, 31);
               UserGemStone = gamePlayer.GemStone;
            }
            else
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    player = flag ? playerBussiness.GetUserSingleByUserID(num) : playerBussiness.GetUserSingleByNickName(nickName);
                    if (player != null)
                    {
                        player.Texp = playerBussiness.GetUserTexpInfoSingle(player.ID);
                        UserGemStone = playerBussiness.GetSingleGemStones(num);
                        items = playerBussiness.GetUserEuqip(player.ID);
                    }
                }
            }
            if (player != null && items != null && player.Texp != null && UserGemStone != null)
                client.Out.SendUserEquip(player, items, UserGemStone);
            else
                Console.WriteLine("//view player wrong");
            return 0;
        }
    }
}
