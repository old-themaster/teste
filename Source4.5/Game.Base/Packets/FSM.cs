// Decompiled with JetBrains decompiler
// Type: Road.Base.Packets.FSM
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 46985F50-5376-415F-B11F-B261F2D4116F
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Base.dll

namespace Road.Base.Packets
{
  public class FSM
  {
    private int _adder;
    private int _mulitper;
    private int _state;
    public int count;
    public string name;

    public int getState() => this._state;

    public int getAdder() => this._adder;

    public int getMulitper() => this._mulitper;

    public FSM(int adder, int mulitper, string objname)
    {
      this.name = objname;
      this.count = 0;
      this._adder = adder;
      this._mulitper = mulitper;
      this.UpdateState();
    }

    public void Setup(int adder, int mulitper)
    {
      this._adder = adder;
      this._mulitper = mulitper;
      this.UpdateState();
    }

    public int UpdateState()
    {
      this._state = (~this._state + this._adder) * this._mulitper;
      this._state ^= this._state >> 16;
      ++this.count;
      return this._state;
    }
  }
}
