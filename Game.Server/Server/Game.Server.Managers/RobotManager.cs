using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Managers
{
    public class RobotManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static int CurRobotID = -1000000;

       // private static Dictionary<int, RobotGamePlayer> RobotGamePlayers = new Dictionary<int, RobotGamePlayer>();






        public class Robot
        {
            public string Name { get; set; } = "Robot";
            public int Level { get; set; } = 20;
            public int GP { get; set; } = 0;
            public bool Sex { get; set; }
            public int UserType { get; set; } = 1;
            public int VIPLevel { get; set; }
            public byte typeVIP { get; set; } = 2;
            public byte State { get; set; } = 1;
            public byte masterID { get; set; } = 0;
            public byte apprenticeshipState { get; set; } = 1;
            public string VIPExpireDay { get; set; } = new DateTime(2018, 1, 1).ToString();
            public List<Equip> Equips { get; set; } = new List<Equip> { new Equip() };

        }
        public class Equip
        {
            public int ID { get; set; } = 7001;
            public int Strength { get; set; } = 0;
            public int Compose { get; set; } = 0;
        }

        public class RobotRoom
        {
            public int PlayerCount { get; set; }
            public int MaxPlayerCount { get; set; }
            public string RoomName { get; set; } = "Robot Room Name";
            public int RoomType { get; set; }
        }
    }
}
