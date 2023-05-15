// Decompiled with JetBrains decompiler
// Type: Tank.Request.WebsResponse
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using System;
using System.IO;
using System.Net;
using System.Text;

namespace Tank.Request
{
  public class WebsResponse
  {
    public static string GetPage(string url, string postData, string encodeType, out string err)
    {
      Encoding encoding = Encoding.GetEncoding(encodeType);
      byte[] bytes = encoding.GetBytes(postData);
      try
      {
        HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
        httpWebRequest.CookieContainer = new CookieContainer();
        httpWebRequest.AllowAutoRedirect = true;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        string end = new StreamReader((httpWebRequest.GetResponse() as HttpWebResponse).GetResponseStream(), encoding).ReadToEnd();
        err = string.Empty;
        return end;
      }
      catch (Exception ex)
      {
        err = ex.Message;
        return string.Empty;
      }
    }
  }
}
