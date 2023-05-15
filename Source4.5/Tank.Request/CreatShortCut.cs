// Decompiled with JetBrains decompiler
// Type: Tank.Request.CreatShortCut
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System.Reflection;
using System.Web;

namespace Tank.Request
{
  public class CreatShortCut : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      string str = context.Request["gameurl"];
      context.Response.Write("Not support right now");
    }

    public bool IsReusable => false;
  }
}
