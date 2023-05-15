// Decompiled with JetBrains decompiler
// Type: Tank.Request.RenameNick
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Bussiness.Interface;
using log4net;
using Road.Flash;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class RenameNick : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
      bool flag = false;
      string translation = LanguageMgr.GetTranslation("Tank.Request.RenameNick.Fail1");
      XElement node = new XElement((XName) "Result");
      try
      {
        BaseInterface.CreateInterface();
        string src = context.Request["p"];
        if (context.Request["site"] != null)
          HttpUtility.UrlDecode(context.Request["site"]);
        string userHostAddress = context.Request.UserHostAddress;
        if (!string.IsNullOrEmpty(src))
        {
          byte[] bytes = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, src);
          string[] strArray = Encoding.UTF8.GetString(bytes, 7, bytes.Length - 7).Split(',');
          if (strArray.Length == 5)
          {
            string name = strArray[0];
            string pass = strArray[1];
            string str1 = strArray[2];
            string str2 = strArray[3];
            string str3 = strArray[4];
            if (PlayerManager.Login(name, pass))
            {
              using (new PlayerBussiness())
              {
                flag = true;
                translation = LanguageMgr.GetTranslation("Tank.Request.RenameNick.Success");
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        RenameNick.log.Error((object) nameof (RenameNick), ex);
        flag = false;
        translation = LanguageMgr.GetTranslation("Tank.Request.RenameNick.Fail2");
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) translation));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
