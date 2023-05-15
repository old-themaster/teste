// Decompiled with JetBrains decompiler
// Type: Tank.Request.QuestList
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class QuestList : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
        context.Response.Write(QuestList.Bulid(context));
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
          QuestInfo[] allQuest = produceBussiness.GetALlQuest();
          QuestAwardInfo[] allQuestGoods = produceBussiness.GetAllQuestGoods();
          QuestConditionInfo[] allQuestCondiction = produceBussiness.GetAllQuestCondiction();
          foreach (QuestInfo questInfo1 in allQuest)
          {
            QuestInfo quest = questInfo1;
            XElement questInfo2 = FlashUtils.CreateQuestInfo(quest);
            foreach (QuestConditionInfo info in (IEnumerable) ((IEnumerable<QuestConditionInfo>) allQuestCondiction).Where<QuestConditionInfo>((Func<QuestConditionInfo, bool>) (s => s.QuestID == quest.ID)))
              questInfo2.Add((object) FlashUtils.CreateQuestCondiction(info));
            foreach (QuestAwardInfo info in (IEnumerable) ((IEnumerable<QuestAwardInfo>) allQuestGoods).Where<QuestAwardInfo>((Func<QuestAwardInfo, bool>) (s => s.QuestID == quest.ID)))
              questInfo2.Add((object) FlashUtils.CreateQuestGoods(info));
            result.Add((object) questInfo2);
          }
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        QuestList.log.Error((object) nameof (QuestList), ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (QuestList), true);
    }

    private static void AppendAttribute(XmlDocument doc, XmlNode node, string attr, string value)
    {
      XmlAttribute attribute = doc.CreateAttribute(attr);
      attribute.Value = value;
      node.Attributes.Append(attribute);
    }

    public bool IsReusable => false;
  }
}
