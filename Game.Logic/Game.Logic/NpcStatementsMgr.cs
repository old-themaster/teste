using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Game.Logic
{
	public class NpcStatementsMgr
	{
		private static string filePath;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static List<string> m_npcstatement = new List<string>();

		private static Random random;

		public static string GetRandomStatement()
		{
			int index = random.Next(0, m_npcstatement.Count);
			return m_npcstatement[index];
		}

		public static string GetStatement(int index)
		{
			if (index < 0 || index > m_npcstatement.Count)
			{
				return null;
			}
			return m_npcstatement[index];
		}

		public static bool Init()
		{
			filePath = Directory.GetCurrentDirectory() + "\\ai\\npc\\npc_statements.txt";
			random = new Random();
			return ReLoad();
		}

		public static string[] RandomStatement(int count)
		{
			string[] array = new string[count];
			int[] array2 = RandomStatementIndexs(count);
			for (int i = 0; i < count; i++)
			{
				int index = array2[i];
				array[i] = m_npcstatement[index];
			}
			return array;
		}

		public static int[] RandomStatementIndexs(int count)
		{
			int[] array = new int[count];
			int num = 0;
			while (num < count)
			{
				int num2 = random.Next(0, m_npcstatement.Count);
				if (!array.Contains(num2))
				{
					array[num] = num2;
					num++;
				}
			}
			return array;
		}

		public static bool ReLoad()
		{
			try
			{
				string empty = string.Empty;
				StreamReader streamReader = new StreamReader(filePath, Encoding.Default);
				while (!string.IsNullOrEmpty(empty = streamReader.ReadLine()))
				{
					m_npcstatement.Add(empty);
				}
				return true;
			}
			catch (Exception exception)
			{
				log.Error("NpcStatementsMgr.Reload()", exception);
				return false;
			}
		}
	}
}
