// Decompiled with JetBrains decompiler
// Type: Center.Server.Player
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\WONDERTANK vReZero\Emulator\Center\Center.Server.dll

namespace Center.Server
{
  public class Player
  {
    public ServerClient CurrentServer;
    public int Id;
    public bool IsFirst;
    public long LastTime;
    public string Name;
    public string NickName;
    public string Password;
    public ePlayerState State;
  }
}
