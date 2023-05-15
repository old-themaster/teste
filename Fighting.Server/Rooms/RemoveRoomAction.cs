// Decompiled with JetBrains decompiler
// Type: Fighting.Server.Rooms.RemoveRoomAction
// Assembly: Fighting.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DD84ADA-AC24-47D6-83C2-9107959F6D72
// Assembly location: C:\Users\Administrador.OMINIHOST\Downloads\DDBTank4.1\Emulador\Fight\Fighting.Server.dll

namespace Fighting.Server.Rooms
{
    public class RemoveRoomAction : IAction
    {
        private ProxyRoom m_room;

        public RemoveRoomAction(ProxyRoom room) => this.m_room = room;

        public void Execute()
        {
            ProxyRoomMgr.RemoveRoomUnsafe(this.m_room.RoomId);
            this.m_room.Dispose();
        }
    }
}
