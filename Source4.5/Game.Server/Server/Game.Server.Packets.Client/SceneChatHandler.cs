using SqlDataProvider.Data;
using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.Managers;
using Game.Server.Rooms;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Game.Server.Battle;
using System.Collections.Generic;
using System.Linq;
using Game.Server.Packets;


namespace Game.Server.Packets.Client
{
    [PacketHandler(19, "Cenários de usuários de bate-papo")]
    public class SceneChatHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            packet.ClientID = (client.Player.PlayerCharacter.ID);
            byte b = packet.ReadByte();
            bool flag = packet.ReadBoolean();
            packet.ReadString();
            string text = packet.ReadString();
            GSPacketIn gSPacketIn = new GSPacketIn(19, client.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(client.Player.ZoneId);
            gSPacketIn.WriteByte(b);
            gSPacketIn.WriteBoolean(flag);
            gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
            gSPacketIn.WriteString(text);
            String[] mensagem = text.Split(';');
            if (mensagem.Length > 1 && mensagem.Length < 5)
            {
                if (mensagem[0].Equals("@@"))
                {
                    if (verificarAdm(client.Player.PlayerCharacter.NickName))
                    {
                        if (mensagem[1].Equals("reload")) // Recarregar o servidor !;reload
                        {
                            // Recarregar
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            BallMgr.ReLoad(); Console.WriteLine("Foi recarregado BallMgr");
                            ShopMgr.ReLoad(); Console.WriteLine("Foi recarregado ShopMgr");
                            MapMgr.ReLoadMap(); Console.WriteLine("Foi recarregado MapMgr");
                            MapMgr.ReLoadMapServer(); Console.WriteLine("Foi recarregado MapServerMgr");
                            PropItemMgr.Reload(); Console.WriteLine("Foi recarregado PropItemMgr");
                            ItemMgr.ReLoad(); Console.WriteLine("Foi recarregado ItemMgr");
                            QuestMgr.ReLoad(); Console.WriteLine("Foi recarregado QuestMgr");
                            FusionMgr.ReLoad(); Console.WriteLine("Foi recarregado FusionMgr");
                            ConsortiaMgr.ReLoad(); Console.WriteLine("Foi recarregado ConsortiaMgr");
                            NPCInfoMgr.ReLoad(); Console.WriteLine("Foi recarregado NPCInfoMgr");
                            PveInfoMgr.ReLoad(); Console.WriteLine("Foi recarregado PveInfoMgr");
                            RateMgr.ReLoad(); Console.WriteLine("Foi recarregado RateMgr");
                            FightRateMgr.ReLoad(); Console.WriteLine("Foi recarregado FightRateMgr");
                            AwardMgr.ReLoad(); Console.WriteLine("Foi recarregado AwardMgr");
                            LanguageMgr.Reload(""); Console.WriteLine("Foi recarregado LanguageMgr");
                            MissionInfoMgr.Reload(); Console.WriteLine("Foi recarregado MissionInfoMgr");
                            DropMgr.ReLoad(); Console.WriteLine("Foi recarregado DropMgr");
                            // Avisar o console que foi recarregado
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("O servidor foi recarregado através de comando ingame reload.");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            // Avisar o player que foi recarregado
                            client.Player.SendMessage("Você recarregou o servidor.");
                        }

                        if (mensagem[1].Equals("receberitem"))
                        {
                            int TemplateID = int.Parse(mensagem[2]);
                            int Count = int.Parse(mensagem[3]);
                            ItemInfo ItemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(TemplateID), Count, 101);
                            List<ItemInfo> list = new List<ItemInfo>();
                            ItemInfo.Count = Count;
                            ItemInfo.StrengthenLevel = 0;
                            ItemInfo.AttackCompose = 0;
                            ItemInfo.DefendCompose = 0;
                            ItemInfo.AgilityCompose = 0;
                            ItemInfo.LuckCompose = 0;
                            ItemInfo.IsBinds = false;
                            ItemInfo.ValidDate = 0;
                            list.Add(ItemInfo);

                            foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
                            {
                                client.Player.SendItemsToMail(list, "Item recebido através de comando de administrador", "Item recebido por comando", default);
                                client.Player.SendMessage("Você recebeu um item através de comando.");
                            }
                        }
                        if (mensagem[1].Equals("meuitem"))
                        {
                            int TemplateID = int.Parse(mensagem[2]);
                            int Count = int.Parse(mensagem[3]);
                            ItemInfo ItemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(TemplateID), Count, 101);
                            ItemInfo.Count = Count;
                            ItemInfo.StrengthenLevel = 0;
                            ItemInfo.AttackCompose = 0;
                            ItemInfo.DefendCompose = 0;
                            ItemInfo.AgilityCompose = 0;
                            ItemInfo.LuckCompose = 0;
                            ItemInfo.IsBinds = false;
                            ItemInfo.ValidDate = 0;

                            List<ItemInfo> list = new List<ItemInfo>();
                            list.Add(ItemInfo);

                            client.Player.SendItemsToMail(list, "Item recebido através de comando de administrador", "Item recebido por comando", default);
                            client.Player.SendMessage("Você recebeu um item através de comando.");
                        }


                        if (mensagem[1].Equals("paratodos"))
                        {
                            int TemplateID = int.Parse(mensagem[2]);
                            int Count = int.Parse(mensagem[3]);
                            ItemInfo ItemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(TemplateID), Count, 101);
                            List<ItemInfo> list = new List<ItemInfo>();
                            ItemInfo.Count = Count;
                            ItemInfo.StrengthenLevel = 0;
                            ItemInfo.AttackCompose = 0;
                            ItemInfo.DefendCompose = 0;
                            ItemInfo.AgilityCompose = 0;
                            ItemInfo.LuckCompose = 0;
                            ItemInfo.IsBinds = true;
                            ItemInfo.ValidDate = 0;
                            list.Add(ItemInfo);

                            // Envie para todos os jogadores on-line
                            foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
                            {
                                gamePlayer.SendItemsToMail(list, "Obrigado por jogar ddtank", "Prêmio por ficar online", default);
                                gamePlayer.SendMessage("Você ganhou recebeu um prêmio por estar online! ");
                            }
                        }



                        if (mensagem[1] == "sorteio")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Avisando jogadores...");
                            List<GamePlayer> players = WorldMgr.GetAllPlayers().ToList();
                            if (players.Count > 0)
                            {
                                foreach (GamePlayer player in players)
                                {
                                    player.SendMessage("Sorteio começará em 5 segundos!");
                                }
                                Console.WriteLine("Aviso enviado aos jogadores");
                                System.Threading.Thread.Sleep(5000);
                                Random rnd = new Random();
                                int winnerIndex = rnd.Next(players.Count);
                                GamePlayer winner = players[winnerIndex];
                                foreach (GamePlayer player in players)
                                {
                                    player.SendMessage("O jogador " + winner.PlayerCharacter.NickName + " foi o sorteado!");
                                }
                                Console.WriteLine("O jogador " + winner.PlayerCharacter.NickName + " foi o sorteado!");
                            }
                            else
                            {
                                Console.WriteLine("Não há jogadores online para sortear.");
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        if (mensagem[1].Equals("kikardasala"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            if (player == null)
                            {
                                client.Player.SendMessage("Jogador não encontrado");
                                return 0;
                            }
                            if (player.CurrentRoom == null)
                            {
                                client.Player.SendMessage("Jogador não está em uma sala");
                                return 0;
                            }
                            RoomMgr.ExitRoom(player.CurrentRoom, player);
                            player.SendMessage("Você foi desconectado da sala por um administrador.");
                        }
                        if (mensagem[1].Equals("setarvidas"))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite o NickName do personagem: ");
                            string playerName = Console.ReadLine();
                            GamePlayer player = WorldMgr.GetAllPlayers().FirstOrDefault(p => p.PlayerCharacter.NickName == playerName);
                            if (player == null)
                            {
                                client.Player.SendMessage("Jogador não encontrado");
                                return 0;
                            }
                            player.PlayerCharacter.hp = 100000; // define a 10k
                            Console.WriteLine("A vida do jogador {0} foi definida a 10mil.", playerName);
                            Console.ForegroundColor = ConsoleColor.Green;
                        }


                        if (mensagem[1].Equals("cash"))
                        {
                            string nickName = mensagem[2];
                            int money = Convert.ToInt32(mensagem[3]);

                            if (String.IsNullOrEmpty(nickName))
                                return 0;

                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(nickName);
                            if (player == null)
                            {
                                client.Player.SendMessage("Jogador desconectado");
                                return 0;
                            }

                            player.AddMoney(money);
                            player.Extra.UpdateEventCondition(4, money); //money condition

                            if (!player.PlayerCharacter.IsRecharged)
                            {
                                player.PlayerCharacter.IsRecharged = true;
                                player.Out.SendUpdateFirstRecharge(player.PlayerCharacter.IsRecharged, player.PlayerCharacter.IsGetAward);
                            }

                            // Obter a data e hora atual
                            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            // Obter o nome de usuário ou apelido do jogador que usou o comando
                            string playerName = player.PlayerCharacter.NickName; // Para nome de usuário
                                                                                 // string playerName = player.PlayerCharacter.NickName; // Para apelido

                            // Abrindo ou criando arquivo para escrever
                            using (StreamWriter writer = new StreamWriter("recargas.txt", true))
                            {
                                // Escrevendo no arquivo com o timestamp e as informações adicionais
                                writer.WriteLine($"{timestamp} - {playerName} recarregou {money} de cash.");
                            }

                            player.SendMessage("Recarga no valor de " + money + " habilitada em sua conta.");
                        }

                        if (mensagem[1].Equals("all"))
                        {
                            string message = mensagem[2];
                            foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
                            {
                                gamePlayer.SendMessage(string.Format("DDTank Official: {0}", (object)message));
                            }
                            Console.WriteLine("Mensagem Enviada.");
                        }


                        if (mensagem[1].Equals("enviaritem"))
                        {
                            string nickname = mensagem[2];
                            int templateID = int.Parse(mensagem[3]);
                            int count = int.Parse(mensagem[4]);

                            // Encontra o jogador pelo nickname
                            GamePlayer gamePlayer = WorldMgr.GetClientByPlayerNickName(nickname);

                            // Verifica se o jogador foi encontrado
                            if (gamePlayer != null)
                            {
                                // Cria o item a ser enviado
                                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateID);
                                ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplate, count, 104);
                                itemInfo.Count = count;
                                itemInfo.StrengthenLevel = 0;
                                itemInfo.AttackCompose = 0;
                                itemInfo.DefendCompose = 0;
                                itemInfo.AgilityCompose = 0;
                                itemInfo.LuckCompose = 0;
                                itemInfo.IsBinds = false;
                                itemInfo.ValidDate = 0;

                                // Envia o item para o jogador através de um e-mail
                                if (gamePlayer.SendItemToMail(itemInfo, "Item recebido através de comando de administrador", "Item recebido por comando", eMailType.Default))
                                {
                                    gamePlayer.SendMessage("Você recebeu um item através de um comando.");
                                }
                                else
                                {
                                    Console.WriteLine($"Não foi possível enviar o item para {nickname}.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Jogador com nickname {nickname} não encontrado.");
                            }
                        }



                        if (mensagem[1].Equals("mensagem"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage(client.Player.PlayerCharacter.NickName + ": " + mensagem[2]);
                            }
                        }
                        if (mensagem[1].Equals("cupons"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.AddMoney(Int32.Parse(mensagem[2]));
                                player.SendMessage("Parabéns! Você tem muita sorte recebeu " + mensagem[2] + " cupons do evento ! Administração Agradece Você por Jogar no Nosso Servidor.");
                            }
                        }
                        if (mensagem[1].Equals("lcupons"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.AddGiftToken(Int32.Parse(mensagem[2]));
                                player.SendMessage("Parabéns! Você tem muita sorte recebeu " + mensagem[2] + " L.Cupons do evento ! Administração Agradece Você por Jogar no Nosso Servidor.");
                            }
                        }
                        if (mensagem[1].Equals("gold"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.AddGold(Int32.Parse(mensagem[2]));
                                player.SendMessage("Parabéns! Você tem muita sorte recebeu " + mensagem[2] + " moedas de ouro do evento ! Administração Agradece Você por Jogar no Nosso Servidor.");
                            }
                        }
                        if (mensagem[1].Equals("honra"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.AddHonor(Int32.Parse(mensagem[2]));
                                player.SendMessage("Parabéns! Você tem muita sorte recebeu " + mensagem[2] + " pontos de honras do evento ! Administração Agradece Você por Jogar no Nosso Servidor.");
                            }
                        }
                        if (mensagem[1].Equals("login"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            GSPacketIn @in1 = new GSPacketIn(0x25, client.Player.PlayerCharacter.ID);
                            @in1.WriteInt(client.Player.PlayerCharacter.ID);
                            @in1.WriteString(client.Player.PlayerCharacter.NickName);
                            @in1.WriteString("Sistema");
                            @in1.WriteString(" Login: " + player.PlayerCharacter.UserName);
                            @in1.WriteBoolean(false);
                            client.Player.SendTCP(@in1);
                        }

                        if (mensagem[1].Equals("banir"))
                        {
                            DateTime dt2 = new DateTime(2050, 07, 02);
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(mensagem[2], dt2, false);
                            }
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage("O Jogador <" + mensagem[2] + "> foi banido do servidor por motivos futeis, caso queira questionar ou denunciar algo em relação, entre em contato conosco. Administração agradece !");
                            }
                        }
                        if (mensagem[1].Equals("desbanir"))
                        {
                            DateTime dt2 = new DateTime(2050, 07, 02);
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(mensagem[2], dt2, true);
                            }
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage("O Jogador <" + mensagem[2] + "> foi desbanido do servidor por motivos reais que foram comprovados, caso queira questionar ou denunciar algo em relação, entre em contato conosco. Administração agradece !");
                            }
                        }
                        if (mensagem[1].Equals("kikar"))
                        {
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.KitoffUserByNickName(mensagem[2], " ");
                            }
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage("O Jogador <" + mensagem[2] + "> foi kikado do servidor por motivos futeis, caso queira questionar ou denunciar algo em relação, entre em contato conosco. Administração agradece !");
                            }
                        }
                        if (mensagem[1].Equals("kikartodos"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.Disconnect();
                            }
                        }




                        if (mensagem[1].Equals("onlines"))
                        {
                            string onlinePlayers = "Usuários online: ";
                            foreach (var player in WorldMgr.GetAllPlayers())
                            {
                                onlinePlayers += player.PlayerCharacter.NickName + ", ";
                            }
                            onlinePlayers = onlinePlayers.TrimEnd(',', ' ');

                            GSPacketIn @in1 = new GSPacketIn(0x25, client.Player.PlayerCharacter.ID);
                            @in1.WriteInt(client.Player.PlayerCharacter.ID);
                            @in1.WriteString(client.Player.PlayerCharacter.NickName);
                            @in1.WriteString("Sistema");
                            @in1.WriteString(onlinePlayers);
                            @in1.WriteBoolean(false);
                            client.Player.SendTCP(@in1);
                        }
                        if (mensagem[1].Equals("ban"))
                        {
                            DateTime dt2 = new DateTime(2050, 07, 02);
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(mensagem[2], dt2, false);
                            }
                        }
                        if (mensagem[1].Equals("tirarbanir"))
                        {
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(mensagem[2], DateTime.Now, true);
                            }
                        }


                        if (mensagem[1].Equals("mute"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            player.PlayerCharacter.IsBanChat = true;
                            player.PlayerCharacter.BanChat = 1;
                            player.PlayerCharacter.BanChatEndDate = DateTime.Now.AddHours(24);
                        }
                        if (mensagem[1].Equals("desmutar"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            player.PlayerCharacter.IsBanChat = false;
                            player.PlayerCharacter.BanChat = 0;
                            player.PlayerCharacter.BanChatEndDate = DateTime.MinValue;
                            player.SendMessage("Seu chat foi liberado. Divirta-se!");
                        }


                        if (mensagem[1].Equals("banchat"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            player.PlayerCharacter.IsBanChat = true;
                            player.PlayerCharacter.BanChat = 1;
                            player.PlayerCharacter.BanChatEndDate = DateTime.Now.AddHours(2);
                            foreach (GamePlayer msg in WorldMgr.GetAllPlayers())
                            {
                                msg.SendMessage("O jogador " + mensagem[2] + " teve o seu chat banido por 2 hora por motivos fulteis. Administração Agradece. ");
                            }
                        }
                        if (mensagem[1].Equals("vitalidade"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                //player.AddMissionEnergy(Int32.Parse(mensagem[2]));
                                player.SendMessage("Parabéns! Você tem muita sorte recebeu " + mensagem[2] + " pontos de vitalidade do evento ! Administração Agradece Você por Jogar no Nosso Servidor.");
                            }
                        }
                        if (mensagem[1].Equals("upado"))
                        {
                            int TemplateID = int.Parse(mensagem[2]);
                            int Count = int.Parse(mensagem[3]);
                            int StrengthenLevel = int.Parse(mensagem[4]);
                            int AttackCompose = int.Parse(mensagem[5]);
                            int DefendCompose = int.Parse(mensagem[6]);
                            int AgilityCompose = int.Parse(mensagem[7]);
                            int LuckCompose = int.Parse(mensagem[8]);

                            ItemInfo ItemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(TemplateID), Count, 101);
                            List<ItemInfo> list = new List<ItemInfo>();
                            ItemInfo.Count = Count;
                            ItemInfo.StrengthenLevel = StrengthenLevel;
                            ItemInfo.AttackCompose = AttackCompose;
                            ItemInfo.DefendCompose = DefendCompose;
                            ItemInfo.AgilityCompose = AgilityCompose;
                            ItemInfo.LuckCompose = LuckCompose;
                            ItemInfo.IsBinds = false;
                            ItemInfo.ValidDate = 0;
                            list.Add(ItemInfo);

                            foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
                            {
                                client.Player.SendItemsToMail(list, "Item recebido através de comando de administrador", "Item recebido por comando", default);
                                client.Player.SendMessage("Você recebeu um item através de comando.");
                            }
                        }


                        if (mensagem[1].Equals("titulo"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            //  player.Rank.AddNewRank(Int32.Parse(mensagem[3]));
                            player.Rank.SaveToDatabase();
                        }
                        //Novo Comando//
                        if (mensagem[1].Equals("bugcartas"))
                        {
                            RoomMgr.ExitRoom(client.Player.CurrentRoom, client.Player);
                        }
                        //Fim do Novo Comando//
                        if (mensagem[1].Equals("comandos"))
                        {
                            client.Player.SendMessage("----- Todos Comandos -----");
                            client.Player.SendMessage("");
                            client.Player.SendMessage("@@;mensagem;escreva");
                            client.Player.SendMessage("@@;reload");
                            client.Player.SendMessage("@@;item;templateid;quantidade");
                            client.Player.SendMessage("@@;cupons;...");
                            client.Player.SendMessage("@@;lcupons;...");
                            client.Player.SendMessage("@@;gold;...");
                            client.Player.SendMessage("@@;honra;...");
                            client.Player.SendMessage("@@;login;...");
                            client.Player.SendMessage("@@;banir;...");
                            client.Player.SendMessage("@@;desbanir;nickname");
                            client.Player.SendMessage("@@;kikar;nickname");
                            client.Player.SendMessage("@@;kikartodos;...");
                            client.Player.SendMessage("@@;fugura;...");
                            client.Player.SendMessage("@@;onlines");
                            client.Player.SendMessage("@@;banchat;nickname");
                            client.Player.SendMessage("@@;vitalidade;...");
                            client.Player.SendMessage("@@;titulo;...;...");
                            client.Player.SendMessage("@@;bugcartas");
                            client.Player.SendMessage("@@;recharge;nickname;value");
                            client.Player.SendMessage("@@;giveall;templateid;quantidade");
                            client.Player.SendMessage("@@;kikardasala;NickName");
                            client.Player.SendMessage("@@;ban;NickName");
                            client.Player.SendMessage("@@;tirarban;NickName");
                            client.Player.SendMessage("@@;mute;NickName");
                            client.Player.SendMessage("@@;tirarmute;NickName");
                        }
                    }

                    else if (verificarMod(client.Player.PlayerCharacter.NickName))
                    {
                        if (mensagem[1].Equals("mensagem"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage(client.Player.PlayerCharacter.NickName + ": " + mensagem[2]);
                            }
                        }
                        if (mensagem[1].Equals("login"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            GSPacketIn @in1 = new GSPacketIn(0x25, client.Player.PlayerCharacter.ID);
                            @in1.WriteInt(client.Player.PlayerCharacter.ID);
                            @in1.WriteString(client.Player.PlayerCharacter.NickName);
                            @in1.WriteString("Sistema");
                            @in1.WriteString(" Login: " + player.PlayerCharacter.UserName);
                            @in1.WriteBoolean(false);
                            client.Player.SendTCP(@in1);
                        }
                        if (mensagem[1].Equals("banir"))
                        {
                            DateTime dt2 = new DateTime(2050, 07, 02);
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(mensagem[2], dt2, false);
                            }
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage("O Jogador <" + mensagem[2] + "> foi banido do servidor por motivos futeis, caso queira questionar ou denunciar algo em relação, entre em contato conosco. Administração agradece !");
                            }
                        }
                        if (mensagem[1].Equals("desbanir"))
                        {
                            DateTime dt2 = new DateTime(2050, 07, 02);
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(mensagem[2], dt2, true);
                            }
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage("O Jogador <" + mensagem[2] + "> foi desbanido do servidor por motivos reais que foram comprovados, caso queira questionar ou denunciar algo em relação, entre em contato conosco. Administração agradece !");
                            }
                        }
                        if (mensagem[1].Equals("kikar"))
                        {
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.KitoffUserByNickName(mensagem[2], " ");
                            }
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                player.SendMessage("O Jogador <" + mensagem[2] + "> foi kikado do servidor por motivos futeis, caso queira questionar ou denunciar algo em relação, entre em contato conosco. Administração agradece !");
                            }
                        }
                        if (mensagem[1].Equals("onlines"))
                        {
                            GSPacketIn @in1 = new GSPacketIn(0x25, client.Player.PlayerCharacter.ID);
                            @in1.WriteInt(client.Player.PlayerCharacter.ID);
                            @in1.WriteString(client.Player.PlayerCharacter.NickName);
                            @in1.WriteString("Sistema");
                            @in1.WriteString(" Usuarios online: " + WorldMgr.GetAllPlayers().Length);
                            @in1.WriteBoolean(false);
                            client.Player.SendTCP(@in1);
                        }
                        if (mensagem[1].Equals("banchat"))
                        {
                            GamePlayer player = WorldMgr.GetClientByPlayerNickName(mensagem[2]);
                            player.PlayerCharacter.IsBanChat = true;
                            player.PlayerCharacter.BanChat = 1;
                            player.PlayerCharacter.BanChatEndDate = DateTime.Now.AddHours(1);
                            foreach (GamePlayer msg in WorldMgr.GetAllPlayers())
                            {
                                msg.SendMessage("O jogador " + mensagem[2] + " teve o seu chat banido por 1 hora por motivos fulteis. Administração Agradece. ");
                            }
                        }
                        if (mensagem[1].Equals("vitalidade"))
                        {
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                // player.AddMissionEnergy(Int32.Parse(mensagem[2]));
                                player.SendMessage("Parabéns! Você tem muita sorte recebeu " + mensagem[2] + " pontos de vitalidade do evento ! Administração Agradece Você por Jogar no Nosso Servidor.");
                            }
                        }


                        // test bro
                        if (mensagem[1].Equals("comandos"))
                        {
                            client.Player.SendMessage("----- Todos Comandos -----");
                            client.Player.SendMessage("");
                            client.Player.SendMessage("@@;mensagem;...");
                            client.Player.SendMessage("@@;reload");
                            client.Player.SendMessage("@@;item;templateid;count");
                            client.Player.SendMessage("@@;cupons;...");
                            client.Player.SendMessage("@@;lcupons;...");
                            client.Player.SendMessage("@@;gold;...");
                            client.Player.SendMessage("@@;honra;...");
                            client.Player.SendMessage("@@;login;...");
                            client.Player.SendMessage("@@;banir;...");
                            client.Player.SendMessage("@@;desbanir;...");
                            client.Player.SendMessage("@@;kikar;...");
                            client.Player.SendMessage("@@;kikartodos;...");
                            client.Player.SendMessage("@@;fugura;...");
                            client.Player.SendMessage("@@;onlines");
                            client.Player.SendMessage("@@;banchat;...");
                            client.Player.SendMessage("@@;vitalidade;...");
                            client.Player.SendMessage("@@;titulo;...;...");
                            client.Player.SendMessage("@@;bugcartas");
                            client.Player.SendMessage("@@;recharge;nickname;value");
                            client.Player.SendMessage("");
                        }
                    }


                }
            }
            if (client.Player.PlayerCharacter.IsBanChat)
            {
                //  client.Out.SendMessage(eMessageType, LanguageMgr.GetTranslation("ConsortiaChatHandler.IsBanChat", new object[0]));
                return 1;
            }
            if (client.Player.PlayerCharacter.Grade < 12)
            {
                client.Player.SendMessage("Você precisa ter nível 12 ou superior para falar no chat.");
                return 1;
            }

            if (client.Player.CurrentRoom != null && client.Player.CurrentRoom.RoomType == eRoomType.Match && client.Player.CurrentRoom.Game != null)
            {
                client.Player.CurrentRoom.BattleServer.Server.SendChatMessage(text, client.Player, flag);
                return 1;
            }
            if (b == 3)
            {
                if (client.Player.PlayerCharacter.ConsortiaID == 0)
                {
                    return 0;
                }
                gSPacketIn.WriteInt(client.Player.PlayerCharacter.ConsortiaID);
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                GamePlayer[] array = allPlayers;
                for (int i = 0; i < array.Length; i++)
                {
                    GamePlayer gamePlayer = array[i];
                    if (gamePlayer.PlayerCharacter.ConsortiaID == client.Player.PlayerCharacter.ConsortiaID && !gamePlayer.IsBlackFriend(client.Player.PlayerCharacter.ID))
                    {
                        gamePlayer.Out.SendTCP(gSPacketIn);
                    }
                }
            }
            else if (b == 9)
            {
                if (client.Player.CurrentMarryRoom == null)
                {
                    return 1;
                }
                client.Player.CurrentMarryRoom.SendToAllForScene(gSPacketIn, client.Player.MarryMap);
            }
            else if (client.Player.CurrentRoom != null)
            {
                if (flag)
                {
                    client.Player.CurrentRoom.SendToTeam(gSPacketIn, client.Player.CurrentRoomTeam, client.Player);
                }
                else
                {
                    client.Player.CurrentRoom.SendToAll(gSPacketIn);
                }
            }
            else
            {
                if (System.DateTime.Compare(client.Player.LastChatTime.AddSeconds(1.0), System.DateTime.Now) > 0 && b == 5)
                {
                    return 1;
                }
                if (flag)
                {
                    return 1;
                }
                if (System.DateTime.Compare(client.Player.LastChatTime.AddSeconds(30.0), System.DateTime.Now) > 0)
                {
                    // client.Out.SendMessage(eMessageTyp, LanguageMgr.GetTranslation("SceneChatHandler.Fast", new object[0]));
                    return 1;
                }
                client.Player.LastChatTime = System.DateTime.Now;
                GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
                GamePlayer[] array2 = allPlayers2;
                for (int j = 0; j < array2.Length; j++)
                {
                    GamePlayer gamePlayer2 = array2[j];
                    if (gamePlayer2.CurrentRoom == null && gamePlayer2.CurrentMarryRoom == null && !gamePlayer2.IsBlackFriend(client.Player.PlayerCharacter.ID))
                    {
                        gamePlayer2.Out.SendTCP(gSPacketIn);
                    }
                }
            }
            return 1;
        }
        public SceneChatHandler()
        {
        }
        public Boolean verificarAdm(String nick)
        {
            Boolean retorno = false;
            string[] lines = System.IO.File.ReadAllLines(@"ddtADM.txt");
            lines[0] = lines[1];
            foreach (String nick2 in lines)
            {
                if (nick.ToLower().Equals(nick2))
                {
                    retorno = true;
                }
            }
            return retorno;
        }
        public Boolean verificarMod(String nick1)
        {
            Boolean retorno1 = false;
            string[] lines1 = System.IO.File.ReadAllLines(@"configMod.txt");
            lines1[0] = lines1[1];
            foreach (String nick3 in lines1)
            {
                if (nick1.ToLower().Equals(nick3))
                {
                    retorno1 = true;
                }
            }
            return retorno1;
        }
        public Boolean verificarComando(String nick)
        {
            Boolean retorno = false;
            string[] lines = File.ReadAllLines(@"configplayerscomandos.txt");
            foreach (String nick2 in lines)
            {
                string[] linha = nick2.Split(';');
                if (nick.ToLower().Equals(linha[0]) || linha[0].Equals("players"))
                {

                }
            }
            return retorno;
        }
    }
}
