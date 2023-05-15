using Game.Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(133, "Sistema de Potencial")]
    public class LatentEnergyHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte request = packet.ReadByte();
            switch (request)
            {
                case 1: //Ativar Potencial
                    {
                        string EnergyAttribute = "";
                        int EquipBag = packet.ReadInt(); //loc6.writeInt(param2);
                        int EquipPlace = packet.ReadInt(); //loc6.writeInt(param3);
                        int PropBag = packet.ReadInt(); //loc6.writeInt(param4);
                        int PropPlace = packet.ReadInt(); //loc6.writeInt(param5);
                        ItemInfo PropInfo = client.Player.GetItemAt((eBageType)PropBag, PropPlace); //Itens
                        ItemInfo EquipInfo = client.Player.GetItemAt((eBageType)EquipBag, EquipPlace); //Equipamentos
                        if (EquipInfo == null && PropInfo == null)
                        {
                            client.Player.SendMessage("Você não colocou os itens necessários.");
                            return 0;
                        }
                        if (!EquipInfo.CanLatentEnergy())
                        {
                            client.Player.SendMessage("Você não pode potencializar esse item.");
                            return 0;
                        }
                        if (!PropInfo.IsLatentEnery())
                        {
                            client.Player.SendMessage("Você precisa colocar uma pedra de potencial fonte.");
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            int random = new RandomSafe().Next(PropInfo.Template.Property2, PropInfo.Template.Property3);
                            if (i == 3)
                            {
                                EnergyAttribute += String.Format(Convert.ToString(random));
                            }
                            else
                            {
                                EnergyAttribute += String.Format(random + ",");
                            }
                        }
                        if (EquipInfo.latentEnergyCurStr.Split(',')[0] == "0")
                        {
                            EquipInfo.latentEnergyCurStr = EnergyAttribute;
                            EquipInfo.latentEnergyEndTime = DateTime.Now.AddDays(7);
                        }
                        if (client.Player.GetItemCount(PropInfo.TemplateID) < 1)
                        {
                            client.Player.SendMessage("Você não tem itens suficientes");
                            return 0;
                        }
                        EquipInfo.latentEnergyNewStr = EnergyAttribute;
                        client.Player.UpdateItem(EquipInfo);
                        client.Player.UpdateProperties();
                        equipChangeHandler(client.Player, EquipInfo);
                        client.Player.SendMessage("Potencializado com sucesso.");
                        client.Player.RemoveTemplate(PropInfo.TemplateID, 1);
                        break;
                    }
                case 2: //Mudar para novos atributos.
                    {
                        int EquipBag = packet.ReadInt(); //loc6.writeInt(param2);
                        int EquipPlace = packet.ReadInt(); //loc6.writeInt(param3);
                        ItemInfo EquipInfo = client.Player.GetItemAt((eBageType)EquipBag, EquipPlace);
                        if (EquipInfo == null)
                        {
                            client.Player.SendMessage("O Item não foi encontrado.");
                            return 0;
                        }
                        if (EquipInfo.latentEnergyNewStr.Split(',')[0] == "0")
                        {
                            client.Player.SendMessage("Não há nada para substituir");
                            return 0;
                        }
                        int SameValues = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (EquipInfo.latentEnergyCurStr.Split(',')[i] == EquipInfo.latentEnergyNewStr.Split(',')[i])
                            {
                                SameValues++;
                            }
                        }
                        if (SameValues >= 4)
                        {
                            client.Player.SendMessage("Você não pode substituir seus atributos pelo mesmo valor.");
                            return 0;
                        }
                        EquipInfo.latentEnergyCurStr = EquipInfo.latentEnergyNewStr;
                        EquipInfo.latentEnergyNewStr = "0,0,0,0";
                        EquipInfo.latentEnergyEndTime = DateTime.Now.AddDays(7);
                        client.Player.UpdateItem(EquipInfo);
                        client.Player.UpdateProperties();
                        equipChangeHandler(client.Player, EquipInfo);
                        client.Player.SendMessage("Substituição efetuada com sucesso");
                        break;
                    }
            }
            return 0;
        }

        public void equipChangeHandler(GamePlayer player, ItemInfo itemInfo)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(133, player.PlayerId);
            gSPacketIn.WriteInt(itemInfo.Place); //loc3.place = loc2.readInt();
            gSPacketIn.WriteString(itemInfo.latentEnergyCurStr); //loc3.curStr = loc2.readUTF();
            gSPacketIn.WriteString(itemInfo.latentEnergyNewStr); //loc3.newStr = loc2.readUTF();
            gSPacketIn.WriteDateTime(itemInfo.latentEnergyEndTime);
            player.Out.SendTCP(gSPacketIn); //
        }
    }
}