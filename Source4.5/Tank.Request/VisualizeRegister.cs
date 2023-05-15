// Decompiled with JetBrains decompiler
// Type: Tank.Request.VisualizeRegister
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Tank.Request.Illegalcharacters;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class VisualizeRegister : IHttpHandler
  {
    private static FileSystem fileIllegal = new FileSystem(HttpContext.Current.Server.MapPath(VisualizeRegister.IllegalCharacters), HttpContext.Current.Server.MapPath(VisualizeRegister.IllegalDirectory), "*.txt");
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string IllegalCharacters => ConfigurationManager.AppSettings[nameof (IllegalCharacters)];

    public static string IllegalDirectory => ConfigurationManager.AppSettings[nameof (IllegalDirectory)];

    public bool IsReusable => false;

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string translation = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Fail1");
      XElement node = new XElement((XName) "Result");
      try
      {
        NameValueCollection nameValueCollection = context.Request.Params;
        string userName = nameValueCollection["Name"];
        string passWord = nameValueCollection["Pass"];
        string str1 = nameValueCollection["NickName"].Trim().Replace(",", "");
        string str2 = nameValueCollection["Arm"];
        string str3 = nameValueCollection["Hair"];
        string str4 = nameValueCollection["Face"];
        string str5 = nameValueCollection["Cloth"];
        string str6 = nameValueCollection["Cloth"];
        string str7 = nameValueCollection["ArmID"];
        string str8 = nameValueCollection["HairID"];
        string str9 = nameValueCollection["FaceID"];
        string str10 = nameValueCollection["ClothID"];
        string str11 = nameValueCollection["ClothID"];
        int sex = -1;
        if (bool.Parse(ConfigurationManager.AppSettings["MustSex"]))
          sex = bool.Parse(nameValueCollection["Sex"]) ? 1 : 0;
        if (Encoding.Default.GetByteCount(str1) <= 14)
        {
          if (!VisualizeRegister.fileIllegal.checkIllegalChar(str1))
          {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord) && !string.IsNullOrEmpty(str1))
            {
              string[] strArray1;
              if (sex != 1)
                strArray1 = ConfigurationManager.AppSettings["GrilVisualizeItem"].Split(';');
              else
                strArray1 = ConfigurationManager.AppSettings["BoyVisualizeItem"].Split(';');
              string[] strArray2 = strArray1;
              char[] chArray1 = new char[1]{ ',' };
              string str12 = strArray2[0].Split(chArray1)[0];
              char[] chArray2 = new char[1]{ ',' };
              string str13 = strArray2[0].Split(chArray2)[1];
              char[] chArray3 = new char[1]{ ',' };
              string str14 = strArray2[0].Split(chArray3)[2];
              char[] chArray4 = new char[1]{ ',' };
              string str15 = strArray2[0].Split(chArray4)[3];
              char[] chArray5 = new char[1]{ ',' };
              string str16 = strArray2[0].Split(chArray5)[4];
              string armColor = "";
              string hairColor = "";
              string faceColor = "";
              string clothColor = "";
              string hatColor = "";
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
              {
                string str17 = str12 + "," + str13 + "," + str14 + "," + str15 + "," + str16;
                if (playerBussiness.RegisterPlayer(userName, passWord, str1, str17, str17, armColor, hairColor, faceColor, clothColor, hatColor, sex, ref translation, int.Parse(ConfigurationManager.AppSettings["ValidDate"])))
                {
                  flag = true;
                  translation = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Success");
                }
              }
            }
            else
              translation = LanguageMgr.GetTranslation("!string.IsNullOrEmpty(name) && !");
          }
          else
            translation = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Illegalcharacters");
        }
        else
          translation = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Long");
      }
      catch (Exception ex)
      {
        VisualizeRegister.log.Error((object) nameof (VisualizeRegister), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) translation));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }
  }
}
