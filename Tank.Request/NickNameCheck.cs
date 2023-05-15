// Decompiled with JetBrains decompiler
// Type: Tank.Request.NickNameCheck
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
  public class NickNameCheck : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static FileSystem fileIllegal = new FileSystem(HttpContext.Current.Server.MapPath(NickNameCheck.IllegalCharacters), HttpContext.Current.Server.MapPath(NickNameCheck.IllegalDirectory), "*.txt");
    private static string CharacterAllow = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ.-_";

    public static string IllegalCharacters => ConfigurationManager.AppSettings[nameof (IllegalCharacters)];

    public static string IllegalDirectory => ConfigurationManager.AppSettings[nameof (IllegalDirectory)];

    private bool CheckCharacterAllow(string text)
    {
      bool flag = true;
      foreach (char ch in text.ToCharArray())
      {
        if (!((IEnumerable<char>) NickNameCheck.CharacterAllow.ToCharArray()).Contains<char>(ch))
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    public void ProcessRequest(HttpContext context)
    {
      LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
      bool flag = false;
      string translation = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Exist");
      XElement node = new XElement((XName) "Result");
      try
      {
        string str = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
        if (Encoding.Default.GetByteCount(str) <= 14)
        {
          if (!NickNameCheck.fileIllegal.checkIllegalChar(str))
          {
            if (!string.IsNullOrEmpty(str))
            {
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
              {
                if (playerBussiness.GetUserSingleByNickName(str) == null)
                {
                  flag = true;
                  translation = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Right");
                }
              }
            }
          }
          else
            translation = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Illegalcharacters");
        }
        else
          translation = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Long");
      }
      catch (Exception ex)
      {
        NickNameCheck.log.Error((object) nameof (NickNameCheck), ex);
        flag = false;
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) translation));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
