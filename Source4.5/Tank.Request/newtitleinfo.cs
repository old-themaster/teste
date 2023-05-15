
using Bussiness;
using log4net;
using Road.Flash;
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
    public class newtitleinfo : IHttpHandler
    {
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context) => context.Response.Write(newtitleinfo.Bulid(context));

        public static string Bulid(HttpContext context)
        {
            bool flag = false;
            string str = "Fail";
            XElement result = new XElement((XName)"Result");
            try
            {
                using (ProduceBussiness produceBussiness = new ProduceBussiness())
                {
                    foreach (NewTitleInfo info in produceBussiness.GetAllNewTitle())
                        result.Add((object)FlashUtils.CreateNewTitleItems(info));
                }
                flag = true;
                str = "Success!";
            }
            catch (Exception ex)
            {
                newtitleinfo.ilog_0.Error((object)nameof(newtitleinfo), ex);
            }
            result.Add((object)new XAttribute((XName)"value", (object)flag));
            result.Add((object)new XAttribute((XName)"message", (object)str));
            return csFunction.CreateCompressXml(context, result, nameof(newtitleinfo), false);
        }
    }
}
