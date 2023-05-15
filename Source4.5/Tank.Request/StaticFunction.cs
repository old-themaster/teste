// Decompiled with JetBrains decompiler
// Type: Tank.Request.StaticFunction
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Road.Flash;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using zlib;

namespace Tank.Request
{
  public class StaticFunction
  {
    public static RSACryptoServiceProvider RsaCryptor => CryptoHelper.GetRSACrypto(ConfigurationManager.AppSettings["privateKey"]);

    public static byte[] Compress(string str) => StaticFunction.Compress(Encoding.UTF8.GetBytes(str));

    public static byte[] Compress(byte[] src) => StaticFunction.Compress(src, 0, src.Length);

    public static byte[] Compress(byte[] src, int offset, int length)
    {
      MemoryStream memoryStream = new MemoryStream();
      ZOutputStream zoutputStream = new ZOutputStream((Stream) memoryStream, 9);
      zoutputStream.Write(src, offset, length);
      zoutputStream.Close();
      return memoryStream.ToArray();
    }

    public static string Uncompress(string str) => Encoding.UTF8.GetString(StaticFunction.Uncompress(Encoding.UTF8.GetBytes(str)));

    public static byte[] Uncompress(byte[] src)
    {
      MemoryStream memoryStream = new MemoryStream();
      ZOutputStream zoutputStream = new ZOutputStream((Stream) memoryStream);
      zoutputStream.Write(src, 0, src.Length);
      zoutputStream.Close();
      return memoryStream.ToArray();
    }
  }
}
