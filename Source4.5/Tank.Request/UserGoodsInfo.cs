// Decompiled with JetBrains decompiler
// Type: Tank.Request.UserGoodsInfo
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class UserGoodsInfo : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        int itemID = int.Parse(context.Request.Params["ID"]);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          SqlDataProvider.Data.ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(itemID);
          node.Add((object) FlashUtils.CreateGoodsInfo(userItemSingle));
        }
        flag = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
        UserGoodsInfo.log.Error((object) nameof (UserGoodsInfo), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
