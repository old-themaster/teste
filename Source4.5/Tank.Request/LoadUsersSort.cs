// Decompiled with JetBrains decompiler
// Type: Tank.Request.LoadUsersSort
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
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class LoadUsersSort : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      int total = 0;
      try
      {
        int page = 1;
        int size = 10;
        int order = int.Parse(context.Request["order"]);
        int userID = -1;
        bool resultValue = false;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(page, size, ref total, order, userID, ref resultValue);
          if (resultValue)
          {
            foreach (PlayerInfo playerInfo in playerPage)
            {
              XElement xelement = new XElement((XName) "Item", new object[19]
              {
                (object) new XAttribute((XName) "ID", (object) playerInfo.ID),
                (object) new XAttribute((XName) "NickName", playerInfo.NickName == null ? (object) "" : (object) playerInfo.NickName),
                (object) new XAttribute((XName) "Grade", (object) playerInfo.Grade),
                (object) new XAttribute((XName) "Colors", playerInfo.Colors == null ? (object) "" : (object) playerInfo.Colors),
                (object) new XAttribute((XName) "Skin", playerInfo.Skin == null ? (object) "" : (object) playerInfo.Skin),
                (object) new XAttribute((XName) "Sex", (object) playerInfo.Sex),
                (object) new XAttribute((XName) "Style", playerInfo.Style == null ? (object) "" : (object) playerInfo.Style),
                (object) new XAttribute((XName) "ConsortiaName", playerInfo.ConsortiaName == null ? (object) "" : (object) playerInfo.ConsortiaName),
                (object) new XAttribute((XName) "Hide", (object) playerInfo.Hide),
                (object) new XAttribute((XName) "Offer", (object) playerInfo.Offer),
                (object) new XAttribute((XName) "ReputeOffer", (object) playerInfo.ReputeOffer),
                (object) new XAttribute((XName) "ConsortiaHonor", (object) playerInfo.ConsortiaHonor),
                (object) new XAttribute((XName) "ConsortiaLevel", (object) playerInfo.ConsortiaLevel),
                (object) new XAttribute((XName) "ConsortiaRepute", (object) playerInfo.ConsortiaRepute),
                (object) new XAttribute((XName) "WinCount", (object) playerInfo.Win),
                (object) new XAttribute((XName) "TotalCount", (object) playerInfo.Total),
                (object) new XAttribute((XName) "EscapeCount", (object) playerInfo.Escape),
                (object) new XAttribute((XName) "Repute", (object) playerInfo.Repute),
                (object) new XAttribute((XName) "GP", (object) playerInfo.GP)
              });
              node.Add((object) xelement);
            }
            flag = true;
            str = "Success!";
          }
        }
      }
      catch (Exception ex)
      {
        LoadUsersSort.log.Error((object) nameof (LoadUsersSort), ex);
      }
      node.Add((object) new XAttribute((XName) "total", (object) total));
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
