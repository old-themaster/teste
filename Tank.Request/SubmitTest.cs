// Decompiled with JetBrains decompiler
// Type: Tank.Request.SubmitTest
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Tank.Request
{
  public class SubmitTest : Page
  {
    protected HtmlForm form1;
    protected TextBox TextBox1;
    protected Button Button1;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e) => this.Response.Redirect("/LoginTest.aspx?name=" + this.TextBox1.Text);
  }
}
