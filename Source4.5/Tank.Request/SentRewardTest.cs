// Decompiled with JetBrains decompiler
// Type: Tank.Request.SentRewardTest
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness.Interface;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
  public class SentRewardTest : Page
  {
    protected HtmlForm form1;

    protected void Page_Load(object sender, EventArgs e)
    {
      string str1 = "大幅度是";
      string str2 = "大幅度是";
      string str3 = "watson";
      string str4 = "6666";
      string str5 = "99999";
      string str6 = "11020,4,0,0,0,0,0,0,1|7014,2,9,400,400,400,400,400,0";
      string str7 = str1 + "#" + str2 + "#" + str3 + "#" + str4 + "#" + str5 + "#" + str6 + "#";
      DateTime now = DateTime.Now;
      string str8 = "asdfgh";
      string str9 = BaseInterface.md5(str3 + str4 + str5 + str6 + (object) BaseInterface.ConvertDateTimeInt(now) + str8);
      this.Response.Redirect("http://192.168.0.4:828/SentReward.ashx?content=" + this.Server.UrlEncode(str7 + (object) BaseInterface.ConvertDateTimeInt(now) + "#" + str9));
    }
  }
}
