// Decompiled with JetBrains decompiler
// Type: Tank.Request.SentReward
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class SentReward : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetSentRewardIP => ConfigurationManager.AppSettings["SentRewardIP"];

    public static string GetSentRewardKey => ConfigurationManager.AppSettings["SentRewardKey"];

    public static bool ValidSentRewardIP(string ip)
    {
      string getSentRewardIp = SentReward.GetSentRewardIP;
      if (!string.IsNullOrEmpty(getSentRewardIp))
      {
        if (!((IEnumerable<string>) getSentRewardIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      try
      {
        int result = 1;
        if (SentReward.ValidSentRewardIP(context.Request.UserHostAddress))
        {
          string content = HttpUtility.UrlDecode(context.Request["content"]);
          string getSentRewardKey = SentReward.GetSentRewardKey;
          string[] strArray = BaseInterface.CreateInterface().UnEncryptSentReward(content, ref result, getSentRewardKey);
          if (strArray.Length == 8 && result != 5 && (result != 6 && result != 7))
          {
            string str1 = strArray[0];
            string str2 = strArray[1];
            string str3 = strArray[2];
            int.Parse(strArray[3]);
            int.Parse(strArray[4]);
            string str4 = strArray[5];
            if (this.checkParam(ref str4))
            {
              PlayerBussiness playerBussiness = new PlayerBussiness();
            }
            else
              result = 4;
          }
        }
        else
          result = 3;
        context.Response.Write((object) result);
      }
      catch (Exception ex)
      {
        SentReward.log.Error((object) nameof (SentReward), ex);
      }
    }

    private bool checkParam(ref string param)
    {
      int num1 = 0;
      string str1 = "1";
      int num2 = 9;
      int num3 = 0;
      string str2 = "0";
      string str3 = "10";
      string str4 = "20";
      string str5 = "30";
      string str6 = "40";
      string str7 = "1";
      string str8 = "0";
      if (!string.IsNullOrEmpty(param))
      {
        string[] strArray1 = param.Split('|');
        int length = strArray1.Length;
        if (length > 0)
        {
          param = "";
          int index1 = 0;
          foreach (string str9 in strArray1)
          {
            char[] chArray = new char[1]{ ',' };
            string[] strArray2 = str9.Split(chArray);
            if (strArray2.Length != 0)
            {
              strArray1[index1] = "";
              strArray2[2] = int.Parse(strArray2[2]) < num1 || string.IsNullOrEmpty(strArray2[2].ToString()) ? str1 : strArray2[2];
              strArray2[3] = int.Parse(strArray2[3].ToString()) < num3 || int.Parse(strArray2[3].ToString()) > num2 || string.IsNullOrEmpty(strArray2[3].ToString()) ? num3.ToString() : strArray2[3];
              strArray2[4] = strArray2[4] == str2 || strArray2[4] == str3 || (strArray2[4] == str4 || strArray2[4] == str5) || strArray2[4] == str6 && !string.IsNullOrEmpty(strArray2[4].ToString()) ? strArray2[4] : str2;
              strArray2[5] = strArray2[5] == str2 || strArray2[5] == str3 || (strArray2[5] == str4 || strArray2[5] == str5) || strArray2[5] == str6 && !string.IsNullOrEmpty(strArray2[5].ToString()) ? strArray2[5] : str2;
              strArray2[6] = strArray2[6] == str2 || strArray2[6] == str3 || (strArray2[6] == str4 || strArray2[6] == str5) || strArray2[6] == str6 && !string.IsNullOrEmpty(strArray2[6].ToString()) ? strArray2[6] : str2;
              strArray2[7] = strArray2[7] == str2 || strArray2[7] == str3 || (strArray2[7] == str4 || strArray2[7] == str5) || strArray2[7] == str6 && !string.IsNullOrEmpty(strArray2[7].ToString()) ? strArray2[7] : str2;
              strArray2[8] = strArray2[8] == str7 || strArray2[8] == str8 && !string.IsNullOrEmpty(strArray2[8]) ? strArray2[8] : str7;
            }
            for (int index2 = 0; index2 < 9; ++index2)
              strArray1[index1] = strArray1[index1] + strArray2[index2] + ",";
            strArray1[index1] = strArray1[index1].Remove(strArray1[index1].Length - 1, 1);
            ++index1;
          }
          for (int index2 = 0; index2 < length; ++index2)
            param = param + strArray1[index2] + "|";
          param = param.Remove(param.Length - 1, 1);
          return true;
        }
      }
      return false;
    }

    public bool IsReusable => false;
  }
}
