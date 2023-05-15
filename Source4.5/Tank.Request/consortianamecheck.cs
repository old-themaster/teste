// Decompiled with JetBrains decompiler
// Type: Tank.Request.consortianamecheck
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
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
  public class consortianamecheck : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
      bool flag = false;
      string str1 = "O nome já foi usado.";
      XElement node = new XElement((XName) "Result");
      try
      {
        string str2 = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
        if (Encoding.Default.GetByteCount(str2) <= 20)
        {
          if (!string.IsNullOrEmpty(str2))
          {
            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
            {
              if (consortiaBussiness.GetConsortiaSingleByName(str2) == null)
              {
                flag = true;
                str1 = "Sucesso! O nome pode ser utilizado.";
              }
            }
          }
        }
        else
          str1 = "O nome da associação é muito longo";
      }
      catch (Exception ex)
      {
        consortianamecheck.log.Error((object) "NickNameCheck", ex);
        flag = false;
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str1));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
