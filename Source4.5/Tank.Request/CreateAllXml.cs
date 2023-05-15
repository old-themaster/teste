// Decompiled with JetBrains decompiler
// Type: Tank.Request.CreateAllXml
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class CreateAllXml : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(ActiveList.Bulid(context));
        stringBuilder.Append(activitysystemitems.Bulid(context));
        stringBuilder.Append(BallList.Bulid(context));
        stringBuilder.Append(eventrewarditemlist.Bulid(context));
        stringBuilder.Append(LoadMapsItems.Bulid(context));
        stringBuilder.Append(LoadPVEItems.Build(context));
        stringBuilder.Append(QuestList.Bulid(context));
        stringBuilder.Append(TemplateAllList.Bulid(context));
        stringBuilder.Append(ShopItemList.Bulid(context));
        stringBuilder.Append(ShopGoodsShowList.Bulid(context));
        stringBuilder.Append(LoadItemsCategory.Bulid(context));
        stringBuilder.Append(MapServerList.Bulid(context));
        stringBuilder.Append(ConsortiaLevelList.Bulid(context));
        stringBuilder.Append(DailyAwardList.Bulid(context));
        stringBuilder.Append(NPCInfoList.Bulid(context));
        stringBuilder.Append(AchievementList.Bulid(context));
        stringBuilder.Append(newtitleinfo.Bulid(context));

                context.Response.ContentType = "text/plain";
        context.Response.Write(stringBuilder.ToString());
      }
      else
        context.Response.Write("IP is not valid!");
    }

    public bool IsReusable => false;
  }
}
