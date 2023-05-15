// Decompiled with JetBrains decompiler
// Type: Tank.Request.MailSenderList
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class MailSenderList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        int userID = int.Parse(context.Request.QueryString["selfID"]);
        if (userID != 0)
        {
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            foreach (MailInfo info in playerBussiness.GetMailBySenderID(userID))
              node.Add((object) FlashUtils.CreateMailInfo(info, "Item"));
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        MailSenderList.log.Error((object) nameof (MailSenderList), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.BinaryWrite(StaticFunction.Compress(node.ToString(false)));
    }

    public bool IsReusable => false;
  }
}
