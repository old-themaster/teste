// Decompiled with JetBrains decompiler
// Type: Bussiness.XmlExtends
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bussiness
{
  public static class XmlExtends
  {
    public static string ToString(this XElement node, bool check)
    {
      StringBuilder output = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        CheckCharacters = check,
        OmitXmlDeclaration = true,
        Indent = true
      };
      using (XmlWriter writer = XmlWriter.Create(output, settings))
        node.WriteTo(writer);
      return output.ToString();
    }
  }
}
