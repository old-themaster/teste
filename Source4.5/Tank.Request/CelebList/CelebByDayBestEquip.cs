// Decompiled with JetBrains decompiler
// Type: Tank.Request.CelebList.CelebByDayBestEquip
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

namespace Tank.Request.CelebList
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class CelebByDayBestEquip : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context) => context.Response.Write(CelebByDayBestEquip.Build(context));

    public static string Build(HttpContext context) => !csFunction.ValidAdminIP(context.Request.UserHostAddress) ? "CelebByDayGPList Fail!" : CelebByDayBestEquip.Build();

    public static string Build()
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (BestEquipInfo info in playerBussiness.GetCelebByDayBestEquip())
            result.Add((object) FlashUtils.CreateBestEquipInfo(info));
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        CelebByDayBestEquip.log.Error((object) "Load CelebByDayBestEquip is fail!", ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(result, "CelebForBestEquip", false);
    }

    public bool IsReusable => false;
  }
}
