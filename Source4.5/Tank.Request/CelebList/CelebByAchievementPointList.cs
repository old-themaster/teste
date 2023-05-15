﻿// Decompiled with JetBrains decompiler
// Type: Tank.Request.CelebList.CelebByAchievementPointList
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
  public class CelebByAchievementPointList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public bool IsReusable => false;

    public void ProcessRequest(HttpContext context) => context.Response.Write(CelebByAchievementPointList.Build(context));

    public static string Build(HttpContext context) => !csFunction.ValidAdminIP(context.Request.UserHostAddress) ? "CelebByAchievementPointList Fail!" : CelebByAchievementPointList.Build();

    public static string Build() => csFunction.BuildCelebUsers(nameof (CelebByAchievementPointList), 11, "CelebByAchievementPointList_Out");
  }
}