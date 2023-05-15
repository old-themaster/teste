// Decompiled with JetBrains decompiler
// Type: Tank.Request.SystemNotice
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
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
  public class SystemNotice : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetChargeIP => ConfigurationManager.AppSettings["AdminIP"];

    public static bool ValidLoginIP(string ip)
    {
      string getChargeIp = SystemNotice.GetChargeIP;
      if (!string.IsNullOrEmpty(getChargeIp))
      {
        if (!((IEnumerable<string>) getChargeIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      int num = 1;
      try
      {
        if (SystemNotice.ValidLoginIP(this.Context.Request.UserHostAddress))
        {
          string msg = HttpUtility.UrlDecode(this.Request["content"]);
          if (!string.IsNullOrEmpty(msg))
          {
            using (CenterServiceClient centerServiceClient = new CenterServiceClient())
            {
              if (centerServiceClient.SystemNotice(msg))
                num = 0;
            }
          }
        }
        else
          num = 2;
      }
      catch (Exception ex)
      {
        SystemNotice.log.Error((object) "SystemNotice:", ex);
      }
      this.Response.Write((object) num);
    }
  }
}
