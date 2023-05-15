// Decompiled with JetBrains decompiler
// Type: Tank.Request.VisualizeItemLoad
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class VisualizeItemLoad : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag1 = false;
      string str = "Fail!";
      bool flag2 = bool.Parse(context.Request["sex"]);
      XElement node = new XElement((XName) "Result");
      try
      {
        string appSetting = ConfigurationManager.AppSettings[flag2 ? "BoyVisualizeItem" : "GrilVisualizeItem"];
        node.Add((object) new XAttribute((XName) "content", (object) appSetting));
        flag1 = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
        VisualizeItemLoad.log.Error((object) nameof (VisualizeItemLoad), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag1));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
