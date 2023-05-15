// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.FightPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(17)]
  public class FightPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      bool isEquip = packet.ReadBoolean();
      UsersPetInfo petAt = Player.PetBag.GetPetAt(num);
      if (petAt == null)
        return false;
      if (petAt.Level > Player.PetBag.MaxLevelByGrade && !petAt.IsEquip)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg21"));
        return false;
      }
      if (Player.PetBag.EquipPet(num, isEquip))
        Player.EquipBag.UpdatePlayerProperties();
      else
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg22"));
      return false;
    }
  }
}
