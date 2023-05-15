

namespace Game.Server.Packets
{
    public enum BoguAdventureType
    {
        ACTIVITY_OPEN = 89, // 0x00000059
        ENTER_BOGUADVENTURE = 90, // 0x0000005A
        UPDATE_CELL = 91, // 0x0000005B
        REVIVE_GAME = 92, // 0x0000005C
        ACQUIRE_AWARD = 93, // 0x0000005D
        OUT_BOGUADVENTURE = 94, // 0x0000005E
        FREE_RESET = 99, // 0x00000063
    }
}
