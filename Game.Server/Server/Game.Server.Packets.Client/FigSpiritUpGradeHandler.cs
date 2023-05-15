using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
    [PacketHandler(209, "场景用户离开")]
    public class FigSpiritUpGradeHandler : IPacketHandler
    {
        private static int[] int_0 = FightSpiritTemplateMgr.Exps();
        private static int int_1 = GameProperties.FightSpiritMaxLevel;

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.PlayerCharacter.Grade < 30)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, "O nível 30 pode usar guerra");
                return 0;
            }
            int id = client.Player.PlayerCharacter.ID;
            int num1 = (int)packet.ReadByte();
            int num2 = packet.ReadInt();
            packet.ReadInt();
            packet.ReadInt();
            int templateId = packet.ReadInt();
            int num3 = packet.ReadInt();
            int place = packet.ReadInt();
            int int_2 = packet.ReadInt();
            packet.ReadInt();
            SqlDataProvider.Data.ItemInfo itemByTemplateId = client.Player.PropBag.GetItemByTemplateID(0, templateId);
            int itemCount = client.Player.PropBag.GetItemCount(templateId);
            UserGemStone gemStone = client.Player.GetGemStone(place);
            if (gemStone == null)
            {
                client.Player.Out.SendPlayerFigSpiritUp(id, gemStone, false, true, true, 0, 0);
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Sucesso!"));
                return 0;
            }
            string[] string_0 = gemStone.FigSpiritIdValue.Split('|');
            bool flag1 = false;
            bool isMaxLevel = this.method_1(string_0);
            bool isFall = true;
            int num4 = 1;
            int dir = 0;
            FigSpiritUpInfo[] figSpiritUpInfo_0 = new FigSpiritUpInfo[string_0.Length];
            for (int index = 0; index < string_0.Length; ++index)
            {
                FigSpiritUpInfo figSpiritUpInfo = new FigSpiritUpInfo()
                {
                    level = Convert.ToInt32(string_0[index].Split(',')[0]),
                    exp = Convert.ToInt32(string_0[index].Split(',')[1]),
                    place = Convert.ToInt32(string_0[index].Split(',')[2])
                };
                figSpiritUpInfo_0[index] = figSpiritUpInfo;
            }
            if (itemCount <= 0 || itemByTemplateId == null)
            {
                client.Player.Out.SendPlayerFigSpiritUp(id, gemStone, flag1, isMaxLevel, isFall, 0, dir);
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Sucesso!"));
                return 0;
            }
            if (!itemByTemplateId.isGemStone())
            {
                client.Player.Out.SendPlayerFigSpiritUp(id, gemStone, flag1, isMaxLevel, isFall, 0, dir);
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Sucesso!"));
                return 0;
            }
            if (isMaxLevel)
            {
                client.Player.Out.SendPlayerFigSpiritUp(id, gemStone, flag1, isMaxLevel, isFall, 0, dir);
                return 0;
            }
            FigSpiritUpInfo figSpiritUpInfo_1 = this.method_3(figSpiritUpInfo_0, int_2);
            if (figSpiritUpInfo_1 == null)
            {
                client.Player.Out.SendPlayerFigSpiritUp(id, gemStone, flag1, isMaxLevel, isFall, 0, dir);
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Erro de Local incorreto detectado, entra em contato com o GM do servidor!"));
                return 0;
            }
            int num5;
            if (num2 != 1)
            {
                num5 = itemByTemplateId.Template.Property2;
                client.Player.PropBag.RemoveCountFromStack(itemByTemplateId, 1);
            }
            else
            {
                int count = this.method_0(figSpiritUpInfo_1.exp, figSpiritUpInfo_1.level) / itemByTemplateId.Template.Property2;
                if (itemCount < count)
                    count = itemCount;
                num5 = itemByTemplateId.Template.Property2 * count;
                client.Player.PropBag.RemoveTemplate(templateId, count);
            }
            if (figSpiritUpInfo_1.level < FigSpiritUpGradeHandler.int_1)
            {
                figSpiritUpInfo_1.exp += num5;
                bool flag2 = this.method_2(figSpiritUpInfo_1.exp, figSpiritUpInfo_1.level);
                flag1 = flag2;
                if (flag2)
                {
                    ++figSpiritUpInfo_1.level;
                    figSpiritUpInfo_1.exp = 0;
                }
            }
            FigSpiritUpInfo[] figSpiritUpInfoArray = this.method_4(figSpiritUpInfo_0, figSpiritUpInfo_1, flag1);
            if (flag1)
            {
                isFall = false;
                client.Player.EquipBag.UpdatePlayerProperties();
                dir = 1;
            }
            string str = figSpiritUpInfoArray[0].level.ToString() + "," + (object)figSpiritUpInfoArray[0].exp + "," + (object)figSpiritUpInfoArray[0].place;
            for (int index = 1; index < figSpiritUpInfoArray.Length; ++index)
                str = str + "|" + (object)figSpiritUpInfoArray[index].level + "," + (object)figSpiritUpInfoArray[index].exp + "," + (object)figSpiritUpInfoArray[index].place;
            gemStone.FigSpiritId = num3;
            gemStone.FigSpiritIdValue = str;
            client.Player.UpdateGemStone(place, gemStone);
            client.Player.OnUserToemGemstoneEvent();
            client.Player.Out.SendPlayerFigSpiritUp(id, gemStone, flag1, isMaxLevel, isFall, num4, dir);
            return 0;
        }

        private int method_0(int int_2, int int_3)
        {
            return FigSpiritUpGradeHandler.int_0[int_3 + 1] - (int_2 + FigSpiritUpGradeHandler.int_0[int_3]);
        }

        private bool method_1(string[] string_0)
        {
            if (string_0[0].Split(',')[0] == FigSpiritUpGradeHandler.int_1.ToString())
            {
                if (string_0[1].Split(',')[0] == FigSpiritUpGradeHandler.int_1.ToString())
                    return string_0[2].Split(',')[0] == FigSpiritUpGradeHandler.int_1.ToString();
            }
            return false;
        }

        private bool method_2(int int_2, int int_3)
        {
            for (int index = 1; index < FigSpiritUpGradeHandler.int_0.Length; ++index)
            {
                if (int_2 >= FigSpiritUpGradeHandler.int_0[index] - FigSpiritUpGradeHandler.int_0[index - 1] && int_3 == index - 1)
                    return true;
            }
            return false;
        }

        private FigSpiritUpInfo method_3(FigSpiritUpInfo[] figSpiritUpInfo_0, int int_2)
        {
            foreach (FigSpiritUpInfo figSpiritUpInfo in figSpiritUpInfo_0)
            {
                if (figSpiritUpInfo.place == int_2)
                    return figSpiritUpInfo;
            }
            return (FigSpiritUpInfo)null;
        }

        private FigSpiritUpInfo[] method_4(
          FigSpiritUpInfo[] figSpiritUpInfo_0,
          FigSpiritUpInfo figSpiritUpInfo_1,
          bool bool_0)
        {
            for (int index = 0; index < figSpiritUpInfo_0.Length; ++index)
            {
                if (figSpiritUpInfo_0[index].place == figSpiritUpInfo_1.place)
                    figSpiritUpInfo_0[index] = figSpiritUpInfo_1;
            }
            if (!bool_0)
                return figSpiritUpInfo_0;
            IOrderedEnumerable<FigSpiritUpInfo> orderedEnumerable = ((IEnumerable<FigSpiritUpInfo>)figSpiritUpInfo_0).OrderBy<FigSpiritUpInfo, int>((Func<FigSpiritUpInfo, int>)(p => p.level)).ThenBy<FigSpiritUpInfo, int>((Func<FigSpiritUpInfo, int>)(p => p.exp));
            FigSpiritUpInfo[] figSpiritUpInfoArray = new FigSpiritUpInfo[figSpiritUpInfo_0.Length];
            int index1 = 0;
            foreach (FigSpiritUpInfo figSpiritUpInfo in (IEnumerable<FigSpiritUpInfo>)orderedEnumerable)
            {
                switch (index1)
                {
                    case 0:
                        figSpiritUpInfo.place = 2;
                        break;
                    case 1:
                        figSpiritUpInfo.place = 1;
                        break;
                    case 2:
                        figSpiritUpInfo.place = 0;
                        break;
                }
                figSpiritUpInfoArray[index1] = figSpiritUpInfo;
                ++index1;
            }
            return figSpiritUpInfoArray;
        }
    }
}
