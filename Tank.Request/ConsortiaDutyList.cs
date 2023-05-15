// Decompiled with JetBrains decompiler
// Type: Tank.Request.ConsortiaDutyList
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
  public class ConsortiaDutyList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      int total = 0;
      try
      {
        int page = int.Parse(context.Request["page"]);
        int size = int.Parse(context.Request["size"]);
        int order = int.Parse(context.Request["order"]);
        int consortiaID = int.Parse(context.Request["consortiaID"]);
        int dutyID = int.Parse(context.Request["dutyID"]);
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          foreach (ConsortiaDutyInfo info in consortiaBussiness.GetConsortiaDutyPage(page, size, ref total, order, consortiaID, dutyID))
            node.Add((object) FlashUtils.CreateConsortiaDutyInfo(info));
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        ConsortiaDutyList.log.Error((object) nameof (ConsortiaDutyList), ex);
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
