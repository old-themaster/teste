// Decompiled with JetBrains decompiler
// Type: Bussiness.Interface.SRInterface
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using Bussiness.WebLogin;
using System;

namespace Bussiness.Interface
{
  public class SRInterface : BaseInterface
  {
    public override bool GetUserSex(string name)
    {
      try
      {
        return new PassPortSoapClient().Get_UserSex(string.Empty, name).Value;
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) "获取性别失败", ex);
        return true;
      }
    }
  }
}
