﻿// Decompiled with JetBrains decompiler
// Type: Tank.Request.NoticeServerUpdate
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
  public class NoticeServerUpdate : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

    public static bool ValidLoginIP(string ip)
    {
      string getAdminIp = NoticeServerUpdate.GetAdminIP;
      if (!string.IsNullOrEmpty(getAdminIp))
      {
        if (!((IEnumerable<string>) getAdminIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      int num = 2;
      try
      {
        int serverId = int.Parse(this.Context.Request["serverID"]);
        int type = int.Parse(this.Context.Request["type"]);
        if (NoticeServerUpdate.ValidLoginIP(this.Context.Request.UserHostAddress))
        {
          using (CenterServiceClient centerServiceClient = new CenterServiceClient())
            num = centerServiceClient.NoticeServerUpdate(serverId, type);
          if (type == 5)
          {
            if (num == 0)
              num = this.HandleServerMapUpdate();
          }
        }
        else
          num = 5;
      }
      catch (Exception ex)
      {
        NoticeServerUpdate.log.Error((object) "ExperienceRateUpdate:", ex);
        num = 4;
      }
      this.Response.Write((object) num);
    }

    private int HandleServerMapUpdate() => !BaseInterface.RequestContent("http://" + HttpContext.Current.Request.Url.Authority.ToString() + "/MapServerList.ashx").Contains("Success") ? 3 : 0;
  }
}
