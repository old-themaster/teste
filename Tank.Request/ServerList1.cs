// Decompiled with JetBrains decompiler
// Type: Tank.Request.ServerList1
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.CenterService;
using log4net;
using Road.Flash;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.UI;
using System.Xml.Linq;

namespace Tank.Request
{
  public class ServerList1 : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static DateTime date = DateTime.Now;
    private static string xml = string.Empty;
    private static int OnlineTotal = 0;
    private static ServerData[] infos;

    public static string agentId => ConfigurationManager.AppSettings["ServerID"];

    protected void Page_Load(object sender, EventArgs e)
    {
      int num1 = this.Request["id"] == null ? -1 : int.Parse(this.Request["id"]);
      if (ServerList1.infos == null || ServerList1.date.AddMinutes(5.0).CompareTo(DateTime.Now) < 0)
      {
        bool flag = false;
        string str = "Fail!";
        int num2 = 0;
        XElement node = new XElement((XName) "Result");
        try
        {
          using (CenterServiceClient centerServiceClient = new CenterServiceClient())
          {
            ServerList1.infos = centerServiceClient.GetServerList();
            ServerList1.date = DateTime.Now;
          }
          foreach (ServerData info in ServerList1.infos)
          {
            if (info.State != -1)
            {
              num2 += info.Online;
              node.Add((object) FlashUtils.CreateServerInfo(info.Id, info.Name, info.Ip, info.Port, info.State, info.MustLevel, info.LowestLevel, info.Online));
            }
          }
          flag = true;
          str = "Success!";
        }
        catch (Exception ex)
        {
          ServerList1.log.Error((object) "ServerList1 error:", ex);
        }
        ServerList1.OnlineTotal = num2;
        node.Add((object) new XAttribute((XName) "value", (object) flag));
        node.Add((object) new XAttribute((XName) "message", (object) str));
        node.Add((object) new XAttribute((XName) "total", (object) num2));
        node.Add((object) new XAttribute((XName) "agentId", (object) ServerList1.agentId));
        node.Add((object) new XAttribute((XName) "AreaName", (object) ("an" + ServerList1.agentId)));
        node.Add((object) new XAttribute((XName) "Info", (object) ServerList1.agentId));
        ServerList1.xml = node.ToString(false);
      }
      string s = "0";
      if (num1 == 0)
        s = ServerList1.OnlineTotal.ToString();
      else if (num1 > 0)
      {
        foreach (ServerData info in ServerList1.infos)
        {
          if (info.Id == num1)
          {
            s = info.Online.ToString();
            break;
          }
        }
      }
      else
        s = ServerList1.xml;
      this.Response.Write(s);
    }
  }
}
