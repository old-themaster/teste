// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.DelPetEquip
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Pet.Handle
{
    [global::Pet(21)]
  public class DelPetEquip : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      int eqPlace = packet.ReadInt();
      if (Player.PetBag.RemoveEqPet(num, eqPlace))
        Player.PetBag.OnChangedPetEquip(num);
      return false;
    }
  }
}
