// Decompiled with JetBrains decompiler
// Type: Tank.Request.FavoriteTransit
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
  public class FavoriteTransit : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetFavoriteUrl => ConfigurationManager.AppSettings["FavoriteUrl"];

    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      try
      {
        string str1 = context.Request["username"] == null ? "" : HttpUtility.UrlDecode(context.Request["username"]);
        string str2 = context.Request["site"] == null ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower();
        string format = string.Empty;
        if (!string.IsNullOrEmpty(str2))
        {
          format = ConfigurationManager.AppSettings[string.Format("FavoriteUrl_{0}", (object) str2)];
          int num = str1.IndexOf('_');
          if (num != -1)
            str1 = str1.Substring(num + 1, str1.Length - num - 1);
        }
        if (string.IsNullOrEmpty(format))
          format = FavoriteTransit.GetFavoriteUrl;
        context.Response.Redirect(string.Format(format, (object) str1, (object) str2), false);
      }
      catch (Exception ex)
      {
        FavoriteTransit.log.Error((object) "FavoriteTransit:", ex);
      }
    }

    public bool IsReusable => false;
  }
}
