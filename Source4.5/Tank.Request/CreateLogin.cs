// Decompiled with JetBrains decompiler
// Type: Tank.Request.CreateLogin
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness.Interface;
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
  public class CreateLogin : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetLoginIP => ConfigurationManager.AppSettings["LoginIP"];

    public static bool ValidLoginIP(string ip)
    {
      string getLoginIp = CreateLogin.GetLoginIP;
      if (!string.IsNullOrEmpty(getLoginIp))
      {
        if (!((IEnumerable<string>) getLoginIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      int result = 1;
      try
      {
        string content = HttpUtility.UrlDecode(this.Request["content"]);
        string site = this.Request["site"] == null ? "" : HttpUtility.UrlDecode(this.Request["site"]).ToLower();
        string[] strArray = BaseInterface.CreateInterface().UnEncryptLogin(content, ref result, site);
        if (strArray.Length > 3)
        {
          string lower1 = strArray[0].Trim().ToLower();
          string lower2 = strArray[1].Trim().ToLower();
          if (!string.IsNullOrEmpty(lower1) && !string.IsNullOrEmpty(lower2))
          {
            PlayerManager.Add(BaseInterface.GetNameBySite(lower1, site), lower2);
            result = 0;
          }
          else
            result = -91010;
        }
        else
          result = -1900;
      }
      catch (Exception ex)
      {
        CreateLogin.log.Error((object) "CreateLogin:", ex);
      }
      this.Response.Write((object) result);
    }
  }
}
