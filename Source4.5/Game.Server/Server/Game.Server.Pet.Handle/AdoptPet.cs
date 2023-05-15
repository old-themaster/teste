// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.AdoptPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Pet.Handle
{
    [global::Pet(6)]
  public class AdoptPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      if (Player.PetBag.FindFirstEmptySlot() == -1)
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg15"));
      else if (num < 0)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg16"));
        return false;
      }
      return false;
    }
  }
}
