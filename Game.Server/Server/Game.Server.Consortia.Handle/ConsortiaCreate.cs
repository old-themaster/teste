using System;
using System.Text;
using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Consortia.Handle
{
    [global::Consortia(1)]
	public class ConsortiaCreate : IConsortiaCommandHadler
	{
		public int CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			if (Player.PlayerCharacter.ConsortiaID != 0)
			{
				return 0;
			}
			ConsortiaLevelInfo consortiaLevelInfo = ConsortiaExtraMgr.FindConsortiaLevelInfo(1);
			string str = packet.ReadString();
			if (!string.IsNullOrEmpty(str) && Encoding.Default.GetByteCount(str) <= 12)
			{
				bool val = false;
				int num1 = 0;
				int needGold = consortiaLevelInfo.NeedGold;
				int num2 = 500;
				int num3 = 5;
				string msg = "ConsortiaCreateHandler.Failed";
				ConsortiaDutyInfo dutyInfo = new ConsortiaDutyInfo();
				if (!string.IsNullOrEmpty(str) && Player.PlayerCharacter.Gold >= needGold && Player.PlayerCharacter.Grade >= num3 && Player.PlayerCharacter.Money + Player.PlayerCharacter.MoneyLock >= num2)
				{
					using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
					{
						ConsortiaInfo info = new ConsortiaInfo();
						info.BuildDate = DateTime.Now;
						info.CelebCount = 0;
						info.ChairmanID = Player.PlayerCharacter.ID;
						info.ChairmanName = Player.PlayerCharacter.NickName;
						info.ConsortiaName = str;
						info.CreatorID = info.ChairmanID;
						info.CreatorName = info.ChairmanName;
						info.Description = "";
						info.Honor = 0;
						info.IP = "";
						info.IsExist = true;
						info.Level = consortiaLevelInfo.Level;
						info.MaxCount = consortiaLevelInfo.Count;
						info.Riches = consortiaLevelInfo.Riches;
						info.Placard = "";
						info.Port = 0;
						info.Repute = 0;
						info.Count = 1;
						if (consortiaBussiness.AddConsortia(info, ref msg, ref dutyInfo))
						{
							Player.PlayerCharacter.ConsortiaID = info.ConsortiaID;
							Player.PlayerCharacter.ConsortiaName = info.ConsortiaName;
							Player.PlayerCharacter.DutyLevel = dutyInfo.Level;
							Player.PlayerCharacter.DutyName = dutyInfo.DutyName;
							Player.PlayerCharacter.Right = dutyInfo.Right;
							Player.PlayerCharacter.ConsortiaLevel = consortiaLevelInfo.Level;
							Player.RemoveGold(needGold);
							Player.RemoveMoney(num2);
							msg = "ConsortiaCreateHandler.Success";
							val = true;
							num1 = info.ConsortiaID;
							GameServer.Instance.LoginServer.SendConsortiaCreate(num1, Player.PlayerCharacter.Offer, info.ConsortiaName);
						}
					}
				}
				GSPacketIn packet2 = new GSPacketIn(129);
				packet2.WriteByte(1);
				packet2.WriteString(str);
				packet2.WriteBoolean(val);
				packet2.WriteInt(num1);
				packet2.WriteString(str);
				packet2.WriteString(LanguageMgr.GetTranslation(msg));
				packet2.WriteInt(dutyInfo.Level);
				packet2.WriteString((dutyInfo.DutyName == null) ? "" : dutyInfo.DutyName);
				packet2.WriteInt(dutyInfo.Right);
				Player.Out.SendTCP(packet2);
				return 0;
			}
			Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ConsortiaCreateHandler.Long"));
			return 1;
		}
	}
}
