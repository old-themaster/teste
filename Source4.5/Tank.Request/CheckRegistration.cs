// Decompiled with JetBrains decompiler
// Type: Tank.Request.CheckRegistration
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using log4net;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;
using zlib;

namespace Tank.Request
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class CheckRegistration : IHttpHandler, IRequiresSessionState
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
      bool flag = true;
      string str = "Registered!";
      XElement xelement = new XElement((XName) "Result");
      int num = 1;
      xelement.Add((object) new XAttribute((XName) "value", (object) flag));
      xelement.Add((object) new XAttribute((XName) "message", (object) str));
      xelement.Add((object) new XAttribute((XName) "status", (object) num));
      context.Response.ContentType = "text/plain";
      context.Response.BinaryWrite(StaticFunction.Compress(xelement.ToString()));
    }

    public static byte[] Compress(byte[] data)
    {
      MemoryStream memoryStream = new MemoryStream();
      ZOutputStream zoutputStream = new ZOutputStream((Stream) memoryStream, 3);
      zoutputStream.Write(data, 0, data.Length);
      zoutputStream.Flush();
      zoutputStream.Close();
      return memoryStream.ToArray();
    }

    public bool IsReusable => false;
  }
}
