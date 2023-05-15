// Decompiled with JetBrains decompiler
// Type: Tank.Request.IMListLoad
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class IMListLoad : IHttpHandler, IRequiresSessionState
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        int int32 = Convert.ToInt32(context.Request["id"]);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          FriendInfo[] friendsAll = playerBussiness.GetFriendsAll(int32);
          XElement xelement1 = new XElement((XName) "customList", new object[2]
          {
            (object) new XAttribute((XName) "ID", (object) 0),
            (object) new XAttribute((XName) "Name", (object) "Arkadaş Listesi")
          });
          node.Add((object) xelement1);
          foreach (FriendInfo friendInfo in friendsAll)
          {
            XElement xelement2 = new XElement((XName) "Item", new object[20]
            {
              (object) new XAttribute((XName) "ID", (object) friendInfo.FriendID),
              (object) new XAttribute((XName) "NickName", (object) friendInfo.NickName),
              (object) new XAttribute((XName) "Birthday", (object) DateTime.Now),
              (object) new XAttribute((XName) "ApprenticeshipState", (object) 0),
              (object) new XAttribute((XName) "LoginName", (object) friendInfo.UserName),
              (object) new XAttribute((XName) "Style", (object) friendInfo.Style),
              (object) new XAttribute((XName) "Sex", (object) (friendInfo.Sex == 1)),
              (object) new XAttribute((XName) "Colors", (object) friendInfo.Colors),
              (object) new XAttribute((XName) "Grade", (object) friendInfo.Grade),
              (object) new XAttribute((XName) "Hide", (object) friendInfo.Hide),
              (object) new XAttribute((XName) "ConsortiaName", (object) friendInfo.ConsortiaName),
              (object) new XAttribute((XName) "TotalCount", (object) friendInfo.Total),
              (object) new XAttribute((XName) "EscapeCount", (object) friendInfo.Escape),
              (object) new XAttribute((XName) "WinCount", (object) friendInfo.Win),
              (object) new XAttribute((XName) "Offer", (object) friendInfo.Offer),
              (object) new XAttribute((XName) "Relation", (object) friendInfo.Relation),
              (object) new XAttribute((XName) "Repute", (object) friendInfo.Repute),
              (object) new XAttribute((XName) "State", (object) (friendInfo.State == 1 ? 1 : 0)),
              (object) new XAttribute((XName) "Nimbus", (object) friendInfo.Nimbus),
              (object) new XAttribute((XName) "DutyName", (object) friendInfo.DutyName)
            });
            node.Add((object) xelement2);
          }
        }
        flag = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
        IMListLoad.log.Error((object) nameof (IMListLoad), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
