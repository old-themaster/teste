// Decompiled with JetBrains decompiler
// Type: Tank.Request.Login
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.Interface;
using Game.Base;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class Login : IHttpHandler, IRequiresSessionState
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string translation = LanguageMgr.GetTranslation("Tank.Request.Login.Fail1");
      bool isError = false;
      XElement node = new XElement((XName) "Result");
      string str1 = context.Request["p"];
      try
      {
        BaseInterface baseInterface = BaseInterface.CreateInterface();
        string site = context.Request["site"] == null ? "" : HttpUtility.UrlDecode(context.Request["site"]);
        string userHostAddress = context.Request.UserHostAddress;
        if (string.IsNullOrEmpty(str1))
          return;
        byte[] bytes = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, str1);
        string[] strArray = Encoding.UTF8.GetString(bytes, 7, bytes.Length - 7).Split(',');
        if (strArray.Length != 4)
          return;
        string name = strArray[0];
        string pass = strArray[1];
        string str2 = strArray[2];
        string nickname = strArray[3];
        if (PlayerManager.Login(name, pass))
        {
          int isFirst = 0;
          bool isActive = false;
          bool byUserIsFirst = PlayerManager.GetByUserIsFirst(name);
          PlayerInfo login = baseInterface.CreateLogin(name, str2, int.Parse(ConfigurationManager.AppSettings["ServerID"]), ref translation, ref isFirst, userHostAddress, ref isError, byUserIsFirst, ref isActive, site, nickname);
          if (isActive)
            StaticsMgr.RegCountAdd();
          if (login != null && !isError)
          {
            if (isFirst == 0)
              PlayerManager.Update(name, str2);
            else
              PlayerManager.Remove(name);
            string str3 = string.IsNullOrEmpty(login.Style) ? ",,,,,,,," : login.Style;
            login.Colors = string.IsNullOrEmpty(login.Colors) ? ",,,,,,,," : login.Colors;
            XElement xelement = new XElement((XName) "Item", new object[44]
            {
              (object) new XAttribute((XName) "ID", (object) login.ID),
              (object) new XAttribute((XName) "IsFirst", (object) isFirst),
              (object) new XAttribute((XName) "NickName", (object) login.NickName),
              (object) new XAttribute((XName) "Date", (object) ""),
              (object) new XAttribute((XName) "IsConsortia", (object) 0),
              (object) new XAttribute((XName) "ConsortiaID", (object) login.ConsortiaID),
              (object) new XAttribute((XName) "Sex", (object) login.Sex),
              (object) new XAttribute((XName) "WinCount", (object) login.Win),
              (object) new XAttribute((XName) "TotalCount", (object) login.Total),
              (object) new XAttribute((XName) "EscapeCount", (object) login.Escape),
              (object) new XAttribute((XName) "DutyName", login.DutyName == null ? (object) "" : (object) login.DutyName),
              (object) new XAttribute((XName) "GP", (object) login.GP),
              (object) new XAttribute((XName) "Honor", (object) ""),
              (object) new XAttribute((XName) "Style", (object) str3),
              (object) new XAttribute((XName) "Gold", (object) login.Gold),
              (object) new XAttribute((XName) "Colors", login.Colors == null ? (object) "" : (object) login.Colors),
              (object) new XAttribute((XName) "Attack", (object) login.Attack),
              (object) new XAttribute((XName) "Defence", (object) login.Defence),
              (object) new XAttribute((XName) "Agility", (object) login.Agility),
              (object) new XAttribute((XName) "Luck", (object) login.Luck),
              (object) new XAttribute((XName) "Grade", (object) login.Grade),
              (object) new XAttribute((XName) "Hide", (object) login.Hide),
              (object) new XAttribute((XName) "Repute", (object) login.Repute),
              (object) new XAttribute((XName) "ConsortiaName", login.ConsortiaName == null ? (object) "" : (object) login.ConsortiaName),
              (object) new XAttribute((XName) "Offer", (object) login.Offer),
              (object) new XAttribute((XName) "Skin", login.Skin == null ? (object) "" : (object) login.Skin),
              (object) new XAttribute((XName) "ReputeOffer", (object) login.ReputeOffer),
              (object) new XAttribute((XName) "ConsortiaHonor", (object) login.ConsortiaHonor),
              (object) new XAttribute((XName) "ConsortiaLevel", (object) login.ConsortiaLevel),
              (object) new XAttribute((XName) "ConsortiaRepute", (object) login.ConsortiaRepute),
              (object) new XAttribute((XName) "Money", (object) (login.Money + login.MoneyLock)),
              (object) new XAttribute((XName) "AntiAddiction", (object) login.AntiAddiction),
              (object) new XAttribute((XName) "IsMarried", (object) login.IsMarried),
              (object) new XAttribute((XName) "SpouseID", (object) login.SpouseID),
              (object) new XAttribute((XName) "SpouseName", login.SpouseName == null ? (object) "" : (object) login.SpouseName),
              (object) new XAttribute((XName) "MarryInfoID", (object) login.MarryInfoID),
              (object) new XAttribute((XName) "IsCreatedMarryRoom", (object) login.IsCreatedMarryRoom),
              (object) new XAttribute((XName) "IsGotRing", (object) login.IsGotRing),
              (object) new XAttribute((XName) "LoginName", login.UserName == null ? (object) "" : (object) login.UserName),
              (object) new XAttribute((XName) "Nimbus", (object) login.Nimbus),
              (object) new XAttribute((XName) "FightPower", (object) login.FightPower),
              (object) new XAttribute((XName) "AnswerSite", (object) login.AnswerSite),
              (object) new XAttribute((XName) "WeaklessGuildProgressStr", login.WeaklessGuildProgressStr == null ? (object) "" : (object) login.WeaklessGuildProgressStr),
              (object) new XAttribute((XName) "IsOldPlayer", (object) false)
            });
            node.Add((object) xelement);
            flag = true;
            translation = LanguageMgr.GetTranslation("Tank.Request.Login.Success");
          }
          else
            PlayerManager.Remove(name);
        }
        else
        {
          Login.log.Error((object) ("name:" + name + "-pwd:" + pass));
          translation = LanguageMgr.GetTranslation("BaseInterface.LoginAndUpdate.Try");
        }
      }
      catch (Exception ex)
      {
        byte[] dump = Convert.FromBase64String(str1);
        Login.log.Error((object) ("User Login error: (--" + (object) StaticFunction.RsaCryptor.KeySize + "--)" + ex.ToString()));
        Login.log.Error((object) ("--dataarray: " + Marshal.ToHexDump("fuckingbitch " + (object) dump.Length, dump)));
        flag = false;
        translation = LanguageMgr.GetTranslation("Tank.Request.Login.Fail2");
      }
      finally
      {
        node.Add((object) new XAttribute((XName) "value", (object) flag));
        node.Add((object) new XAttribute((XName) "message", (object) translation));
        context.Response.ContentType = "text/plain";
        context.Response.Write(node.ToString(false));
      }
    }

    public bool IsReusable => false;
  }
}
