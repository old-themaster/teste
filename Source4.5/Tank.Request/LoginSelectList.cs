﻿// Decompiled with JetBrains decompiler
// Type: Tank.Request.LoginSelectList
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
  public class LoginSelectList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        string userName = HttpUtility.UrlDecode(context.Request["username"]);
        HttpUtility.UrlDecode(context.Request["password"]);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo[] userLoginList = playerBussiness.GetUserLoginList(userName);
          if (userLoginList.Length == 0)
            return;
          foreach (PlayerInfo info in userLoginList)
          {
            if (!string.IsNullOrEmpty(info.NickName))
              node.Add((object) FlashUtils.CreateUserLoginList(info));
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        LoginSelectList.log.Error((object) nameof (LoginSelectList), ex);
      }
      finally
      {
        node.Add((object) new XAttribute((XName) "value", (object) flag));
        node.Add((object) new XAttribute((XName) "message", (object) str));
        context.Response.ContentType = "text/plain";
        context.Response.Write(node.ToString(false));
      }
    }

    public bool IsReusable => false;
  }
}
