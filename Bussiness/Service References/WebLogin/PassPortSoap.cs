// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.PassPortSoap
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "WebLogin.PassPortSoap", Namespace = "dandantang")]
  public interface PassPortSoap
  {
    [OperationContract(Action = "dandantang/ChenckValidate", ReplyAction = "*")]
    ChenckValidateResponse ChenckValidate(ChenckValidateRequest request);

    [OperationContract(Action = "dandantang/Get_UserSex", ReplyAction = "*")]
    Get_UserSexResponse Get_UserSex(Get_UserSexRequest request);
  }
}
