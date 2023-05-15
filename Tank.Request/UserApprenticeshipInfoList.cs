// Decompiled with JetBrains decompiler
// Type: Tank.Request.UserApprenticeshipInfoList
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class UserApprenticeshipInfoList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = true;
      string str = "true!";
      int num = 0;
      XElement node = new XElement((XName) "Result");
      try
      {
        int UserID1 = int.Parse(context.Request["selfid"]);
        int UserID2 = int.Parse(context.Request["RelationshipID"]);
        if (UserID2 == 0)
          UserID2 = UserID1;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          PlayerInfo userSingleByUserId1 = playerBussiness.GetUserSingleByUserID(UserID2);
          PlayerInfo userSingleByUserId2 = playerBussiness.GetUserSingleByUserID(UserID1);
          if (userSingleByUserId1 != null && userSingleByUserId2 != null)
          {
            if (userSingleByUserId2.masterID == userSingleByUserId1.ID)
            {
              XElement apprenticeshipInfo = FlashUtils.CreateUserApprenticeshipInfo(userSingleByUserId1);
              node.Add((object) apprenticeshipInfo);
            }
            foreach (KeyValuePair<int, string> keyValuePair in userSingleByUserId1.MasterOrApprenticesArr)
            {
              PlayerInfo userSingleByUserId3 = playerBussiness.GetUserSingleByUserID(keyValuePair.Key);
              if (userSingleByUserId3 != null && userSingleByUserId3.ID != UserID1)
              {
                XElement apprenticeshipInfo = FlashUtils.CreateUserApprenticeshipInfo(userSingleByUserId3);
                node.Add((object) apprenticeshipInfo);
              }
            }
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        UserApprenticeshipInfoList.log.Error((object) ex);
      }
      node.Add((object) new XAttribute((XName) "total", (object) num));
      node.Add((object) new XAttribute((XName) "value", (object) flag));
      node.Add((object) new XAttribute((XName) "message", (object) str));
      context.Response.ContentType = "text/plain";
      context.Response.Write(node.ToString(false));
    }

    public bool IsReusable => false;
  }
}
