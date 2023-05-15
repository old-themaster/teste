// Decompiled with JetBrains decompiler
// Type: Tank.Request.LoadUserEquip
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
  public class LoadUserEquip : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        int UserID = int.Parse(context.Request["ID"]);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(UserID);
          node.Add((object) new XAttribute((XName) "Agility", (object) userSingleByUserId.Agility), (object) new XAttribute((XName) "Attack", (object) userSingleByUserId.Attack), (object) new XAttribute((XName) "Colors", (object) userSingleByUserId.Colors), (object) new XAttribute((XName) "Skin", (object) userSingleByUserId.Skin), (object) new XAttribute((XName) "Defence", (object) userSingleByUserId.Defence), (object) new XAttribute((XName) "GP", (object) userSingleByUserId.GP), (object) new XAttribute((XName) "Grade", (object) userSingleByUserId.Grade), (object) new XAttribute((XName) "Luck", (object) userSingleByUserId.Luck), (object) new XAttribute((XName) "Hide", (object) userSingleByUserId.Hide), (object) new XAttribute((XName) "Repute", (object) userSingleByUserId.Repute), (object) new XAttribute((XName) "Offer", (object) userSingleByUserId.Offer), (object) new XAttribute((XName) "NickName", (object) userSingleByUserId.NickName), (object) new XAttribute((XName) "ConsortiaName", (object) userSingleByUserId.ConsortiaName), (object) new XAttribute((XName) "ConsortiaID", (object) userSingleByUserId.ConsortiaID), (object) new XAttribute((XName) "ReputeOffer", (object) userSingleByUserId.ReputeOffer), (object) new XAttribute((XName) "ConsortiaHonor", (object) userSingleByUserId.ConsortiaHonor), (object) new XAttribute((XName) "ConsortiaLevel", (object) userSingleByUserId.ConsortiaLevel), (object) new XAttribute((XName) "ConsortiaRepute", (object) userSingleByUserId.ConsortiaRepute), (object) new XAttribute((XName) "WinCount", (object) userSingleByUserId.Win), (object) new XAttribute((XName) "TotalCount", (object) userSingleByUserId.Total), (object) new XAttribute((XName) "EscapeCount", (object) userSingleByUserId.Escape), (object) new XAttribute((XName) "Sex", (object) userSingleByUserId.Sex), (object) new XAttribute((XName) "Style", (object) userSingleByUserId.Style), (object) new XAttribute((XName) "FightPower", (object) userSingleByUserId.FightPower));
          foreach (SqlDataProvider.Data.ItemInfo info in playerBussiness.GetUserEuqip(UserID).ToArray())
            node.Add((object) FlashUtils.CreateGoodsInfo(info));
        }
        flag = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
        LoadUserEquip.log.Error((object) nameof (LoadUserEquip), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
