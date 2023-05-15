// Decompiled with JetBrains decompiler
// Type: Tank.Request.IMFriendsGood
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using System;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class IMFriendsGood : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        string UserName = context.Request["UserName"];
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          ArrayList friendsGood = playerBussiness.GetFriendsGood(UserName);
          for (int index = 0; index < friendsGood.Count; ++index)
          {
            XElement xelement = new XElement((XName) "Item", (object) new XAttribute((XName) "UserName", (object) friendsGood[index].ToString()));
            node.Add((object) xelement);
          }
        }
        flag = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
        IMFriendsGood.log.Error((object) nameof (IMFriendsGood), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
