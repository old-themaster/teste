// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.Get_UserSexResponseBody
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
  public class Get_UserSexResponseBody
  {
    [DataMember(Order = 0)]
    public bool? Get_UserSexResult;

    public Get_UserSexResponseBody()
    {
    }

    public Get_UserSexResponseBody(bool? Get_UserSexResult) => this.Get_UserSexResult = Get_UserSexResult;
  }
}
