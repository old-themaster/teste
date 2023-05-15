// Decompiled with JetBrains decompiler
// Type: Bussiness.CenterService.CenterServiceClient
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Bussiness.CenterService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class CenterServiceClient : ClientBase<ICenterService>, ICenterService
  {
    public CenterServiceClient()
    {
    }

    public CenterServiceClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public CenterServiceClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public CenterServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public CenterServiceClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public ServerData[] GetServerList() => this.Channel.GetServerList();

    public bool ChargeMoney(int userID, string chargeID) => this.Channel.ChargeMoney(userID, chargeID);

    public bool SystemNotice(string msg) => this.Channel.SystemNotice(msg);

    public bool KitoffUser(int playerID, string msg) => this.Channel.KitoffUser(playerID, msg);

    public bool ReLoadServerList() => this.Channel.ReLoadServerList();

    public bool MailNotice(int playerID) => this.Channel.MailNotice(playerID);

    public bool ActivePlayer(bool isActive) => this.Channel.ActivePlayer(isActive);

    public bool CreatePlayer(int id, string name, string password, bool isFirst) => this.Channel.CreatePlayer(id, name, password, isFirst);

    public bool ValidateLoginAndGetID(
      string name,
      string password,
      ref int userID,
      ref bool isFirst) => this.Channel.ValidateLoginAndGetID(name, password, ref userID, ref isFirst);

    public bool AASUpdateState(bool state) => this.Channel.AASUpdateState(state);

    public int AASGetState() => this.Channel.AASGetState();

    public int ExperienceRateUpdate(int serverId) => this.Channel.ExperienceRateUpdate(serverId);

    public int NoticeServerUpdate(int serverId, int type) => this.Channel.NoticeServerUpdate(serverId, type);

    public bool UpdateConfigState(int type, bool state) => this.Channel.UpdateConfigState(type, state);

    public int GetConfigState(int type) => this.Channel.GetConfigState(type);

    public bool Reload(string type) => this.Channel.Reload(type);
  }
}
