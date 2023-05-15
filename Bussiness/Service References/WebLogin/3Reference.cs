// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.ChenckValidateResponseBody
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
