// Decompiled with JetBrains decompiler
// Type: Tank.Request.KeyGenerator
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class KeyGenerator : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      new CspParameters().Flags = CspProviderFlags.UseMachineKeyStore;
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(2048);
      RSAParameters rsaParameters = cryptoServiceProvider.ExportParameters(true);
      StringBuilder stringBuilder1 = new StringBuilder();
      for (int index = 0; index < rsaParameters.Modulus.Length; ++index)
        stringBuilder1.Append(rsaParameters.Modulus[index].ToString("X2"));
      StringBuilder stringBuilder2 = new StringBuilder();
      for (int index = 0; index < rsaParameters.Exponent.Length; ++index)
        stringBuilder2.Append(rsaParameters.Exponent[index].ToString("X2"));
      XElement xelement1 = new XElement((XName) "list");
      XElement xelement2 = new XElement((XName) "private", (object) new XAttribute((XName) "key", (object) cryptoServiceProvider.ToXmlString(true)));
      XElement xelement3 = new XElement((XName) "public", new object[2]
      {
        (object) new XAttribute((XName) "model", (object) stringBuilder1.ToString()),
        (object) new XAttribute((XName) "exponent", (object) stringBuilder2.ToString())
      });
      xelement1.Add((object) xelement2);
      xelement1.Add((object) xelement3);
      context.Response.Write(xelement1.ToString());
    }

    public bool IsReusable => false;
  }
}
