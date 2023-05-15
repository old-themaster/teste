// Decompiled with JetBrains decompiler
// Type: Tank.Request.GetSID
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.SessionState;
using System.Xml.Linq;

namespace Tank.Request
{
  public class GetSID : IHttpHandler, IRequiresSessionState
  {
    public void ProcessRequest(HttpContext context)
    {
      new CspParameters().Flags = CspProviderFlags.UseMachineKeyStore;
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(2048);
      cryptoServiceProvider.FromXmlString(ConfigurationManager.AppSettings["privateKey"]);
      RSAParameters rsaParameters = cryptoServiceProvider.ExportParameters(false);
      XElement xelement = new XElement((XName) "result", new object[2]
      {
        (object) new XAttribute((XName) "m1", (object) Convert.ToBase64String(rsaParameters.Modulus)),
        (object) new XAttribute((XName) "m2", (object) Convert.ToBase64String(rsaParameters.Exponent))
      });
      context.Response.ContentType = "text/plain";
      context.Response.Write(xelement.ToString());
    }

    public bool IsReusable => false;
  }
}
