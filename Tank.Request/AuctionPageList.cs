// Decompiled with JetBrains decompiler
// Type: Tank.Request.AuctionPageList
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
  public class AuctionPageList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str1 = "Fail!";
      int total = 0;
      XElement node = new XElement((XName) "Result");
      try
      {
        int page = int.Parse(context.Request["page"]);
        string name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));
        int type = int.Parse(context.Request["type"]);
        int pay = int.Parse(context.Request["pay"]);
        int userID = int.Parse(context.Request["userID"]);
        int buyID = int.Parse(context.Request["buyID"]);
        int order = int.Parse(context.Request["order"]);
        bool sort = bool.Parse(context.Request["sort"]);
        string str2 = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["Auctions"]));
        string string_1 = string.IsNullOrEmpty(str2) ? "0" : str2;
        int size = 50;
        using (PlayerBussiness playerBussiness1 = new PlayerBussiness())
        {
          foreach (AuctionInfo info in playerBussiness1.GetAuctionPage(page, name, type, pay, ref total, userID, buyID, order, sort, size, string_1))
          {
            XElement auctionInfo = FlashUtils.CreateAuctionInfo(info);
            using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
            {
              SqlDataProvider.Data.ItemInfo userItemSingle = playerBussiness2.GetUserItemSingle(info.ItemID);
              if (userItemSingle != null)
                auctionInfo.Add((object) FlashUtils.CreateGoodsInfo(userItemSingle));
              node.Add((object) auctionInfo);
            }
          }
          flag = true;
          str1 = "Success!";
        }
      }
      catch (Exception ex)
      {
        AuctionPageList.log.Error((object) nameof (AuctionPageList), ex);
      }
      node.Add((object) new XAttribute((XName) "total", (object) total));
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str1));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
