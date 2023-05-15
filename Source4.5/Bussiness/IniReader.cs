// Decompiled with JetBrains decompiler
// Type: Bussiness.IniReader
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System.Runtime.InteropServices;
using System.Text;

namespace Bussiness
{
  public class IniReader
  {
    private string FilePath;

    public IniReader(string _FilePath) => this.FilePath = _FilePath;

    public string GetIniString(string Section, string Key)
    {
      StringBuilder retVal = new StringBuilder(2550);
      IniReader.GetPrivateProfileString(Section, Key, "", retVal, 2550, this.FilePath);
      return retVal.ToString();
    }

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(
      string section,
      string key,
      string def,
      StringBuilder retVal,
      int size,
      string filePath);
  }
}
