// Decompiled with JetBrains decompiler
// Type: Tank.Request.KitoffUser
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace Tank.Request
{
  public class KitoffUser : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

    public static bool ValidLoginIP(string ip)
    {
      string getAdminIp = KitoffUser.GetAdminIP;
      if (!string.IsNullOrEmpty(getAdminIp))
      {
        if (!((IEnumerable<string>) getAdminIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      bool flag = false;
      try
      {
        KitoffUser.ValidLoginIP(this.Context.Request.UserHostAddress);
      }
      catch (Exception ex)
      {
        KitoffUser.log.Error((object) "GetAdminIP:", ex);
      }
      this.Response.Write((object) flag);
    }
  }
}
