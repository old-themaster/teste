// Decompiled with JetBrains decompiler
// Type: Tank.Request.ChargeMoney
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
  public class ChargeMoney : Page
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetChargeIP => ConfigurationSettings.AppSettings["ChargeIP"];

    protected void Page_Load(object sender, EventArgs e)
    {
      int num = 1;
      try
      {
        string userHostAddress = this.Context.Request.UserHostAddress;
        if (ChargeMoney.ValidLoginIP(userHostAddress))
        {
          string content = HttpUtility.UrlDecode(this.Request["content"]);
          string site = this.Request["site"] == null ? "" : HttpUtility.UrlDecode(this.Request["site"]).ToLower();
          int int32 = Convert.ToInt32(HttpUtility.UrlDecode(this.Request["nickname"]));
          string[] strArray = BaseInterface.CreateInterface().UnEncryptCharge(content, ref num, site);
          if (strArray.Length > 5)
          {
            string chargeID = strArray[0];
            string user = strArray[1].Trim();
            int money = int.Parse(strArray[2]);
            string str = strArray[3];
            Decimal needMoney = Decimal.Parse(strArray[4]);
            if (!string.IsNullOrEmpty(user))
            {
              string nameBySite = BaseInterface.GetNameBySite(user, site);
              if (money > 0)
              {
                using (PlayerBussiness playerBussiness1 = new PlayerBussiness())
                {
                  int userID = 0;
                  DateTime now = DateTime.Now;
                  if (playerBussiness1.AddChargeMoney(chargeID, nameBySite, money, str, needMoney, ref userID, ref num, now, userHostAddress, int32))
                  {
                    num = 0;
                    using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    {
                      centerServiceClient.ChargeMoney(userID, chargeID);
                      using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
                      {
                        PlayerInfo userSingleByUserId = playerBussiness2.GetUserSingleByUserID(userID);
                        if (userSingleByUserId != null)
                        {
                          StaticsMgr.Log(now, nameBySite, userSingleByUserId.Sex, money, str, needMoney);
                        }
                        else
                        {
                          StaticsMgr.Log(now, nameBySite, true, money, str, needMoney);
                          ChargeMoney.log.Error((object) "ChargeMoney_StaticsMgr:Player is null!");
                        }
                      }
                    }
                  }
                }
              }
              else
                num = 3;
            }
            else
              num = 2;
          }
        }
        else
          num = 5;
      }
      catch (Exception ex)
      {
        ChargeMoney.log.Error((object) "ChargeMoney:", ex);
      }
      this.Response.Write(num.ToString() + this.Context.Request.UserHostAddress);
    }

    public static bool ValidLoginIP(string ip)
    {
      string getChargeIp = ChargeMoney.GetChargeIP;
      int num;
      if (!string.IsNullOrEmpty(getChargeIp))
        num = ((IEnumerable<string>) getChargeIp.Split('|')).Contains<string>(ip) ? 1 : 0;
      else
        num = 1;
      return (uint) num > 0U;
    }
  }
}
