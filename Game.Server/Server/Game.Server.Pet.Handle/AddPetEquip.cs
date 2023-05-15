// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.AddPetEquip
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(20)]
  public class AddPetEquip : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      int slot = packet.ReadInt();
      int place = packet.ReadInt();
      PlayerInventory inventory = Player.GetInventory((eBageType) num);
      ItemInfo itemAt = inventory.GetItemAt(slot);
      if (itemAt != null && itemAt.IsEquipPet() && itemAt.IsValidItem())
      {
        if (Player.PetBag.AddEqPet(place, itemAt))
        {
          inventory.TakeOutItem(itemAt);
          Player.PetBag.OnChangedPetEquip(place);
          Player.SendMessage(LanguageMgr.GetTranslation("AddPetEquip.Success"));
        }
        else
          Player.SendMessage(LanguageMgr.GetTranslation("AddPetEquip.Fail"));
      }
      else
        Player.SendMessage(LanguageMgr.GetTranslation("AddPetEquip.WrongItem"));
      return false;
    }
  }
}
