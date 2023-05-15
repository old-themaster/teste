// Decompiled with JetBrains decompiler
// Type: Tank.Request.GMActivityInfo
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7437BD41-3E33-4D33-8CF3-0A4B4906B009
// Assembly location: C:\Users\k4p3t\Downloads\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GMActivityInfo : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            XElement result = new XElement((XName)"Result");
            string str1 = "Fail!";
            string str2 = "false";
            try
            {
                using (ProduceBussiness produceBussiness = new ProduceBussiness())
                {
                   // GmActivityInfo[] allGmActivity = produceBussiness.GetAllGmActivity();
                   // GmGiftInfo[] allGmGift = produceBussiness.GetAllGmGift();
                   // GmActiveConditionInfo[] gmActiveCondition = produceBussiness.GetAllGmActiveCondition();
                   // GmActiveRewardInfo[] allGmActiveReward = produceBussiness.GetAllGmActiveReward();
                    foreach (GmActivityInfo gmActivityInfo1 in allGmActivity)
                    {
                        GmActivityInfo activityInfo = gmActivityInfo1;
                        XElement content1 = new XElement((XName)"ActiveInfo");
                        XElement gmActivityInfo2 = FlashUtils.CreateGMActivityInfo(activityInfo);
                        XElement content2 = new XElement((XName)"ActiveGiftBag");
                        foreach (GmGiftInfo gmGiftInfo1 in ((IEnumerable<GmGiftInfo>)allGmGift).Where<GmGiftInfo>((Func<GmGiftInfo, bool>)(s => s.activityId == activityInfo.activityId)))
                        {
                            GmGiftInfo gmGiftInfo = gmGiftInfo1;
                            XElement gmGiftInfo2 = FlashUtils.CreateGMGiftInfo(gmGiftInfo);
                            XElement content3 = new XElement((XName)"ActiveCondition");
                            XElement content4 = new XElement((XName)"ActiveReward");
                            bool flag1 = false;
                            bool flag2 = false;
                            foreach (GmActiveConditionInfo gmConditionInfo in ((IEnumerable<GmActiveConditionInfo>)gmActiveCondition).Where<GmActiveConditionInfo>((Func<GmActiveConditionInfo, bool>)(s => s.giftbagId == gmGiftInfo.giftbagId)))
                            {
                                content3.Add((object)FlashUtils.CreateGMConditionInfo(gmConditionInfo));
                                if (!flag2)
                                    flag2 = true;
                            }
                            foreach (GmActiveRewardInfo gmRewardInfo in ((IEnumerable<GmActiveRewardInfo>)allGmActiveReward).Where<GmActiveRewardInfo>((Func<GmActiveRewardInfo, bool>)(s => s.giftId == gmGiftInfo.giftbagId)))
                            {
                                content4.Add((object)FlashUtils.CreateGMRewardInfo(gmRewardInfo));
                                if (!flag1)
                                    flag1 = true;
                            }
                            content2.Add((object)gmGiftInfo2);
                            if (flag2)
                                content2.Add((object)content3);
                            if (flag1)
                                content2.Add((object)content4);
                        }
                        content1.Add((object)gmActivityInfo2);
                        content1.Add((object)content2);
                        result.Add((object)content1);
                    }
                    str1 = "Success!";
                    str2 = "true";
                }
            }
            catch (Exception ex)
            {
                if (!GMActivityInfo.Log.IsErrorEnabled)
                    return;
                GMActivityInfo.Log.Error((object)"GMActivityInfo create error", ex);
            }
            finally
            {
                result.Add((object)new XAttribute((XName)"value", (object)str2));
                result.Add((object)new XAttribute((XName)"message", (object)str1));
                context.Response.Write(csFunction.CreateCompressXml(context, result, nameof(GMActivityInfo), true));
            }
        }
    }
}
