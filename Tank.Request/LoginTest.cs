// Decompiled with JetBrains decompiler
// Type: Tank.Request.LoginTest
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness.Interface;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
  public class LoginTest : Page
  {
    protected HtmlForm form1;

    protected void Page_Load(object sender, EventArgs e)
    {
      string str1 = "onelife";
      string str2 = "733789";
      int num = 1255165271;
      string str3 = "yk-MotL-qhpAo88-7road-mtl55dantang-login-logddt777";
      string str4 = BaseInterface.md5(str1 + str2 + num.ToString() + str3);
      string str5 = "content=" + HttpUtility.UrlEncode(str1 + "|" + str2 + "|" + num.ToString() + "|" + str4);
      this.Response.Write(BaseInterface.RequestContent("http://localhost:728/CreateLogin.aspx?content=" + HttpUtility.UrlEncode(str1 + "|" + str2 + "|" + num.ToString() + "|" + str4)));
    }
  }
}
