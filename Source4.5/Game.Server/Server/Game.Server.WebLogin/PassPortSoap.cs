// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.PassPortSoap
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Game.Server.WebLogin
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
