// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.GameCommandMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  public class GameCommandMgr
  {
    private Dictionary<int, IGameCommandHandler> handles = new Dictionary<int, IGameCommandHandler>();

    public GameCommandMgr()
    {
      this.handles.Clear();
      this.SearchCommandHandlers(Assembly.GetAssembly(typeof (GameServer)));
    }

    public IGameCommandHandler LoadCommandHandler(int code) => this.handles[code];

    protected void RegisterCommandHandler(int code, IGameCommandHandler handle) => this.handles.Add(code, handle);

    protected int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && type.GetInterface("Game.Server.RingStation.RoomGamePkg.TankHandle.IGameCommandHandler") != (Type) null)
        {
          GameCommandAttbute[] customAttributes = (GameCommandAttbute[]) type.GetCustomAttributes(typeof (GameCommandAttbute), true);
          if (customAttributes.Length != 0)
          {
            ++num;
            this.RegisterCommandHandler((int) customAttributes[0].Code, Activator.CreateInstance(type) as IGameCommandHandler);
          }
        }
      }
      return num;
    }
  }
}
