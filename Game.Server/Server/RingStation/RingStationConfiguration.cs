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
	  "TioGordo",
	  "Bot",
	  "Ronaldinho",
	  "DDTank",
	  "Maya",
	  "LuvaDePedreiro"
		};
		public static int roomID = 3000;
		public static int ServerID = 104;
		public static string ServerName = "Bot automantico";

		public static int NextPlayerID()
		{
			int value = Interlocked.Increment(ref RingStationConfiguration.PlayerID);
			value = Interlocked.Increment(ref RingStationConfiguration.PlayerID);
			return value;
		}

		public static int NextRoomId() => Interlocked.Increment(ref RingStationConfiguration.roomID);
	}
}
