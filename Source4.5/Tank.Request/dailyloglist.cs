// Decompiled with JetBrains decompiler
// Type: Tank.Request.dailyloglist
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [WebService(Namespace = "http://tempuri.org/")]
  public class dailyloglist : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected Sql_DbObject db = new Sql_DbObject("AppConfig", "conString");

    public bool IsReusable => false;

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str1 = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        string str2 = context.Request["key"];
        int UserID = int.Parse(context.Request["selfid"]);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          DailyLogListInfo info = playerBussiness.GetDailyLogListSingle(UserID);
          if (info == null)
            info = new DailyLogListInfo()
            {
              UserID = UserID,
              DayLog = "",
              UserAwardLog = 0,
              LastDate = DateTime.Now
            };
          string str3 = info.DayLog;
          int num1 = info.UserAwardLog;
          DateTime dateTime = info.LastDate;
          char[] chArray = new char[1]{ ',' };
          int length = str3.Split(chArray).Length;
          DateTime now = DateTime.Now;
          int month = now.Month;
          now = DateTime.Now;
          int year = now.Year;
          now = DateTime.Now;
          int day = now.Day;
          int num2 = DateTime.DaysInMonth(year, month);
          if (month != dateTime.Month || year != dateTime.Year)
          {
            str3 = "";
            num1 = 0;
            dateTime = DateTime.Now;
          }
          if (length < num2)
          {
            if (string.IsNullOrEmpty(str3) && length > 1)
              str3 = "False";
            for (int index = length; index < day - 1; ++index)
              str3 += ",False";
          }
          info.DayLog = str3;
          info.UserAwardLog = num1;
          info.LastDate = dateTime;
          playerBussiness.UpdateDailyLogList(info);
          XElement xelement = new XElement((XName) "DailyLogList", new object[4]
          {
            (object) new XAttribute((XName) "UserAwardLog", (object) num1),
            (object) new XAttribute((XName) "DayLog", (object) str3),
            (object) new XAttribute((XName) "luckyNum", (object) 0),
            (object) new XAttribute((XName) "myLuckyNum", (object) 0)
          });
          node.Add((object) xelement);
        }
        flag = true;
        str1 = "Success!";
      }
      catch (Exception ex)
      {
        dailyloglist.log.Error((object) nameof (dailyloglist), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str1));
      node.Add((object) new XAttribute((XName) "nowDate", (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
      context.Response.ContentType = "text/plain";
      context.Response.BinaryWrite(StaticFunction.Compress(node.ToString(false)));
    }

    public bool UpdateDailyLogList(DailyLogListInfo info)
    {
      bool flag = false;
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[5]
        {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@UserAwardLog", (object) info.UserAwardLog),
          new SqlParameter("@DayLog", (object) info.DayLog),
          new SqlParameter("@LastDate", (object) info.LastDate),
          new SqlParameter("@Result", SqlDbType.Int)
        };
        SqlParameters[4].Direction = ParameterDirection.ReturnValue;
        flag = this.db.RunProcedure("SP_DailyLogList_Update", SqlParameters);
      }
      catch (Exception ex)
      {
        if (dailyloglist.log.IsErrorEnabled)
          dailyloglist.log.Error((object) "SP_DailyLogList_Update", ex);
      }
      return flag;
    }
  }
}
