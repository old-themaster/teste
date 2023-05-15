// Decompiled with JetBrains decompiler
// Type: Tank.Request.AASUpdateState
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness.CenterService;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace Tank.Request
{
  public class AASUpdateState : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

    public static bool ValidLoginIP(string ip)
    {
      string getAdminIp = AASUpdateState.GetAdminIP;
      if (!string.IsNullOrEmpty(getAdminIp))
      {
        if (!((IEnumerable<string>) getAdminIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      int num = 2;
      try
      {
        bool state = bool.Parse(this.Request["state"]);
        if (AASUpdateState.ValidLoginIP(this.Context.Request.UserHostAddress))
        {
          using (CenterServiceClient centerServiceClient = new CenterServiceClient())
            num = !centerServiceClient.AASUpdateState(state) ? 1 : 0;
        }
      }
      catch (Exception ex)
      {
        AASUpdateState.log.Error((object) "ASSUpdateState:", ex);
      }
      this.Response.Write((object) num);
    }
  }
}
