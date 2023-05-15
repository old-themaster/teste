﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.PassPortSoapClient
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Game.Server.WebLogin
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class PassPortSoapClient : ClientBase<PassPortSoap>, PassPortSoap
  {
    public PassPortSoapClient()
    {
    }

    public PassPortSoapClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public PassPortSoapClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public PassPortSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public PassPortSoapClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ChenckValidateResponse PassPortSoap.ChenckValidate(
      ChenckValidateRequest request) => this.Channel.ChenckValidate(request);

    public string ChenckValidate(string applicationname, string username, string password)
    {
      ChenckValidateRequest request = new ChenckValidateRequest()
      {
        Body = new ChenckValidateRequestBody()
      };
      request.Body.applicationname = applicationname;
      request.Body.username = username;
      request.Body.password = password;
      return ((PassPortSoap) this).ChenckValidate(request).Body.ChenckValidateResult;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Get_UserSexResponse PassPortSoap.Get_UserSex(
      Get_UserSexRequest request) => this.Channel.Get_UserSex(request);

    public bool? Get_UserSex(string applicationname, string username)
    {
      Get_UserSexRequest request = new Get_UserSexRequest()
      {
        Body = new Get_UserSexRequestBody()
      };
      request.Body.applicationname = applicationname;
      request.Body.username = username;
      return ((PassPortSoap) this).Get_UserSex(request).Body.Get_UserSexResult;
    }
  }
}
