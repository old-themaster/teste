// Decompiled with JetBrains decompiler
// Type: Tank.Request.LoadUserMail
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
  public class LoadUserMail : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node1 = new XElement((XName) "Result");
      try
      {
        int userID = int.Parse(context.Request.QueryString["selfid"]);
        if (userID != 0)
        {
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            foreach (MailInfo mailInfo in playerBussiness.GetMailByUserID(userID))
            {
              XElement node2 = new XElement((XName) "Item", new object[15]
              {
                (object) new XAttribute((XName) "ID", (object) mailInfo.ID),
                (object) new XAttribute((XName) "Title", (object) mailInfo.Title),
                (object) new XAttribute((XName) "Content", (object) mailInfo.Content),
                (object) new XAttribute((XName) "Sender", (object) mailInfo.Sender),
                (object) new XAttribute((XName) "SendTime", (object) mailInfo.SendTime.ToString("yyyy-MM-dd HH:mm:ss")),
                (object) new XAttribute((XName) "Gold", (object) mailInfo.Gold),
                (object) new XAttribute((XName) "Money", (object) mailInfo.Money),
                (object) new XAttribute((XName) "Annex1ID", mailInfo.Annex1 == null ? (object) "" : (object) mailInfo.Annex1),
                (object) new XAttribute((XName) "Annex2ID", mailInfo.Annex2 == null ? (object) "" : (object) mailInfo.Annex2),
                (object) new XAttribute((XName) "Annex3ID", mailInfo.Annex3 == null ? (object) "" : (object) mailInfo.Annex3),
                (object) new XAttribute((XName) "Annex4ID", mailInfo.Annex4 == null ? (object) "" : (object) mailInfo.Annex4),
                (object) new XAttribute((XName) "Annex5ID", mailInfo.Annex5 == null ? (object) "" : (object) mailInfo.Annex5),
                (object) new XAttribute((XName) "Type", (object) mailInfo.Type),
                (object) new XAttribute((XName) "ValidDate", (object) mailInfo.ValidDate),
                (object) new XAttribute((XName) "IsRead", (object) mailInfo.IsRead)
              });
              LoadUserMail.AddAnnex(node2, mailInfo.Annex1);
              LoadUserMail.AddAnnex(node2, mailInfo.Annex2);
              LoadUserMail.AddAnnex(node2, mailInfo.Annex3);
              LoadUserMail.AddAnnex(node2, mailInfo.Annex4);
              LoadUserMail.AddAnnex(node2, mailInfo.Annex5);
              node1.Add((object) node2);
            }
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        LoadUserMail.log.Error((object) nameof (LoadUserMail), ex);
      }
      node1.Add((object) new XAttribute((XName) "value", (object) flag));
      node1.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.BinaryWrite(StaticFunction.Compress(node1.ToString(false)));
    }

    public static void AddAnnex(XElement node, string value)
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        if (string.IsNullOrEmpty(value))
          return;
        SqlDataProvider.Data.ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(int.Parse(value));
        if (userItemSingle == null)
          return;
        node.Add((object) FlashUtils.CreateGoodsInfo(userItemSingle));
      }
    }

    public bool IsReusable => false;
  }
}
