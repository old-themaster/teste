namespace Tank.Request
{
    using Bussiness;
    using Bussiness.CenterService;
    using log4net;
    using Road.Flash;
    using System;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Xml.Linq;

    public class ActivePullDown : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            int userID = Convert.ToInt32(context.Request["selfid"]);
            int activeID = Convert.ToInt32(context.Request["activeID"]);
            string text1 = context.Request["key"];
            string src = context.Request["activeKey"];
            bool flag = false;
            string msg = "ActivePullDownHandler.Fail";
            string awardID = "";
            XElement node = new XElement("Result");
            if (src != "")
            {
                byte[] bytes = CryptoHelper.RsaDecryt2(Tank.Request.StaticFunction.RsaCryptor, src);
                awardID = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            try
            {
                using (ActiveBussiness bussiness = new ActiveBussiness())
                {
                    if (bussiness.PullDown(activeID, awardID, userID, ref msg) == 0)
                    {
                        using (CenterServiceClient client = new CenterServiceClient())
                        {
                            client.MailNotice(userID);
                        }
                    }
                }
                flag = true;
                msg = LanguageMgr.GetTranslation(msg, new object[0]);
            }
            catch (Exception exception)
            {
                log.Error("ActivePullDown", exception);
            }
            node.Add(new XAttribute("value", flag));
            node.Add(new XAttribute("message", msg));
            context.Response.ContentType = "text/plain";
            context.Response.Write(node.ToString(false));
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

