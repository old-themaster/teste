// Decompiled with JetBrains decompiler
// Type: Tank.Request.giftsendlog
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class giftsendlog : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = false;
      string str1 = "Fail!";
      XElement node = new XElement((XName) "Result");
      try
      {
        string str2 = context.Request["key"];
        int.Parse(context.Request["selfid"]);
        int userid = int.Parse(context.Request["userID"]);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          UserGiftInfo[] allUserGifts = playerBussiness.GetAllUserGifts(userid, false);
          if (allUserGifts != null)
          {
            foreach (UserGiftInfo userGiftInfo in allUserGifts)
            {
              XElement xelement = new XElement((XName) "Item", new object[3]
              {
                (object) new XAttribute((XName) "playerID", (object) userGiftInfo.ReceiverID),
                (object) new XAttribute((XName) "TemplateID", (object) userGiftInfo.TemplateID),
                (object) new XAttribute((XName) "count", (object) userGiftInfo.Count)
              });
              node.Add((object) xelement);
            }
          }
        }
        flag = true;
        str1 = "Success!";
      }
      catch (Exception ex)
      {
        giftsendlog.log.Error((object) nameof (giftsendlog), ex);
      }
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str1));
      context.Response.ContentType = "text/plain";
      context.Response.BinaryWrite(StaticFunction.Compress(node.ToString(false)));
    }

    public bool IsReusable => false;
  }
}
