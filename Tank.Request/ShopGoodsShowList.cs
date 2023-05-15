// Decompiled with JetBrains decompiler
// Type: Tank.Request.ShopGoodsShowList
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
  public class ShopGoodsShowList : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
        context.Response.Write(ShopGoodsShowList.Bulid(context));
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
          XElement xelement = new XElement((XName) "Store");
          foreach (ShopGoodsShowListInfo allShopGoodsShow in produceBussiness.GetAllShopGoodsShowList())
            xelement.Add((object) FlashUtils.CreateShopShowInfo(allShopGoodsShow));
          result.Add((object) xelement);
          flag = true;
          str = "Success!";
        }
      }
      catch
      {
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (ShopGoodsShowList), true);
    }

    public bool IsReusable => false;
  }
}
