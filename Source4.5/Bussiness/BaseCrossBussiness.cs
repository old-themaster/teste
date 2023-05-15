using log4net;
using SqlDataProvider.BaseClass;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Bussiness
{
  public class BaseCrossBussiness : IDisposable
  {
    protected readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected Sql_DbObject db = new Sql_DbObject("AppConfig", "crosszoneString");
    protected Sql_DbObject db2 = new Sql_DbObject("AppConfig", "conString");

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
        if (this.log.IsErrorEnabled)
          this.log.Error((object) "Init", ex);
      }
      return new DataTable(queryStr);
    }

    public void Dispose()
    {
      this.db.Dispose();
      GC.SuppressFinalize((object) this);
    }
  }
}
