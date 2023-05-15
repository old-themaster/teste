using System.Collections.Generic;

namespace Game.Server.Managers
{

    public class ComandosMgr
	{
		private static Dictionary<int, List<string>> comandos;

		public static bool verificarComando(int userid, string comando)
		{
			return (comandos.ContainsKey(-1) && (comandos[-1].Contains(comando) || comandos[-1].Contains("allComands"))) || (comandos.ContainsKey(userid) && (comandos[userid].Contains(comando) || comandos[userid].Contains("allComands")));
		}

		public static bool checkStaff(int userid)
		{
			return comandos.ContainsKey(userid);
		}

		public static List<string> getComandosForUserId(int userid)
		{
			if (comandos.ContainsKey(userid))
			{
				return comandos[userid];
			}
			return null;
		}

		public static bool Setup()
		{
			//comandos = new PlayerBussiness().loadComandos();
			return comandos != null && comandos.Count > 0;
		}
	}
}
