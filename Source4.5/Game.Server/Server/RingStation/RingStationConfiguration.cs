using System.Threading;

namespace Game.Server.RingStation
{
    public class RingStationConfiguration
    {
        public static int PlayerID = 3000;
        public static string[] RandomName = new string[15]
         {
      "Errou morreu",
      "Mata Noob",
      "Luna",
      "Apolo",
      "TheMaster",
      "Zeus",
      "Aquiles",
      "Nescauzera1337",
      "Cibely",
      "Afrodite",
      "Bot",
      "Ronaldinho",
      "DDTank",
      "Maya",
      "LuvaDePedreiro"
        };
        public static int roomID = 3000;
        public static int ServerID = 104;
        public static string ServerName = "Bot automantico";

        public static int NextPlayerID() => Interlocked.Increment(ref RingStationConfiguration.PlayerID);

        public static int NextRoomId() => Interlocked.Increment(ref RingStationConfiguration.roomID);
    }
}
