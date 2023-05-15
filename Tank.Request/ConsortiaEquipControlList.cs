// Decompiled with JetBrains decompiler
// Type: Tank.Request.ConsortiaEquipControlList
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
  public class ConsortiaEquipControlList : IHttpHandler
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
        int page = 1;
        int size = 10;
        int order = 1;
        int consortiaID = int.Parse(context.Request["consortiaID"]);
        int level = int.Parse(context.Request["level"]);
        int type = int.Parse(context.Request["type"]);
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          foreach (ConsortiaEquipControlInfo info in consortiaBussiness.GetConsortiaEquipControlPage(page, size, ref total, order, consortiaID, level, type))
            node.Add((object) FlashUtils.CreateConsortiaEquipControlInfo(info));
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        ConsortiaEquipControlList.log.Error((object) "ConsortiaList", ex);
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
