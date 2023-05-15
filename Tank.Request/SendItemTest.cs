// Decompiled with JetBrains decompiler
// Type: Tank.Request.SendItemTest
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
  public class SendItemTest : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      HttpCookie cookie = this.Request.Cookies["userInfo"];
      string s = cookie.Value;
      string str = cookie.Values["bd_sig_user"];
      this.Response.Write(s);
    }
  }
}
