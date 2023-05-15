using Game.Base.Packets;
using Game.Logic.Cmd;
using Game.Logic.Phy.Object;
using log4net;
using System;
using System.Reflection;

namespace Game.Logic.Actions
{
	public class ProcessPacketAction : IAction
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private GSPacketIn m_packet;

		private Player m_player;

		public ProcessPacketAction(Player player, GSPacketIn pkg)
		{
			m_player = player;
			m_packet = pkg;
		}

		public void Execute(BaseGame game, long tick)
		{
			if (m_player.IsActive)
			{
				eTankCmdType eTankCmdType = (eTankCmdType)m_packet.ReadByte();
				try
				{
					ICommandHandler commandHandler = CommandMgr.LoadCommandHandler((int)eTankCmdType);
					if (commandHandler != null)
					{
						commandHandler.HandleCommand(game, m_player, m_packet);
					}
					else
					{
						log.Error($"Player Id: {m_player.Id}");
					}
				}
				catch (Exception exception)
				{
					log.Error($"Player Id: {m_player.Id}  cmd:0x{(byte)eTankCmdType:X2}", exception);
				}
			}
		}

		public bool IsFinished(long tick)
		{
			return true;
		}
	}
}
