// Decompiled with JetBrains decompiler
// Type: Tank.Request.AccountRegister
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class AccountRegister : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      XElement node = new XElement((XName) "Result");
      bool flag = false;
      try
      {
        string UserName = HttpUtility.UrlDecode(context.Request["username"]);
        string NickName = HttpUtility.UrlDecode(context.Request["password"]);
        string Password = HttpUtility.UrlDecode(context.Request["password"]);
        bool Sex = false;
        int Money = 100;
        int GiftToken = 100;
        int Gold = 100;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
          flag = playerBussiness.RegisterUser(UserName, NickName, Password, Sex, Money, GiftToken, Gold);
      }
      catch (Exception ex)
      {
        AccountRegister.log.Error((object) "RegisterResult", ex);
      }
      finally
      {
        node.Add((object) new XAttribute((XName) "value", (object) "vl"));
        node.Add((object) new XAttribute((XName) "message", (object) flag));
        context.Response.ContentType = "text/plain";
        context.Response.Write(node.ToString(false));
      }
    }

    public bool IsReusable => false;
  }
}
