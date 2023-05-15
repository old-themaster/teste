using Game.Logic;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Rooms
{
    public class FakeRoom : BaseRoom
    {
        private static Random random = new Random(Environment.TickCount);

        private bool isPlaying = false;

        private PveInfo pveinfo;

        
        public FakeRoom(int roomId, string roomName, int playerCount, int maxPlayerCount, int roomType) : base(roomId)
        {
            this.Name = roomName;
            switch (roomType)
            {
                case 1:
                    this.RoomType = eRoomType.Match;
                    break;
                case 2:
                    this.RoomType = eRoomType.Match;
                    this.isPlaying = true;
                    break;
                    //case 1:
                    //    this.RoomType = eRoomType.Match;
                    //    this.Password = Guid.NewGuid().ToString();
                    //    break;
                    //case 2:
                    //    this.RoomType = eRoomType.Match;
                    //    this.isPlaying = true;
                    //    break;
                    //case 3:
                    //    this.RoomType = eRoomType.Boss;
                    //    this.Password = Guid.NewGuid().ToString();
                    //    this.pveinfo = PveInfoMgr.GetRandomPve();
                    //    this.MapId = pveinfo.ID;
                    //    this.HardLevel = this.GetRandomHardLevel();
                    //    break;
                    //case 4:
                    //    this.RoomType = eRoomType.Dungeon;
                    //    this.pveinfo = PveInfoMgr.GetRandomPve();
                    //    this.MapId = pveinfo.ID;
                    //    this.HardLevel = this.GetRandomHardLevel();
                    //    this.isPlaying = true;
                    //    break;

            }

        }

        private eHardLevel GetRandomHardLevel()
        {
            while (true)
            {
                var result = random.Next(0, 3);
               // if (result == 0 && !this.pveinfo.SimpleGameScript.IsNullOrEmpty())
                {
                    return eHardLevel.Simple;
                }
               // else if (result == 1 && !this.pveinfo.NormalGameScript.IsNullOrEmpty())
                {
                    return eHardLevel.Normal;
                }
              //  else if (result == 2 && !this.pveinfo.HardGameScript.IsNullOrEmpty())
                {
                    return eHardLevel.Hard;
                }
             //   else if (result == 3 && !this.pveinfo.TerrorGameScript.IsNullOrEmpty())
                {
                    return eHardLevel.Terror;
                }
            }
        }      
    }
}
