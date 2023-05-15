using SqlDataProvider.Data;
using System;
using System.Threading;

namespace Game.Server.Managers
{
    public class EventNumber
    {
        public static int Event = 0;
        public static void MsgAll(string msg)
        {
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                player.SendMessage("[EVENTO] " + msg);
            }

        }
        public static void EventRun()
        {
            Event = 1;
            Console.WriteLine(Event);
            MsgAll("O Evento vai começar dentro de 10 Segundos");
            Thread.Sleep(5000);
            MsgAll("O Evento vai começar dentro de 5 Segundos");
            Thread.Sleep(5000);
            MsgAll("O Evento começou tentem descobrir numero de 1 a 200");
            Thread.Sleep(3000);
            MsgAll("Para tentar descobrir o numero é so digitar no chat o numero");
        }
        public static void FinishEvent(GameClient client)
        {
            Event = 0;
            MsgAll("Temos um vencedor: " + client.Player.PlayerCharacter.NickName);
            ItemTemplateInfo PET = new ItemTemplateInfo();
            PET.CategoryID = 35;
            PET.TemplateID = 35140401;
            PET.Level = 5; // KKKK TEST
            ItemInfo itemInfo = ItemInfo.CreateFromTemplate(PET, 3, 0);
            client.Player.SendItemToMail(itemInfo, "Parabéns você ganhou o evento do DDTank II, Evento criado por SkelletonX", "DDTank II Evento", Packets.eMailType.ItemOverdue);
        }
    }
}