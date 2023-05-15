// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.Get_UserSexResponse
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [MessageContract(IsWrapped = false)]
  public class Get_UserSexResponse
  {
    [MessageBodyMember(Name = "Get_UserSexResponse", Namespace = "dandantang", Order = 0)]
    public Get_UserSexResponseBody Body;

    public Get_UserSexResponse()
    {
    }

    public Get_UserSexResponse(Get_UserSexResponseBody Body) => this.Body = Body;
  }
}
