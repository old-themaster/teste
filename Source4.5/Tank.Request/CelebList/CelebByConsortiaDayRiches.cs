// Decompiled with JetBrains decompiler
// Type: Tank.Request.CelebList.CelebByConsortiaDayRiches
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
  public class CelebByConsortiaDayRiches : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context) => context.Response.Write(CelebByConsortiaDayRiches.Build(context));

    public static string Build(HttpContext context) => !csFunction.ValidAdminIP(context.Request.UserHostAddress) ? "CelebByConsortiaDayRiches Fail!" : CelebByConsortiaDayRiches.Build();

    public static string Build() => csFunction.BuildCelebConsortia(nameof (CelebByConsortiaDayRiches), 11, "CelebByConsortiaDayRiches_Out");

    public bool IsReusable => false;
  }
}
