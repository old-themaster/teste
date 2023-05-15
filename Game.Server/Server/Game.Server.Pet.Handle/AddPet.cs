// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.AddPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameUtils;
using Game.Server.Managers;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Pet.Handle
{
    [global::Pet(2)]
  public class AddPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int place = packet.ReadInt();
      int num = packet.ReadInt();
      int id = Player.PlayerCharacter.ID;
      PetInventory petBag = Player.PetBag;
      int firstEmptySlot = petBag.FindFirstEmptySlot();
      if (Player.PlayerCharacter.Grade < 25)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg2"));
        return false;
      }
      if (firstEmptySlot == -1)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg3"));
      }
      else
      {
        ItemInfo itemAt = Player.GetItemAt((eBageType) num, place);
        PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(itemAt.Template.Property5);
        if (petTemplate == null)
        {
          Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg4"));
          return false;
        }
        UsersPetInfo pet = PetMgr.CreatePet(petTemplate, id, firstEmptySlot, petBag.MaxLevelByGrade);
        pet.IsExit = true;
        pet.PetEquips = new List<PetEquipInfo>();
        pet.BaseProp = JsonConvert.SerializeObject((object) pet);
        petBag.AddPetTo(pet, firstEmptySlot);
        Player.RemoveCountFromStack(itemAt, 1);
        if (petTemplate.StarLevel > 4)
          GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysTipNotice(LanguageMgr.GetTranslation("PetHandler.Msg5", (object) Player.PlayerCharacter.NickName, (object) petTemplate.Name, (object) petTemplate.StarLevel)));
        else
          Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg6", (object) petTemplate.Name, (object) petTemplate.StarLevel));
        petBag.SaveToDatabase(false);
        GSPacketIn pkg = new GSPacketIn((short) 68);
        pkg.WriteByte((byte) 2);
        pkg.WriteInt(petTemplate.TemplateID);
        pkg.WriteBoolean(true);
        Player.SendTCP(pkg);
      }
      return false;
    }
  }
}
