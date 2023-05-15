// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.Get_UserSexResponseBody
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.WebLogin
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
