﻿// Decompiled with JetBrains decompiler
// Type: Tank.Request.LogTime
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
  public class LogTime : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      XElement xelement = new XElement((XName) "Result");
      int total = 0;
      try
      {
        int page = int.Parse(context.Request["page"]);
        int size = int.Parse(context.Request["size"]);
        int order = int.Parse(context.Request["order"]);
        int consortiaID = int.Parse(context.Request["consortiaID"]);
        int state = int.Parse(context.Request["state"]);
        string name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"] == null ? "" : context.Request["name"]));
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          foreach (ConsortiaAllyInfo info in consortiaBussiness.GetConsortiaAllyPage(page, size, ref total, order, consortiaID, state, name))
            xelement.Add((object) FlashUtils.CreateConsortiaAllyInfo(info));
        }
      }
      catch (Exception ex)
      {
      }
    }

    public bool IsReusable => false;
  }
}
