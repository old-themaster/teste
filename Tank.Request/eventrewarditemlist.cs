// Decompiled with JetBrains decompiler
// Type: Tank.Request.eventrewarditemlist
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
  public class eventrewarditemlist : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
        context.Response.Write(eventrewarditemlist.Bulid(context));
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
          Dictionary<int, Dictionary<int, EventRewardInfo>> dictionary1 = new Dictionary<int, Dictionary<int, EventRewardInfo>>();
          EventRewardInfo[] allEventRewardInfo = produceBussiness.GetAllEventRewardInfo();
          EventRewardGoodsInfo[] eventRewardGoods = produceBussiness.GetAllEventRewardGoods();
          foreach (EventRewardInfo eventRewardInfo in allEventRewardInfo)
          {
            eventRewardInfo.AwardLists = new List<EventRewardGoodsInfo>();
            if (!dictionary1.ContainsKey(eventRewardInfo.ActivityType))
              dictionary1.Add(eventRewardInfo.ActivityType, new Dictionary<int, EventRewardInfo>()
              {
                {
                  eventRewardInfo.SubActivityType,
                  eventRewardInfo
                }
              });
            else if (!dictionary1[eventRewardInfo.ActivityType].ContainsKey(eventRewardInfo.SubActivityType))
              dictionary1[eventRewardInfo.ActivityType].Add(eventRewardInfo.SubActivityType, eventRewardInfo);
          }
          foreach (EventRewardGoodsInfo eventRewardGoodsInfo in eventRewardGoods)
          {
            if (dictionary1.ContainsKey(eventRewardGoodsInfo.ActivityType) && dictionary1[eventRewardGoodsInfo.ActivityType].ContainsKey(eventRewardGoodsInfo.SubActivityType))
              dictionary1[eventRewardGoodsInfo.ActivityType][eventRewardGoodsInfo.SubActivityType].AwardLists.Add(eventRewardGoodsInfo);
          }
          XElement xelement1 = (XElement) null;
          foreach (Dictionary<int, EventRewardInfo> dictionary2 in dictionary1.Values)
          {
            foreach (EventRewardInfo eventRewardInfo in dictionary2.Values)
            {
              if (xelement1 == null)
                xelement1 = new XElement((XName) "ActivityType", (object) new XAttribute((XName) "value", (object) eventRewardInfo.ActivityType));
              XElement xelement2 = new XElement((XName) "Items", new object[2]
              {
                (object) new XAttribute((XName) "SubActivityType", (object) eventRewardInfo.SubActivityType),
                (object) new XAttribute((XName) "Condition", (object) eventRewardInfo.Condition)
              });
              foreach (EventRewardGoodsInfo awardList in eventRewardInfo.AwardLists)
              {
                XElement xelement3 = new XElement((XName) "Item", new object[9]
                {
                  (object) new XAttribute((XName) "TemplateId", (object) awardList.TemplateId),
                  (object) new XAttribute((XName) "StrengthLevel", (object) awardList.StrengthLevel),
                  (object) new XAttribute((XName) "AttackCompose", (object) awardList.AttackCompose),
                  (object) new XAttribute((XName) "DefendCompose", (object) awardList.DefendCompose),
                  (object) new XAttribute((XName) "LuckCompose", (object) awardList.LuckCompose),
                  (object) new XAttribute((XName) "AgilityCompose", (object) awardList.AgilityCompose),
                  (object) new XAttribute((XName) "IsBind", (object) awardList.IsBind),
                  (object) new XAttribute((XName) "ValidDate", (object) awardList.ValidDate),
                  (object) new XAttribute((XName) "Count", (object) awardList.Count)
                });
                xelement2.Add((object) xelement3);
              }
              xelement1.Add((object) xelement2);
            }
            result.Add((object) xelement1);
            xelement1 = (XElement) null;
          }
          flag = true;
          str = "Success!";
        }
      }
      catch
      {
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (eventrewarditemlist), true);
    }

    public bool IsReusable => false;
  }
}
