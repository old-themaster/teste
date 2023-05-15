// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.ChenckValidateResponseBody
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Game.Server.WebLogin
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [DataContract(Namespace = "dandantang")]
  public class ChenckValidateResponseBody
  {
    [DataMember(EmitDefaultValue = false, Order = 0)]
    public string ChenckValidateResult;

    public ChenckValidateResponseBody()
    {
    }

    public ChenckValidateResponseBody(string ChenckValidateResult) => this.ChenckValidateResult = ChenckValidateResult;
  }
}
