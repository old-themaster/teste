// Decompiled with JetBrains decompiler
// Type: Tank.Request.ChargeTest
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness.Interface;
using System;
using System.Configuration;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class ChargeTest : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      try
      {
        string str1 = context.Request["chargeID"];
        string str2 = context.Request["userName"];
        int num1 = int.Parse(context.Request["money"]);
        string str3 = context.Request["payWay"];
        Decimal num2 = Decimal.Parse(context.Request["needMoney"]);
        string str4 = context.Request["nickname"] == null ? "" : HttpUtility.UrlDecode(context.Request["nickname"]);
        string str5 = "";
        QYInterface qyInterface = new QYInterface();
        string empty = string.Empty;
        string str6 = string.IsNullOrEmpty(str5) ? BaseInterface.GetChargeKey : ConfigurationManager.AppSettings[string.Format("ChargeKey_{0}", (object) str5)];
        string str7 = BaseInterface.md5(str1 + str2 + (object) num1 + str3 + (object) num2 + str6);
        string Url = "http://192.168.0.4:828/ChargeMoney.aspx?content=" + str1 + "|" + str2 + "|" + (object) num1 + "|" + str3 + "|" + (object) num2 + "|" + str7 + "&site=" + str5 + "&nickname=" + HttpUtility.UrlEncode(str4);
        context.Response.Write(BaseInterface.RequestContent(Url));
      }
      catch
      {
        context.Response.Write("false");
      }
    }

    public bool IsReusable => false;
  }
}
