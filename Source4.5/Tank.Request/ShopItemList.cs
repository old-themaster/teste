﻿namespace Tank.Request
{
    using Bussiness;
    using Road.Flash;
    using SqlDataProvider.Data;
    using System;
    using System.Web;
    using System.Web.Services;
    using System.Xml.Linq;

    [WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ShopItemList : IHttpHandler
    {
        public static string Bulid(HttpContext context)
        {
            bool flag = false;
            string str = "Fail!";
            XElement result = new XElement("Result");
            try
            {
                using (ProduceBussiness bussiness = new ProduceBussiness())
                {
                    XElement content = new XElement("Store");
                    ShopItemInfo[] info = bussiness.GetALllShop();
                    for (int i = 0; i < info.Length; i++)
                    {
                        ShopItemInfo shop = info[i];
                        content.Add(FlashUtils.CreateShopInfo(shop));
                    }
                    result.Add(content);
                    flag = true;
                    str = "Success!";
                }
            }
            catch
            {
            }
            result.Add(new XAttribute("value", flag));
            result.Add(new XAttribute("message", str));
            return csFunction.CreateCompressXml(context, result, "ShopItemList", true);
        }

        public void ProcessRequest(HttpContext context)
        {
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                context.Response.Write(Bulid(context));
            }
            else
            {
                context.Response.Write("IP is not valid!");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

