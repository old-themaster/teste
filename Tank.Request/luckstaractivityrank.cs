namespace Tank.Request
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Xml.Linq;
    using System.Collections.Specialized;
    using log4net;
    using System.Reflection;
    using Bussiness;
    using SqlDataProvider.Data;
    using Road.Flash;

    public class luckstaractivityrank : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            bool flag = false;
            string str = "Fail!";
            XElement result = new XElement("Result");
            try
            {
                using (ProduceBussiness bussiness = new ProduceBussiness())
                {
                    LuckstarActivityRankInfo[] info = bussiness.GetAllLuckstarActivityRank();
                    for (int i = 0; i < info.Length; i++)
                    {
                        LuckstarActivityRankInfo rank = info[i];
                        result.Add(FlashUtils.CreateLuckstarActivityRank(rank));
                    }

                    flag = true;
                    str = "Success!";
                }
            }
            catch (Exception exception)
            {
                log.Error("Load DailyAwardList is fail!", exception);
            }
            result.Add(new XAttribute("lastUpdateTime", DateTime.Now.ToString("MM-dd hh:mm")));
            result.Add(new XAttribute("value", flag));
            result.Add(new XAttribute("message", str));
            context.Response.ContentType = "text/plain";
            context.Response.Write(result.ToString(false));
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

