// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.EquipSkillPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Pet.Handle
{
    [global::Pet(7)]
  public class EquipSkillPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int place = packet.ReadInt();
      int killId = packet.ReadInt();
      int killindex = packet.ReadInt();
      if (!Player.PetBag.EquipSkillPet(place, killId, killindex))
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg18"));
      return false;
    }
  }
}
