// Decompiled with JetBrains decompiler
// Type: Bussiness.BaseBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using log4net;
using SqlDataProvider.BaseClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Bussiness
{
  public class BaseBussiness : IDisposable
  {
    protected Sql_DbObject db = new Sql_DbObject("AppConfig", "conString");
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void Dispose()
    {
      this.db.Dispose();
      GC.SuppressFinalize((object) this);
    }

    public DataTable GetPage(
      string queryStr,
      string queryWhere,
      int pageCurrent,
      int pageSize,
      string fdShow,
      string fdOreder,
      string fdKey,
      ref int total)
    {
      try
      {
        SqlParameter[] SqlParameters = new SqlParameter[8]
        {
          new SqlParameter("@QueryStr", (object) queryStr),
          new SqlParameter("@QueryWhere", (object) queryWhere),
          new SqlParameter("@PageSize", (object) pageSize),
          new SqlParameter("@PageCurrent", (object) pageCurrent),
          new SqlParameter("@FdShow", (object) fdShow),
          new SqlParameter("@FdOrder", (object) fdOreder),
          new SqlParameter("@FdKey", (object) fdKey),
          new SqlParameter("@TotalRow", (object) total)
        };
        SqlParameters[7].Direction = ParameterDirection.Output;
        DataTable dataTable = this.db.GetDataTable(queryStr, "SP_CustomPage", SqlParameters, 120);
        total = (int) SqlParameters[7].Value;
        return dataTable;
      }
      catch (Exception ex)
      {
        if (BaseBussiness.log.IsErrorEnabled)
          BaseBussiness.log.Error((object) "Init", ex);
      }
      return new DataTable(queryStr);
    }
        protected T AutoInit<T>(SqlDataReader reader)
        {
            T val = (T)Activator.CreateInstance(typeof(T));
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (val.GetType().GetProperty(reader.GetName(i)) != null)
                {
                    val.GetType().GetProperty(reader.GetName(i)).SetValue(val, reader.GetValue(i));
                }
            }
            return val;
        }
    }
}
