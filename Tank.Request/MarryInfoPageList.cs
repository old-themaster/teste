// Decompiled with JetBrains decompiler
// Type: Tank.Request.MarryInfoPageList
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
  public class MarryInfoPageList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      int total = 0;
      XElement node = new XElement((XName) "Result");
      try
      {
        int page = int.Parse(context.Request["page"]);
        string name = (string) null;
        if (context.Request["name"] != null)
          name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));
        bool sex = bool.Parse(context.Request["sex"]);
        int size = 12;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (MarryInfo info in playerBussiness.GetMarryInfoPage(page, name, sex, size, ref total))
          {
            XElement marryInfo = FlashUtils.CreateMarryInfo(info);
            node.Add((object) marryInfo);
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        MarryInfoPageList.log.Error((object) nameof (MarryInfoPageList), ex);
      }
      node.Add((object) new XAttribute((XName) "total", (object) total));
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
