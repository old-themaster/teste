// Decompiled with JetBrains decompiler
// Type: Tank.Request.CelebList.CreateAllCeleb
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request.CelebList
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class CreateAllCeleb : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(CelebByGpList.Build());
        stringBuilder.Append(CelebByDayGPList.Build());
        stringBuilder.Append(CelebByWeekGPList.Build());
        stringBuilder.Append(CelebByOfferList.Build());
        stringBuilder.Append(CelebByDayOfferList.Build());
        stringBuilder.Append(CelebByWeekOfferList.Build());
        stringBuilder.Append(CelebByDayFightPowerList.Build());
        stringBuilder.Append(CelebByConsortiaRiches.Build());
        stringBuilder.Append(CelebByConsortiaDayRiches.Build());
        stringBuilder.Append(CelebByConsortiaWeekRiches.Build());
        stringBuilder.Append(CelebByConsortiaHonor.Build());
        stringBuilder.Append(CelebByConsortiaDayHonor.Build());
        stringBuilder.Append(CelebByConsortiaWeekHonor.Build());
        stringBuilder.Append(CelebByConsortiaLevel.Build());
        stringBuilder.Append(CelebByDayBestEquip.Build());
        stringBuilder.Append(celebbyconsortiafightpower.Build());
        stringBuilder.Append(celebbyweekleaguescore.Build());
        stringBuilder.Append(CelebByAchievementPointDayList.Build());
        stringBuilder.Append(CelebByAchievementPointList.Build());
        stringBuilder.Append(CelebByAchievementPointWeekList.Build());
        stringBuilder.Append(CelebByGiftGpList.Build());
        stringBuilder.Append(CelebByDayGiftGpList.Build());
        stringBuilder.Append(CelebByWeekGiftGP.Build());
        stringBuilder.Append(elitematchplayerlist.Build());
        context.Response.ContentType = "text/plain";
        context.Response.Write(stringBuilder.ToString());
      }
      else
        context.Response.Write("IP is not valid!" + context.Request.UserHostAddress);
    }

    public bool IsReusable => false;
  }
}
