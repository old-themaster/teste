

namespace Game.Server.Packets
{
    public enum LittleGamePackageOut
    {
        WORLD_LIST = 1,
        START_LOAD = 2,
        GAME_START = 3,
        SETCLOCK = 5,
        PONG = 6,
        NET_DELAY = 7,
        ADD_SPRITE = 16, // 0x00000010
        REMOVE_SPRITE = 17, // 0x00000011
        KICK_PLAYE = 18, // 0x00000012
        MOVE = 32, // 0x00000020
        UPDATE_POS = 33, // 0x00000021
        GETSCORE = 49, // 0x00000031
        ADD_OBJECT = 64, // 0x00000040
        REMOVE_OBJECT = 65, // 0x00000041
        INVOKE_OBJECT = 66, // 0x00000042
        UPDATELIVINGSPROPERTY = 80, // 0x00000050
        DoMovie = 81, // 0x00000051
        DoAction = 96, // 0x00000060
    }
}
