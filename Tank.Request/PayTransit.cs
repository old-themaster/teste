// Decompiled with JetBrains decompiler
// Type: Tank.Request.PayTransit
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class PayTransit : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private string site = "";

    public string PayURL => ConfigurationManager.AppSettings["PayURL_" + this.site];

    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      string str = "";
      string format = string.Empty;
      try
      {
        if (!string.IsNullOrEmpty(context.Request["username"]))
          str = HttpUtility.UrlDecode(context.Request["username"].Trim());
        this.site = context.Request["site"] == null ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower();
        if (!string.IsNullOrEmpty(this.site))
        {
          format = this.PayURL;
          int num = str.IndexOf('_');
          if (num != -1)
            str = str.Substring(num + 1, str.Length - num - 1);
        }
        if (string.IsNullOrEmpty(format))
          format = ConfigurationManager.AppSettings["PayURL"];
        context.Response.Redirect(string.Format(format, (object) str, (object) this.site), false);
      }
      catch (Exception ex)
      {
        PayTransit.log.Error((object) "PayTransit:", ex);
      }
    }

    public bool IsReusable => false;
  }
}
