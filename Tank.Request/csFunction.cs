// Decompiled with JetBrains decompiler
// Type: Tank.Request.csFunction
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
  public class csFunction
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static string[] al = ";|and|1=1|exec|insert|select|delete|update|like|count|chr|mid|master|or|truncate|char|declare|join".Split('|');

    public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

    public static bool ValidAdminIP(string ip)
    {
      string getAdminIp = csFunction.GetAdminIP;
      if (!string.IsNullOrEmpty(getAdminIp))
      {
        if (!((IEnumerable<string>) getAdminIp.Split('|')).Contains<string>(ip))
          return false;
      }
      return true;
    }

    public static string ConvertSql(string inputString)
    {
      inputString = inputString.Trim().ToLower();
      inputString = inputString.Replace("'", "''");
      inputString = inputString.Replace(";--", "");
      inputString = inputString.Replace("=", "");
      inputString = inputString.Replace(" or", "");
      inputString = inputString.Replace(" or ", "");
      inputString = inputString.Replace(" and", "");
      inputString = inputString.Replace("and ", "");
      if (!csFunction.SqlChar(inputString))
        inputString = "";
      return inputString;
    }

    public static bool SqlChar(string v)
    {
      if (v.Trim() != "")
      {
        foreach (string str in csFunction.al)
        {
          if (v.IndexOf(str + " ") > -1 || v.IndexOf(" " + str) > -1)
            return false;
        }
      }
      return true;
    }

    public static string CreateCompressXml(
      HttpContext context,
      XElement result,
      string file,
      bool isCompress) => csFunction.CreateCompressXml(context.Server.MapPath("~"), result, file, isCompress);

    public static string CreateCompressXml(XElement result, string file, bool isCompress) => csFunction.CreateCompressXml(StaticsMgr.CurrentPath, result, file, isCompress);

    public static string CreateCompressXml(
      string path,
      XElement result,
      string file,
      bool isCompress)
    {
      try
      {
        file += ".xml";
        path = Path.Combine(path, file);
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
          if (isCompress)
          {
            using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
              binaryWriter.Write(StaticFunction.Compress(result.ToString(false)));
          }
          else
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
              streamWriter.Write(result.ToString(false));
          }
        }
        return "Build:" + file + ",Success!";
      }
      catch (Exception ex)
      {
        csFunction.log.Error((object) ("CreateCompressXml " + file + " is fail!"), ex);
        return "Build:" + file + ",Fail!";
      }
    }

    public static string BuildCelebConsortia(string file, int order) => csFunction.BuildCelebConsortia(file, order, "");

    public static string BuildCelebConsortia(string file, int order, string fileNotCompress)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      int total = 0;
      try
      {
        int page = 1;
        int size = 50;
        int consortiaID = -1;
        string name = "";
        int level = -1;
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          foreach (ConsortiaInfo info in consortiaBussiness.GetConsortiaPage(page, size, ref total, order, name, consortiaID, level, -1))
          {
            XElement consortiaInfo = FlashUtils.CreateConsortiaInfo(info);
            if (info.ChairmanID != 0)
            {
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
              {
                PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(info.ChairmanID);
                if (userSingleByUserId != null)
                  consortiaInfo.Add((object) FlashUtils.CreateCelebInfo(userSingleByUserId));
              }
            }
            result.Add((object) consortiaInfo);
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        csFunction.log.Error((object) (file + " is fail!"), ex);
      }
      result.Add((object) new XAttribute((XName) "total", (object) total));
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      result.Add((object) new XAttribute((XName) "date", (object) DateTime.Today.ToString("yyyy-MM-dd")));
      if (!string.IsNullOrEmpty(fileNotCompress))
        csFunction.CreateCompressXml(result, fileNotCompress, false);
      return csFunction.CreateCompressXml(result, file, true);
    }

    public static string BuildCelebUsers(string file, int order) => csFunction.BuildCelebUsers(file, order, "");

    public static string BuildEliteMatchPlayerList(string file)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        int page = 1;
        int size = 50;
        int userID = -1;
        int total = 0;
        bool resultValue = false;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(page, size, ref total, 7, userID, ref resultValue);
          if (resultValue)
          {
            int rank1 = 1;
            int rank2 = 1;
            XElement xelement1 = new XElement((XName) "ItemSet", (object) new XAttribute((XName) "value", (object) 1));
            XElement xelement2 = new XElement((XName) "ItemSet", (object) new XAttribute((XName) "value", (object) 2));
            foreach (PlayerInfo info in playerPage)
            {
              if (info.Grade <= 40)
              {
                xelement1.Add((object) FlashUtils.CreateEliteMatchPlayersList(info, rank1));
                ++rank1;
              }
              else
              {
                xelement2.Add((object) FlashUtils.CreateEliteMatchPlayersList(info, rank2));
                ++rank2;
              }
            }
            result.Add((object) xelement1);
            result.Add((object) xelement2);
            flag = true;
            str = "Success!";
          }
        }
      }
      catch (Exception ex)
      {
        csFunction.log.Error((object) (file + " is fail!"), ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      result.Add((object) new XAttribute((XName) "lastUpdateTime", (object) DateTime.Now.ToString()));
      return csFunction.CreateCompressXml(result, file, true);
    }

    public static string BuildCelebUsers(string file, int order, string fileNotCompress)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        int page = 1;
        int size = 50;
        int userID = -1;
        int total = 0;
        bool resultValue = false;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          if (order == 6)
            playerBussiness.UpdateUserReputeFightPower();
          PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(page, size, ref total, order, userID, ref resultValue);
          if (resultValue)
          {
            foreach (PlayerInfo info in playerPage)
              result.Add((object) FlashUtils.CreateCelebInfo(info));
            flag = true;
            str = "Success!";
          }
        }
      }
      catch (Exception ex)
      {
        csFunction.log.Error((object) (file + " is fail!"), ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      result.Add((object) new XAttribute((XName) "date", (object) DateTime.Today.ToString("yyyy-MM-dd")));
      if (!string.IsNullOrEmpty(fileNotCompress))
        csFunction.CreateCompressXml(result, fileNotCompress, false);
      return csFunction.CreateCompressXml(result, file, true);
    }

    public static string BuildCelebUsersMath(string file, string fileNotCompress)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        int page = 1;
        int size = 50;
        int total = 0;
        bool resultValue = false;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo[] playerMathPage = playerBussiness.GetPlayerMathPage(page, size, ref total, ref resultValue);
          if (resultValue)
          {
            foreach (PlayerInfo info in playerMathPage)
              result.Add((object) FlashUtils.CreateCelebInfo(info));
            flag = true;
            str = "Success!";
          }
        }
      }
      catch (Exception ex)
      {
        csFunction.log.Error((object) (file + " is fail!"), ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      result.Add((object) new XAttribute((XName) "date", (object) DateTime.Today.ToString("yyyy-MM-dd")));
      if (!string.IsNullOrEmpty(fileNotCompress))
        csFunction.CreateCompressXml(result, fileNotCompress, false);
      return csFunction.CreateCompressXml(result, file, true);
    }

    public static string BuildCelebConsortiaFightPower(string file, string fileNotCompress)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      int num = 0;
      try
      {
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          ConsortiaInfo[] consortiaInfoArray = consortiaBussiness.UpdateConsortiaFightPower();
          num = consortiaInfoArray.Length;
          foreach (ConsortiaInfo info in consortiaInfoArray)
          {
            XElement consortiaInfo = FlashUtils.CreateConsortiaInfo(info);
            if (info.ChairmanID != 0)
            {
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
              {
                PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(info.ChairmanID);
                if (userSingleByUserId != null)
                  consortiaInfo.Add((object) FlashUtils.CreateCelebInfo(userSingleByUserId));
              }
            }
            result.Add((object) consortiaInfo);
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        csFunction.log.Error((object) (file + " is fail!"), ex);
      }
      result.Add((object) new XAttribute((XName) "total", (object) num));
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      result.Add((object) new XAttribute((XName) "date", (object) DateTime.Today.ToString("yyyy-MM-dd")));
      if (!string.IsNullOrEmpty(fileNotCompress))
        csFunction.CreateCompressXml(result, fileNotCompress, false);
      return csFunction.CreateCompressXml(result, file, true);
    }
  }
}
