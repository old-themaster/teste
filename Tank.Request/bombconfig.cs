﻿// Decompiled with JetBrains decompiler
// Type: Tank.Request.bombconfig
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class bombconfig : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
        context.Response.Write(bombconfig.Bulid(context));
      else
        context.Response.Write("IP is not valid!");
    }

    public static string Bulid(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        using (ProduceBussiness produceBussiness = new ProduceBussiness())
        {
          foreach (BallConfigInfo b in produceBussiness.GetAllBallConfig())
            result.Add((object) FlashUtils.CreateBallConfigInfo(b));
        }
        flag = true;
        str = "Success!";
      }
      catch (Exception ex)
      {
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (bombconfig), true);
    }

    public bool IsReusable => false;
  }
}
