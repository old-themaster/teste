// Decompiled with JetBrains decompiler
// Type: Tank.Request.ExperienceRate
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
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
  public class ExperienceRate : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected HtmlForm form1;

    public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

    public static bool ValidLoginIP(string ip)
    {
      string getAdminIp = ExperienceRate.GetAdminIP;
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
        int serverId = int.Parse(this.Context.Request["serverId"]);
        if (ExperienceRate.ValidLoginIP(this.Context.Request.UserHostAddress))
        {
          using (CenterServiceClient centerServiceClient = new CenterServiceClient())
            num = centerServiceClient.ExperienceRateUpdate(serverId);
        }
      }
      catch (Exception ex)
      {
        ExperienceRate.log.Error((object) "ExperienceRateUpdate:", ex);
      }
      this.Response.Write((object) num);
    }
  }
}
