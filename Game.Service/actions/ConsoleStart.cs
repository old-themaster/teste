using System;
using log4net;
using System.Collections;
using Game.Server;
using System.Reflection;
using System.Runtime.InteropServices;
using Game.Server.Managers;
using System.Threading;
using Game.Server.Packets;
using Bussiness.Managers;
using Bussiness;
using Game.Server.Rooms;
using Game.Logic;
using Game.Base;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;

namespace Game.Service.actions
{
    public class ConsoleStart : IAction
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Name
        {
            get { return "--start"; }
        }


        public string Syntax
        {
            get { return "--start [-config=./config/serverconfig.xml]"; }
        }


        public string Description
        {
            get { return "Starts the DOL server in console mode"; }
        }

        public void OnAction(Hashtable parameters)
        {
            ServiceClient serviceClient = new ServiceClient();
            Console.Title = ("DDtank v4.5 ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data de compilação: 19/11/2020");
            Console.WriteLine("Nota de atualização: 121");
            Console.WriteLine("programador : k4p3t4 ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("|| Conexão permitida com sucesso.");
            Console.WriteLine("|| Nome do computador: " + serviceClient.MachineName());
            Console.WriteLine("|| Nome de usuário: " + serviceClient.MachineUserName());
            Console.WriteLine("|| Endereço da execução: " + serviceClient.MachineAddress());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("|| Annuit Coeptis - ▲ - (0b1101-0b1101)");
            Console.WriteLine("|| From dust we are born, to dust we will return");
            Console.WriteLine("|| Glorious are those who are above all");
            Console.ResetColor();
            GameServer.CreateInstance(new GameServerConfig());
            GameServer.Instance.Start();
            FusionCombined.ListCombinedFusion();
            GameServer.KeepRunning = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Servidor Iniciado!");
            ConsoleClient client = new ConsoleClient();

            while (GameServer.KeepRunning)
            {
                try
                {
                    handler = ConsoleCtrHandler;
                    SetConsoleCtrlHandler(handler, true);

                    Console.Write("> ");
                    string line = Console.ReadLine();
                    string[] para = line.Split(' ');
                    switch (para[0])
                    {
                        case "exit":
                            GameServer.KeepRunning = false;
                            break;
                        case "manu":
                            _count = 6;
                            _timer = new Timer(new TimerCallback(ShutDownCallBack), null, 0, 60 * 1000);
                            break;
                        case "cp":
                            {
                                GameClient[] allClients = GameServer.Instance.GetAllClients();
                                int num2 = (allClients == null) ? 0 : allClients.Length;
                                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                                int num3 = (allPlayers == null) ? 0 : allPlayers.Length;
                                System.Collections.Generic.List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
                                int num4 = 0;
                                int num5 = 0;
                                foreach (BaseRoom current in allUsingRoom)
                                {
                                    if (!current.IsEmpty)
                                    {
                                        num4++;
                                        if (current.IsPlaying)
                                        {
                                            num5++;
                                        }
                                    }
                                }
                                double num6 = (double)System.GC.GetTotalMemory(false);
                                System.Console.WriteLine(string.Format("Total Clients/Players:{0}/{1}", num2, num3));
                                System.Console.WriteLine(string.Format("Total Rooms/Games:{0}/{1}", num4, num5));
                                System.Console.WriteLine(string.Format("Total Momey Used:{0} MB", num6 / 1024.0 / 1024.0));
                                break;
                            }
                        case "shutdown":
                            ConsoleStart._count = 6;
                            ConsoleStart._timer = new Timer(new TimerCallback(ConsoleStart.ShutDownCallBack), null, 0, 60000);
                            continue;
                        case "savemap":
                            break;
                        case "clear":
                            Console.Clear();
                            break;
                        case "ball":
                            if (BallMgr.ReLoad())
                                Console.WriteLine("Ball info is Reload!");
                            else
                                Console.WriteLine("Ball info is Error!");
                            break;
                        case "map":
                            if (MapMgr.ReLoadMap())
                                Console.WriteLine("Map info is Reload!");
                            else
                                Console.WriteLine("Map info is Error!");
                            break;
                        case "mapserver":
                            if (MapMgr.ReLoadMapServer())
                                Console.WriteLine("mapserver info is Reload!");
                            else
                                Console.WriteLine("mapserver info is Error!");
                            break;
                        case "prop":
                            if (PropItemMgr.Reload())
                                Console.WriteLine("prop info is Reload!");
                            else
                                Console.WriteLine("prop info is Error!");
                            break;
                        case "item":
                            if (ItemMgr.ReLoad())
                                Console.WriteLine("item info is Reload!");
                            else
                                Console.WriteLine("item info is Error!");
                            break;
                        case "cupons":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite a quantidade de cupons:");
                            int int32_2 = Convert.ToInt32(Console.ReadLine());
                            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                            {
                                allPlayer.AddMoney(int32_2);
                                allPlayer.SendMessage("Todos os jogadores online receberam " + int32_2.ToString() + " de cupons.");
                                Console.WriteLine("{0} de Cupons enviado com sucesso", (object)int32_2);
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            break;

                        case "banir":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite o NickName do personagem: ");
                            string bnickname = Console.ReadLine();
                            Console.WriteLine("Motivo para banir: ");
                            string breason = Console.ReadLine();
                            DateTime dt2 = new DateTime(2050, 07, 02); //Tempo de banimento
                            using (ManageBussiness managebussiness = new ManageBussiness())
                            {
                                managebussiness.ForbidPlayerByNickName(bnickname, dt2, false);
                            }
                            Console.WriteLine("O Usuário " + bnickname + " Foi Banido do servidor.");

                            break;
                        case "kikartodos":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Expulsando jogadores...");
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                if (player.PlayerCharacter.ID != 1)
                                {
                                    player.Disconnect();
                                }
                            }
                            break;



                        case "kikar":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite o NickName do personagem: ");
                            string userSingleByNickName = Console.ReadLine();
                            using (ManageBussiness mg = new ManageBussiness())
                            {
                                mg.KitoffUserByNickName(userSingleByNickName, " ");
                            }
                            break;

                        case "evento":
                            Console.WriteLine("Msg Enviado com Exito)");
                            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                                allPlayer.SendMessage("Evento Agendado! Fique atento com os anuncio na Pagina do Facebook (www.facebook.com/ddtankofc/) Obrigado por jogar DDTank.");
                            continue;
                        case "exp":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite a quantidade de Exp:");
                            int int32_4 = Convert.ToInt32(Console.ReadLine());
                            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                            {
                                allPlayer.AddGP(int32_4);
                                allPlayer.SendMessage("Todos os jogadores online receberam " + int32_4.ToString() + " de Exp.");
                                Console.WriteLine("{0} de EXP enviado com sucesso", (object)int32_4);
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            break;
                        case "totem":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite a quantidade de Pontos de Honra:");
                            int int32_30 = Convert.ToInt32(Console.ReadLine());
                            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                            {

                                allPlayer.SendMessage("Todos os jogadores online receberam" + int32_30.ToString() + " Pontos de Honra");
                                Console.WriteLine("Você recebeu {0} de pontos de Honra!", (object)int32_30);
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            break;
                        case "limpar":
                            Console.Clear();
                            break;
                        case "setarvidas":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Digite o NickName do personagem: ");
                            string playerName = Console.ReadLine();
                            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                            {
                                if (player.PlayerCharacter.NickName == playerName)
                                {
                                    player.PlayerCharacter.hp = 10000; // define a vida a 10mil
                                    Console.WriteLine("A vida do jogador {0} foi definida a 10mil.", playerName);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    break;
                                }
                            }
                            break;

                        case "atualizar":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Carregando novas informações");
                            try
                            {
                                Console.WriteLine("Servidor esta Coletando informações..");
                                GameProperties.Refresh();
                                Console.WriteLine("Atualizado Map :" + MapMgr.Init().ToString());
                                bool flag = ItemBoxMgr.Init();
                                Console.WriteLine("Atualizado ItemBox :" + flag.ToString());
                                flag = BallMgr.Init();
                                Console.WriteLine("Atualizado Ball :" + flag.ToString());
                                flag = ExerciseMgr.Init();
                                Console.WriteLine("Atualizado Exercise :" + flag.ToString());
                                flag = LevelMgr.Init();
                                Console.WriteLine("Atualizado Level :" + flag.ToString());
                                flag = BallConfigMgr.Init();
                                Console.WriteLine("Atualizado BallConfig :" + flag.ToString());
                                flag = FusionMgr.Init();
                                Console.WriteLine("Atualizado Fusion :" + flag.ToString());
                                flag = AwardMgr.Init();
                                Console.WriteLine("Atualizado Award :" + flag.ToString());
                                flag = AchievementMgr.Init();
                                Console.WriteLine("Atualizado Achievement :" + flag.ToString());
                                flag = NPCInfoMgr.Init();
                                Console.WriteLine("Atualizado NPCInfo :" + flag.ToString());
                                flag = MissionInfoMgr.Init();
                                Console.WriteLine("Atualizado MissionInfo :" + flag.ToString());
                                flag = PveInfoMgr.Init();
                                Console.WriteLine("Atualizado PveInfo :" + flag.ToString());
                                flag = DropMgr.Init();
                                Console.WriteLine("Atualizado Drop :" + flag.ToString());
                                flag = FightRateMgr.Init();
                                Console.WriteLine("Atualizado FightRate :" + flag.ToString());
                                flag = RefineryMgr.Init();
                                Console.WriteLine("Atualizado Refinery :" + flag.ToString());
                                flag = StrengthenMgr.Init();
                                Console.WriteLine("Atualizado Strengthen :" + flag.ToString());
                                flag = PropItemMgr.Init();
                                Console.WriteLine("Atualizado Item PVE :" + flag.ToString());
                                flag = ShopMgr.Init();
                                Console.WriteLine("Atualizado Shop :" + flag.ToString());
                                flag = QuestMgr.Init();
                                Console.WriteLine("Atualizado Quest :" + flag.ToString());
                                flag = ConsortiaMgr.Init();
                                Console.WriteLine("Atualizado Guild :" + flag.ToString());
                                flag = EventAwardMgr.Init();
                                Console.WriteLine("Carregando EventAward :" + flag.ToString());
                                flag = CardMgr.Init();
                                Console.WriteLine("Atualizado Card :" + flag.ToString());
                                flag = PetMgr.Init();
                                Console.WriteLine("Atualizado Pet :" + flag.ToString());
                                flag = GoldEquipMgr.Init();
                                Console.WriteLine("Atualizado Commands" + flag.ToString());
                                flag = CommandsMgr.Init();
                                Console.WriteLine("Atualizado GoldEquip :" + flag.ToString());
                                flag = SubActiveMgr.Init();
                                Console.WriteLine("Atualizado SubActive :" + flag.ToString());
                                flag = EventAwardMgr.Init();
                                Console.WriteLine("Atualizado EventAward :" + flag.ToString());
                                flag = RankMgr.Init();
                                Console.WriteLine("Atualizado Title :" + flag.ToString());
                                flag = PetMgr.Init();
                                Console.WriteLine("Atualizado Pet :" + flag.ToString());
                                flag = ActiveMgr.Init();
                                Console.WriteLine("Activity Quest :" + flag.ToString());
                                flag = ActivityQuestMngr.Init();
                                Console.WriteLine("Atualizado Lista de Eventos:" + flag.ToString());
                                Console.WriteLine("Atualizado com sucesso!");
                            }
                            catch
                            {
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case "shop":

                            if (ShopMgr.ReLoad())
                                Console.WriteLine("shop info is Reload!");
                            else
                                Console.WriteLine("shop info is Error!");
                            break;
                        case "quest":
                            if (QuestMgr.ReLoad())
                                Console.WriteLine("quest info is Reload!");
                            else
                                Console.WriteLine("quest info is Error!");
                            break;
                        case "fusion":
                            if (FusionMgr.ReLoad())
                                Console.WriteLine("fusion info is Reload!");
                            else
                                Console.WriteLine("fusion info is Error!");
                            break;
                        case "consortia":
                            if (ConsortiaMgr.ReLoad())
                                Console.WriteLine("consortiaMgr info is Reload!");
                            else
                                Console.WriteLine("consortiaMgr info is Error!");
                            break;
                        case "rate":
                            if (RateMgr.ReLoad())
                                Console.WriteLine("Rate Rate is Reload!");
                            else
                                Console.WriteLine("Rate Rate is Error!");
                            break;
                        case "drop":
                            if (DropMgr.ReLoad())
                                Console.WriteLine("drop info is reloaded!");
                            else
                                Console.WriteLine("drop info has an error!");
                            break;
                        case "npc":
                            if (NPCInfoMgr.ReLoad())
                                Console.WriteLine("Npc Atualizado com sucesso!");
                            else
                                Console.WriteLine("Ocorreu alguém Erro!");
                            break;
                        case "fight":
                            if (FightRateMgr.ReLoad())
                                Console.WriteLine("FightRateMgr is Reload!");
                            else
                                Console.WriteLine("FightRateMgr is Error!");
                            break;
                        case "player":
                            WorldMgr.GetAllPlayers();
                            break;
                        case "dailyaward":
                            if (AwardMgr.ReLoad())
                                Console.WriteLine("dailyaward is Reload!");
                            else
                                Console.WriteLine("dailyaward is Error!");
                            break;
                        case "language":
                            if (LanguageMgr.Reload(""))
                                Console.WriteLine("language is Reload!");
                            else
                                Console.WriteLine("language is Error!");
                            break;
                        case "nickname":
                            Console.WriteLine("Por favor insira o apelido");
                            string nickname = Console.ReadLine();
                            string state = WorldMgr.GetPlayerStringByPlayerNickName(nickname);
                            Console.WriteLine(state);
                            break;

                        case "sorteio":
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
                            break;

                        case "paratodos":
                            {
                                Console.WriteLine("Por favor, digite o ID do item a ser enviado");
                                ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(int.Parse(Console.ReadLine()));
                                if (itemTemplateInfo == null)
                                {
                                    Console.WriteLine("O Item não existe.");
                                }
                                else
                                {
                                    Console.WriteLine("Por favor, digite a quantidade de itens a ser enviado");
                                    int count = int.Parse(Console.ReadLine());
                                    ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, count, 104);

                                    foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
                                    {
                                        if (gamePlayer.SendItemToMail(itemInfo, "Todos os Jogadores onlines receberam: " + itemInfo.Template.Name, "Administrador", eMailType.Default))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"Item:{itemInfo.Template.Name} enviado para {gamePlayer.PlayerCharacter.NickName} com sucesso!");
                                            Console.ResetColor();
                                        }
                                    }
                                }
                                continue;
                            }
                        case "senditem":
                            {
                                GamePlayer gamePlayer3 = null;
                                Console.WriteLine("Por favor, digite o tipo de envio, 0 = UserID, 1 = UserName, 2 = NickName:");
                                switch (int.Parse(Console.ReadLine()))
                                {
                                    case 0:
                                        Console.WriteLine("UserID:");
                                        gamePlayer3 = WorldMgr.GetPlayerById(int.Parse(Console.ReadLine()));
                                        break;
                                    case 1:
                                        Console.WriteLine("UserName:");
                                        gamePlayer3 = WorldMgr.GetClientByPlayerUserName(Console.ReadLine());
                                        break;
                                    case 2:
                                        Console.WriteLine("NickName:");
                                        gamePlayer3 = WorldMgr.GetClientByPlayerNickName(Console.ReadLine());
                                        break;
                                    default:
                                        Console.WriteLine("O tipo não existe");
                                        break;
                                }
                                if (gamePlayer3 == null)
                                {
                                    Console.WriteLine("O jogador não existe.");
                                }
                                else
                                {
                                    Console.WriteLine("Por favor, digite o ID do item a ser enviado");
                                    ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(int.Parse(Console.ReadLine()));
                                    ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, itemTemplateInfo.MaxCount, 104);
                                    if (itemTemplateInfo == null)
                                    {
                                        Console.WriteLine("O Item existe.");
                                    }
                                    else if (gamePlayer3.SendItemToMail(itemInfo, "Recompensas do sistema:  " + itemInfo.Template.Name, "Administrador", eMailType.Default))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"Item:{itemInfo.Template.Name}enviado para{gamePlayer3.PlayerCharacter.NickName}com sucesso!");
                                        Console.ResetColor();
                                    }
                                }
                                continue;
                            }

                        case "all":
                            Console.Clear();
                            Console.WriteLine("Digite A Sua Menssagem: ");
                            string str4 = Console.ReadLine();
                            foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
                                gamePlayer.SendMessage(string.Format("[Sistema]: {0}", (object)str4));
                            Console.WriteLine("Mesagem Enviada.");
                            continue;
                        default:
                            if (line.Length <= 0) break;
                            if (line[0] == '/')
                            {
                                line = line.Remove(0, 1);
                                line = line.Insert(0, "&");
                            }

                            try
                            {
                                bool res = CommandMgr.HandleCommandNoPlvl(client, line);
                                if (!res)
                                {
                                    Console.WriteLine("Comando desconhecido:" + line);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if (GameServer.Instance != null)
                GameServer.Instance.Stop();

            LogManager.Shutdown();
        }

        private static Timer _timer;
        private static int _count;

        private static void ShutDownCallBack(object state)
        {
            _count--;
            Console.WriteLine(string.Format("O servidor será encerrado após {0} minutos!", _count));
            GameClient[] list = GameServer.Instance.GetAllClients();
            foreach (GameClient c in list)
            {
                if (c.Out != null)
                {
                    c.Out.SendMessage(eMessageType.Normal, string.Format("{0}{1}{2}", LanguageMgr.GetTranslation("Game.Service.actions.ShutDown1"), _count, LanguageMgr.GetTranslation("Game.Service.actions.ShutDown2")));
                }
            }
            if (_count == 0)
            {
                _timer.Dispose();
                _timer = null;
                GameServer.Instance.Stop();
                Console.WriteLine("Servidor fechado!");
            }
        }


        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool add);

        private static ConsoleCtrlDelegate handler;

        private delegate int ConsoleCtrlDelegate(ConsoleEvent ctrlType);

        private static int ConsoleCtrHandler(ConsoleEvent e)
        {
            SetConsoleCtrlHandler(handler, false);
            if (GameServer.Instance != null)
                GameServer.Instance.Stop();
            return 0;
        }

        enum ConsoleEvent
        {
            Ctrl_C,
            Ctrl_Break,
            Close,
            Logoff,
            Shutdown
        }
    }
}
