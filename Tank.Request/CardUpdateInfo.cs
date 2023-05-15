// Decompiled with JetBrains decompiler
// Type: Tank.Request.CardUpdateInfo
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class CardUpdateInfo : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context) => context.Response.Write(CardUpdateInfo.Bulid(context));

    public static string Bulid(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        using (ProduceBussiness produceBussiness = new ProduceBussiness())
        {
          foreach (SqlDataProvider.Data.CardUpdateInfo info in produceBussiness.GetAllCardUpdateInfo())
            result.Add((object) FlashUtils.CreateCardUpdateInfo(info));
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        CardUpdateInfo.log.Error((object) "Load CardUpdateInfo is fail!", ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (CardUpdateInfo), true);
    }

    public bool IsReusable => false;
  }
}
