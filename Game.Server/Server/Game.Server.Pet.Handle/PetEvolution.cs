// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.PetEvolution
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(23)]
  public class PetEvolution : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      int count = packet.ReadInt();
      if (num1 != 11163)
        return false;
      int num2 = 0;
      ItemInfo itemByTemplateId = Player.GetItemByTemplateID(num1);
      if (itemByTemplateId != null && count > 0 && num1 == 11163)
      {
        if (itemByTemplateId.Count < count)
          count = itemByTemplateId.Count;
        num2 = itemByTemplateId.Template.Property2 * count;
      }
      if (num2 > 0)
      {
        bool val = false;
        int evolutionGrade = Player.PlayerCharacter.evolutionGrade;
        int evolutionExp = Player.PlayerCharacter.evolutionExp;
        int num3 = Player.PlayerCharacter.evolutionExp + num2;
        int evolutionMax = PetMgr.GetEvolutionMax();
        for (int index = evolutionGrade; index <= evolutionMax; ++index)
        {
          PetFightPropertyInfo fightProperty = PetMgr.FindFightProperty(index + 1);
          if (fightProperty != null && fightProperty.Exp <= num3)
          {
            Player.PlayerCharacter.evolutionGrade = index + 1;
            val = true;
          }
        }
        Player.PlayerCharacter.evolutionExp = num3;
        Player.PropBag.RemoveTemplate(num1, count);
        Player.EquipBag.UpdatePlayerProperties();
        Player.SendUpdatePublicPlayer();
        GSPacketIn pkg = new GSPacketIn((short) 68);
        pkg.WriteByte((byte) 23);
        pkg.WriteBoolean(val);
        Player.SendTCP(pkg);
      }
      return false;
    }
  }
}
