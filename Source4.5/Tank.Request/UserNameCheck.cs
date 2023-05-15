// Decompiled with JetBrains decompiler
// Type: Tank.Request.UserNameCheck
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.Interface;
using log4net;
using System;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
  public class UserNameCheck : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
      int num = 1;
      try
      {
        string user = HttpUtility.UrlDecode(this.Request["username"]);
        string site = this.Request["site"] == null ? "" : HttpUtility.UrlDecode(this.Request["site"]);
        if (!string.IsNullOrEmpty(user))
        {
          string nameBySite = BaseInterface.GetNameBySite(user, site);
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
            num = playerBussiness.GetUserSingleByUserName(nameBySite) == null ? 2 : 0;
        }
      }
      catch (Exception ex)
      {
        UserNameCheck.log.Error((object) "UserNameCheck:", ex);
      }
      this.Response.Write((object) num);
    }
  }
}
