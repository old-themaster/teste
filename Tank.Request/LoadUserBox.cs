// Decompiled with JetBrains decompiler
// Type: Tank.Request.LoadUserBox
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
  public class LoadUserBox : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
        context.Response.Write(LoadUserBox.Bulid(context));
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
          foreach (UserBoxInfo box in produceBussiness.GetAllUserBox())
            result.Add((object) FlashUtils.CreateUserBoxInfo(box));
          flag = true;
          str = "Success!";
        }
      }
      catch
      {
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (LoadUserBox), true);
    }

    public bool IsReusable => false;
  }
}
