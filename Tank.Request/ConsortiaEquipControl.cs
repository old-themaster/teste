// Decompiled with JetBrains decompiler
// Type: Tank.Request.ConsortiaEquipControl
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
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
  public class ConsortiaEquipControl : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      int num = 0;
      try
      {
        int consortiaID = int.Parse(context.Request["consortiaID"]);
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          for (int type = 1; type < 3; ++type)
          {
            for (int Level = 1; Level < 11; ++Level)
            {
              ConsortiaEquipControlInfo consortiaEuqipRiches = consortiaBussiness.GetConsortiaEuqipRiches(consortiaID, Level, type);
              if (consortiaEuqipRiches != null)
              {
                node.Add((object) new XElement((XName) "Item", new object[3]
                {
                  (object) new XAttribute((XName) "type", (object) consortiaEuqipRiches.Type),
                  (object) new XAttribute((XName) "level", (object) consortiaEuqipRiches.Level),
                  (object) new XAttribute((XName) "riches", (object) consortiaEuqipRiches.Riches)
                }));
                ++num;
              }
            }
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        ConsortiaEquipControl.log.Error((object) "ConsortiaEventList", ex);
      }
      node.Add((object) new XAttribute((XName) "total", (object) num));
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
