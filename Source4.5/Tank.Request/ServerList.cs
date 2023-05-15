// Decompiled with JetBrains decompiler
// Type: Tank.Request.ServerList
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;
using Road.Flash;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class ServerList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private static string agentId => ConfigurationManager.AppSettings["ServerID"];

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      int num = 0;
      XElement node = new XElement((XName) "Result");
      try
      {
        if (BaseInterface.CheckRnd(context.Request["rnd"]))
        {
          using (CenterServiceClient centerServiceClient = new CenterServiceClient())
          {
            foreach (ServerData server in (IEnumerable<ServerData>) centerServiceClient.GetServerList())
            {
              if (server.State != -1)
              {
                num += server.Online;
                node.Add((object) FlashUtils.CreateServerInfo(server.Id, server.Name, server.Ip, server.Port, server.State, server.MustLevel, server.LowestLevel, server.Online));
              }
            }
          }
        }
        flag = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
        ServerList.log.Error((object) "Load server list error:", ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      node.Add((object) new XAttribute((XName) "total", (object) num));
      node.Add((object) new XAttribute((XName) "agentId", (object) ServerList.agentId));
      node.Add((object) new XAttribute((XName) "AreaName", (object) ("a" + ServerList.agentId)));
      node.Add((object) new XAttribute((XName) "Info", (object) ServerList.agentId));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
