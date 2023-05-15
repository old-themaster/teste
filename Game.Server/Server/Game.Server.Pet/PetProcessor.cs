// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.PetProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Pet
{
    public class PetProcessor
  {
    private static object object_0 = new object();
    private IPetProcessor ipetProcessor_0;

    public PetProcessor(IPetProcessor processor) => this.ipetProcessor_0 = processor;

    public void ProcessData(GamePlayer player, GSPacketIn data)
    {
      lock (PetProcessor.object_0)
        this.ipetProcessor_0.OnGameData(player, data);
    }
  }
}
