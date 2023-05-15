// Decompiled with JetBrains decompiler
// Type: Road.Flash.CryptoHelper
// Assembly: Road.Flash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D8D53EE5-2A13-4D9A-AC36-4B7E6FABCB36
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Road.Flash.dll

using System;
using System.Security.Cryptography;
using System.Text;

namespace Road.Flash
{
  public class CryptoHelper
  {
    public static RSACryptoServiceProvider GetRSACrypto(string privateKey)
    {
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(new CspParameters()
      {
        Flags = CspProviderFlags.UseMachineKeyStore
      });
      cryptoServiceProvider.FromXmlString(privateKey);
      return cryptoServiceProvider;
    }

    public static string RsaDecrypt(string privateKey, string src)
    {
      RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(new CspParameters()
      {
        Flags = CspProviderFlags.UseMachineKeyStore
      });
      rsa.FromXmlString(privateKey);
      return CryptoHelper.RsaDecrypt(rsa, src);
    }

    public static string RsaDecrypt(RSACryptoServiceProvider rsa, string src)
    {
      byte[] rgb = Convert.FromBase64String(src);
      return Encoding.UTF8.GetString(rsa.Decrypt(rgb, false));
    }

    public static byte[] RsaDecryt2(RSACryptoServiceProvider rsa, string src)
    {
      byte[] rgb = Convert.FromBase64String(src);
      return rsa.Decrypt(rgb, false);
    }
  }
}
