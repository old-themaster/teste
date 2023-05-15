// Decompiled with JetBrains decompiler
// Type: Tank.Request.Global
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using log4net.Config;
using System;
using System.Reflection;
using System.Web;

namespace Tank.Request
{
  public class Global : HttpApplication
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    protected void Application_Start(object sender, EventArgs e)
    {
      LanguageMgr.Setup(this.Server.MapPath("~"));
      XmlConfigurator.Configure();
      StaticsMgr.Setup();
      PlayerManager.Setup();
    }

    protected void Session_Start(object sender, EventArgs e)
    {
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
    }

    protected void Application_Error(object sender, EventArgs e)
    {
    }

    protected void Session_End(object sender, EventArgs e)
    {
    }

    protected void Application_End(object sender, EventArgs e) => StaticsMgr.Stop();
  }
}
