// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.DiceHandler
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F406BD-7233-42D4-AF91-73FA12644876
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\dll8.6\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(134, "场景用户离开")]
    public class DiceHandler : IPacketHandler
    {
        private ThreadSafeRandom threadSafeRandom = new ThreadSafeRandom();

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte num1 = packet.ReadByte();
            int id = client.Player.PlayerCharacter.ID;
            switch (num1)
            {
                case 10:
                    client.Player.Dice.ReceiveData();
                    client.Player.Out.SendDiceReceiveData(client.Player.Dice);
                    break;
                case 11:
                    if (client.Player.Dice.Data.LuckIntegral >= client.Player.Dice.IntegralPoint[client.Player.Dice.MAX_LEVEL - 1])
                    {
                        client.Player.SendMessage("Bạn đã nhận hết phần thưởng tích lũy hôm nay.");
                        return 0;
                    }
                    int num2 = packet.ReadInt();
                    packet.ReadInt();
                    int index;
                    int num3;
                    switch (num2)
                    {
                        case 1:
                            index = this.threadSafeRandom.Next(2, 13);
                            num3 = client.Player.Dice.doubleDicePrice;
                            break;
                        case 2:
                            index = this.threadSafeRandom.Next(4, 7);
                            num3 = client.Player.Dice.bigDicePrice;
                            break;
                        case 3:
                            index = this.threadSafeRandom.Next(1, 4);
                            num3 = client.Player.Dice.smallDicePrice;
                            break;
                        default:
                            index = this.threadSafeRandom.Next(1, 7);
                            num3 = client.Player.Dice.commonDicePrice;
                            break;
                    }
                    if (client.Player.Dice.Data.FreeCount > 0)
                    {
                        --client.Player.Dice.Data.FreeCount;
                        this.receiveResult(client.Player, index);
                        break;
                    }
                    if (client.Player.MoneyDirect(num3))
                        this.receiveResult(client.Player, index);
                    break;
                case 12:
                    int refreshPrice = client.Player.Dice.refreshPrice;
                    if (client.Player.MoneyDirect(refreshPrice))
                    {
                        client.Player.Dice.CreateDiceAward();
                        client.Player.Out.SendDiceReceiveData(client.Player.Dice);
                        break;
                    }
                    break;
            }
            return 0;
        }

        private void receiveResult(GamePlayer player, int index)
        {
            GSPacketIn packet = new GSPacketIn((short)134);
            packet.WriteByte((byte)4);
            packet.WriteInt(player.Dice.Data.CurrentPosition);
            packet.WriteInt(index);
            player.Dice.Data.CurrentPosition += index;
            if (player.Dice.Data.CurrentPosition > 18)
                player.Dice.Data.CurrentPosition -= 19;
            EventAwardInfo eventAwardInfo = player.Dice.RewardItem[player.Dice.Data.CurrentPosition];
            ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventAwardInfo.TemplateID), eventAwardInfo.Count, 103);
            fromTemplate.IsBinds = eventAwardInfo.IsBinds;
            if (!player.AddTemplate(fromTemplate, "Xí ngầu"))
                player.SendItemToMail(fromTemplate, fromTemplate.Template.Name.ToString(), "Phần thưởng Xí Ngầu.", eMailType.OpenUpArk);
            player.Dice.RewardName = fromTemplate.Template.Name;
            int num = this.threadSafeRandom.Next(2, 13);
            player.Dice.Data.LuckIntegral += num;
            int luckIntegralLevel = player.Dice.Data.LuckIntegralLevel;
            if (player.Dice.Data.LuckIntegral >= player.Dice.IntegralPoint[luckIntegralLevel + 1])
            {
                ++player.Dice.Data.LuckIntegralLevel;
                ++player.PlayerCharacter.luckyNum;
                player.Dice.GetLevelAward();
            }
            if (player.Dice.Data.LuckIntegralLevel > 3)
            {
                player.Dice.Data.LuckIntegralLevel = 3;
                player.Dice.Data.LuckIntegral = player.Dice.IntegralPoint[player.Dice.MAX_LEVEL - 1];
            }
            packet.WriteInt(player.Dice.Data.LuckIntegral);
            packet.WriteInt(player.Dice.Data.LuckIntegralLevel);
            packet.WriteInt(player.Dice.Data.FreeCount);
            packet.WriteString(player.Dice.RewardName);
            player.Out.SendTCP(packet);
        }
    }
}
