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
    public class ApprenticeshipClubList : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool resultValue = true;
            string str = "true!";
            bool flag1 = false;
            bool flag2 = false;
            int total = 0;
            XElement node = new XElement((XName)"Result");
            try
            {
                int page = int.Parse(context.Request["page"]);
                int.Parse(context.Request["selfid"]);
                bool.Parse(context.Request["isReturnSelf"]);
                string nickName = context.Request["name"] == null ? "" : context.Request["name"];
                bool flag3 = bool.Parse(context.Request["appshipStateType"]);
                bool flag4 = bool.Parse(context.Request["requestType"]);
                int size = flag4 ? 9 : 3;
                int where = !flag3 ? 1 : 2;
                int order = !flag3 ? 15 : 17;
                int userID = -1;
                if (!flag4 && !flag3)
                {
                    where = 3;
                    order = 16;
                }
                else if (!flag4 & flag3)
                {
                    where = 4;
                    order = 16;
                }
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    if (nickName != null && nickName.Length > 0)
                    {
                        PlayerInfo singleByNickName = playerBussiness.GetUserSingleByNickName(nickName);
                        userID = singleByNickName == null ? 0 : singleByNickName.ID;
                    }
                    foreach (PlayerInfo info in playerBussiness.GetPlayerPage2(page, size, ref total, order, where, userID, ref resultValue))
                    {
                        XElement apprenticeShipInfo = FlashUtils.CreateApprenticeShipInfo2(info);
                        node.Add((object)apprenticeShipInfo);
                    }
                    resultValue = true;
                    str = "Success!";
                }
            }
            catch (Exception ex)
            {
                ApprenticeshipClubList.log.Error((object)ex);
            }
            node.Add((object)new XAttribute((XName)"total", (object)total));
            node.Add((object)new XAttribute((XName)"value", (object)resultValue));
            node.Add((object)new XAttribute((XName)"message", (object)str));
            node.Add((object)new XAttribute((XName)"isPlayerRegeisted", (object)flag1));
            node.Add((object)new XAttribute((XName)"isSelfPublishEquip", (object)flag2));
            context.Response.ContentType = "text/plain";
            context.Response.Write(node.ToString(false));
        }

        public XElement CreateApprenticeShipInfo2(PlayerInfo info)
        {
            if (info == null)
            {
                ApprenticeshipClubList.log.Warn((object)"Player info is null!");
            }
            else
            {
                foreach (PropertyInfo property in typeof(PlayerInfo).GetProperties())
                {
                    object obj = property.GetValue((object)info, (object[])null);
                    if (obj == null)
                        ApprenticeshipClubList.log.WarnFormat("Property {0} is null!", (object)property.Name);
                    else
                        ApprenticeshipClubList.log.InfoFormat("Property {0} value = {1}", (object)property.Name, obj);
                }
            }
            return new XElement((XName)"Info", new object[37]
            {
        (object) new XAttribute((XName) "UserID", (object) info.ID),
        (object) new XAttribute((XName) "ApplyFor", (object) false),
        (object) new XAttribute((XName) "IsPublishEquip", (object) true),
        (object) new XAttribute((XName) "NickName", (object) info.NickName),
        (object) new XAttribute((XName) "typeVIP", (object) info.typeVIP),
        (object) new XAttribute((XName) "VIPLevel", (object) info.VIPLevel),
        (object) new XAttribute((XName) "IsConsortia", (object) info.IsConsortia),
        (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
        (object) new XAttribute((XName) "Sex", (object) info.Sex),
        (object) new XAttribute((XName) "Win", (object) info.Win),
        (object) new XAttribute((XName) "Total", (object) info.Total),
        (object) new XAttribute((XName) "Escape", (object) info.Escape),
        (object) new XAttribute((XName) "GP", (object) info.GP),
        (object) new XAttribute((XName) "Honor", (object) info.Honor),
        (object) new XAttribute((XName) "Style", (object) info.Style),
        (object) new XAttribute((XName) "Colors", (object) info.Colors),
        (object) new XAttribute((XName) "Hide", (object) info.Hide),
        (object) new XAttribute((XName) "Grade", (object) info.Grade),
        (object) new XAttribute((XName) "State", (object) info.State),
        (object) new XAttribute((XName) "Repute", (object) info.Repute),
        (object) new XAttribute((XName) "Skin", (object) info.Skin),
        (object) new XAttribute((XName) "Offer", (object) info.Offer),
        (object) new XAttribute((XName) "IsMarried", (object) info.IsMarried),
        (object) new XAttribute((XName) "ConsortiaName", (object) info.ConsortiaName),
        (object) new XAttribute((XName) "DutyName", (object) info.DutyName),
        (object) new XAttribute((XName) "Nimbus", (object) info.Nimbus),
        (object) new XAttribute((XName) "FightPower", (object) info.FightPower),
        (object) new XAttribute((XName) "AchievementPoint", (object) info.AchievementPoint),
        (object) new XAttribute((XName) "Rank", (object) info.Honor),
        (object) new XAttribute((XName) "ApprenticeshipState", (object) info.apprenticeshipState),
        (object) new XAttribute((XName) "GraduatesCount", (object) info.graduatesCount),
        (object) new XAttribute((XName) "HonourOfMaster", (object) info.honourOfMaster),
        (object) new XAttribute((XName) "SpouseID", (object) info.SpouseID),
        (object) new XAttribute((XName) "SpouseName", (object) info.SpouseName),
        (object) new XAttribute((XName) "BadgeID", (object) info.badgeID),
        (object) new XAttribute((XName) "BadgeBuyTime", (object) DateTime.Now.ToString()),
        (object) new XAttribute((XName) "ValidDate", (object) 0)
            });
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
