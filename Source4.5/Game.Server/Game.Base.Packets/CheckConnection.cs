using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Game.Base.Packets
{
	public class CheckConnection
	{
		private static List<ConnectionList> connection = new List<ConnectionList>();

		private static List<string> ban = new List<string>();

		private static string NetSh(string A)
		{
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo("netsh", A);
				processStartInfo.RedirectStandardOutput = true;
				processStartInfo.UseShellExecute = false;
				processStartInfo.CreateNoWindow = true;
				Process process = new Process();
				process.StartInfo = processStartInfo;
				process.Start();
				process.WaitForExit();
				return process.StandardOutput.ReadToEnd().Trim().Replace("\r", "")
					.Replace("\n", "");
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public static string addbanlist(string Name, string Address)
		{
			using (StreamWriter w = File.AppendText("logConnections.txt"))
			{
				Log("Ataque Bloqueado, Endereço da Conexão: " + Name, w);
			}
			return NetSh($"ipsec static add filter filterlist=\"{Name}\" srcaddr={Address} dstaddr=me protocol=any");
		}

		public static void Log(string logMessage, TextWriter w, bool flag = false)
		{
			w.Write("\r\nLog Entry : ");
			w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
			w.WriteLine("  :");
			w.WriteLine("  :{0}", logMessage);
			w.WriteLine("-------------------------------");
		}

		public static void AddList(string _ip, BaseClient client, Socket socket)
		{
			int count = 0;
			connection.Add(new ConnectionList
			{
				ip = _ip,
				conntime = DateTime.Now
			});
			connection.ForEach(delegate (ConnectionList a)
			{
				if (a.ip == _ip)
				{
					int num = Convert.ToInt32((DateTime.Now - a.conntime).TotalSeconds);
					if (num <= 3)
					{
						count++;
					}
				}
			});
			if (count >= 3 && BanListCount(_ip) <= 0)
			{
				socket.Close();
				ban.Add(_ip);
				addbanlist("KiwiGuard Block Filter List", _ip);
				client.Disconnect();
			}
		}

		public static int BanListCount(string ip)
		{
			return ban.Count((string A) => A == ip);
		}
	}
}
