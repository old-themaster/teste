using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Bussiness;
using SqlDataProvider.Data;
using Road.Flash;
using log4net;
using System.Reflection;
using System.Xml;

namespace Tank.Request
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AchievementList : IHttpHandler
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string Bulid(HttpContext context)
        {
            bool value = false;
            string message = "Fail!";
            XElement result = new XElement("Result");

            try
            {
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    AchievementInfo[] achievementinfo = db.GetAllAchievement();
                    AchievementGoodsInfo[] achievementgoods = db.GetAllAchievementGoods();
                    AchievementCondictionInfo[] achievementcondiction = db.GetAllAchievementCondiction();
                    foreach (AchievementInfo Item in achievementinfo)
                    {
                        XElement temp_xml = FlashUtils.CreateAchievementInfo(Item);
                        IEnumerable temp_achievementcondiction = achievementcondiction.Where(s => s.AchievementID == Item.ID);
                        foreach (AchievementCondictionInfo Item_Condiction in temp_achievementcondiction)
                        {
                            temp_xml.Add(FlashUtils.CreateAchievementCondiction(Item_Condiction));
                        }
                        IEnumerable temp_achievementgoods = achievementgoods.Where(s => s.AchievementID == Item.ID);
                        foreach (AchievementGoodsInfo Item_Reward in temp_achievementgoods)
                        {
                            temp_xml.Add(FlashUtils.CreateAchievementGoods(Item_Reward));
                        }
                        result.Add(temp_xml);
                    }
                    value = true;
                    message = "Success!";
                }
            }
            catch (Exception ex)
            {
                log.Error("achievementlist", ex);
            }

            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
            return csFunction.CreateCompressXml(context, result, "achievementlist", true);
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