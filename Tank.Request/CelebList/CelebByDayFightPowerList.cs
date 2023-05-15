﻿// Decompiled with JetBrains decompiler
// Type: Tank.Request.CelebList.CelebByDayFightPowerList
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Tank.Request.CelebList
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class CelebByDayFightPowerList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context) => context.Response.Write(CelebByDayFightPowerList.Build(context));

    public static string Build(HttpContext context) => !csFunction.ValidAdminIP(context.Request.UserHostAddress) ? "CelebByDayFightPowerList Fail!" : CelebByDayFightPowerList.Build();

    public static string Build() => csFunction.BuildCelebUsers(nameof (CelebByDayFightPowerList), 6, "CelebByDayFightPowerList_Out");

    public bool IsReusable => false;
  }
}
