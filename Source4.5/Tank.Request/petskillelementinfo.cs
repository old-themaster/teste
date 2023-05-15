// Decompiled with JetBrains decompiler
// Type: Tank.Request.petskillelementinfo
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
  public class petskillelementinfo : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context) => context.Response.Write(petskillelementinfo.Bulid(context));

    public static string Bulid(HttpContext context)
    {
      bool flag = false;
      string str = "Fail!";
      XElement result = new XElement((XName) "Result");
      try
      {
        using (ProduceBussiness produceBussiness = new ProduceBussiness())
        {
          foreach (PetSkillElementInfo info in produceBussiness.GetAllPetSkillElementInfo())
            result.Add((object) FlashUtils.CreatePetSkillElement(info));
          flag = true;
          str = "Success!";
        }
      }
      catch (Exception ex)
      {
        petskillelementinfo.log.Error((object) "Load petskillelementinfo is fail!", ex);
      }
      result.Add((object) new XAttribute((XName) "value", (object) flag));
      result.Add((object) new XAttribute((XName) "message", (object) str));
      return csFunction.CreateCompressXml(context, result, nameof (petskillelementinfo), false);
    }

    public bool IsReusable => false;
  }
}
