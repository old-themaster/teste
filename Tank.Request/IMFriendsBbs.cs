// Decompiled with JetBrains decompiler
// Type: Tank.Request.IMFriendsBbs
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class IMFriendsBbs : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement node = new XElement((XName) "Result");
      IMFriendsBbs.Normal normal = new IMFriendsBbs.Normal();
      StringBuilder stringBuilder1 = new StringBuilder();
      string uid = HttpContext.Current.Request.Params["Uid"];
      string s = normal.FriendsString(uid);
      DataSet dataSet = new DataSet();
      if (s != "")
      {
        try
        {
          int num = (int) dataSet.ReadXml((TextReader) new StringReader(s));
          for (int recordIndex = 0; recordIndex < dataSet.Tables["item"].DefaultView.Count; ++recordIndex)
            stringBuilder1.Append(dataSet.Tables["item"].DefaultView[recordIndex]["UserName"].ToString() + ",");
        }
        catch (Exception ex)
        {
          if (IMFriendsBbs.log.IsErrorEnabled)
            IMFriendsBbs.log.Error((object) "Get Table Item ", ex);
        }
      }
      if (stringBuilder1.Length <= 1 || s == "")
      {
        node.Add((object) new XAttribute((XName) "value", (object) flag));
        node.Add((object) new XAttribute((XName) "message", (object) str));
        context.Response.ContentType = "text/plain";
        context.Response.Write(node.ToString(false));
      }
      else
      {
        string[] strArray = stringBuilder1.ToString().Split(',');
        ArrayList arrayList = new ArrayList();
        StringBuilder stringBuilder2 = new StringBuilder(4000);
        for (int index = 0; index < ((IEnumerable<string>) strArray).Count<string>() && !(strArray[index] == ""); ++index)
        {
          if (stringBuilder2.Length + strArray[index].Length < 4000)
          {
            stringBuilder2.Append(strArray[index] + ",");
          }
          else
          {
            arrayList.Add((object) stringBuilder2.ToString());
            stringBuilder2.Remove(0, stringBuilder2.Length);
          }
        }
        arrayList.Add((object) stringBuilder2.ToString());
        try
        {
          for (int index1 = 0; index1 < arrayList.Count; ++index1)
          {
            string condictArray = arrayList[index1].ToString();
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
              FriendInfo[] friendsBbs = playerBussiness.GetFriendsBbs(condictArray);
              for (int index2 = 0; index2 < ((IEnumerable<FriendInfo>) friendsBbs).Count<FriendInfo>(); ++index2)
              {
                DataRow[] dataRowArray = dataSet.Tables["item"].Select("UserName='" + friendsBbs[index2].UserName + "'");
                XElement xelement = new XElement((XName) "Item", new object[7]
                {
                  (object) new XAttribute((XName) "NickName", (object) friendsBbs[index2].NickName),
                  (object) new XAttribute((XName) "UserName", (object) friendsBbs[index2].UserName),
                  (object) new XAttribute((XName) "UserId", (object) friendsBbs[index2].UserID),
                  (object) new XAttribute((XName) "Photo", dataRowArray[0]["Photo"] == null ? (object) "" : (object) dataRowArray[0]["Photo"].ToString()),
                  (object) new XAttribute((XName) "PersonWeb", dataRowArray[0]["PersonWeb"] == null ? (object) "" : (object) dataRowArray[0]["PersonWeb"].ToString()),
                  (object) new XAttribute((XName) "IsExist", (object) friendsBbs[index2].IsExist),
                  (object) new XAttribute((XName) "OtherName", dataRowArray[0]["OtherName"] == null ? (object) "" : (object) dataRowArray[0]["OtherName"].ToString())
                });
                node.Add((object) xelement);
              }
            }
          }
          flag = true;
          str = "Success!";
        }
        catch (Exception ex)
        {
          IMFriendsBbs.log.Error((object) "IMFriendsGood", ex);
        }
        node.Add((object) new XAttribute((XName) "value", (object) flag));
        node.Add((object) new XAttribute((XName) "message", (object) str));
        context.Response.ContentType = "text/plain";
        context.Response.Write(node.ToString(false));
      }
    }

    public bool IsReusable => false;

    public interface IAgentFriends
    {
      string FriendsString(string uid);
    }

    public class Normal : IMFriendsBbs.IAgentFriends
    {
      private string Url;

      public string FriendsString(string uid)
      {
        try
        {
          if (IMFriendsBbs.Normal.FriendInterface == "")
            return string.Empty;
          string err = "";
          this.Url = string.Format((IFormatProvider) CultureInfo.InvariantCulture, IMFriendsBbs.Normal.FriendInterface, (object) uid);
          string page = WebsResponse.GetPage(this.Url, "", "utf-8", out err);
          if (err == "")
            return page;
          throw new Exception(err);
        }
        catch (Exception ex)
        {
          if (IMFriendsBbs.log.IsErrorEnabled)
            IMFriendsBbs.log.Error((object) "Normal：", ex);
        }
        return string.Empty;
      }

      public static string FriendInterface => ConfigurationManager.AppSettings[nameof (FriendInterface)];
    }
  }
}
