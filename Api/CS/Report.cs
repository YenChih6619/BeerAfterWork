using Dapper;
using System.Data.SQLite;
using System.Data;

namespace Api.CS
{
    public class Report : IDisposable
    {
        private readonly IConfiguration _configuration;
  
        public IDbConnection sqliteConnect()
        {
            return new SQLiteConnection(_configuration.GetConnectionString("sqliteConStt"));
        }

        public IEnumerable<Model.Report.profit.outParams> profit_Query (Model.Report.profit.inParams inParams)
        {
            string ConStr = $@"SELECT a1.* ,a2.QTY_Cost ,a3.Manufacturer_Name,a3.Manufacturer_Origin
                            FROM (
                                SELECT a1.lot_Code, a1.lot_Name, SUM(a1.QTY_Price) AS Total_QTY_Price
                                FROM sys_Cart_travel a1 WHERE (a1.lot_Code = @lot_Code
                            OR @lot_Code IS NULL
                            OR @lot_Code = '')
                            AND (a1.table_Name = @table_Name
                            OR @table_Name IS NULL
                            OR @table_Name = '')
                            AND a1.Date_Time BETWEEN @StartDate AND @EndDate
                                GROUP BY a1.lot_Code, a1.lot_Name
                            ) a1

                            LEFT JOIN wms_Stock a2 on a1.lot_Code  = a2.lot_Code
                            LEFT join sys_Manufacturer a3 on a3.Manufacturer_ID = a2.Manufacturer_ID";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add("@lot_Code", inParams.lot_Code);
            dynamic.Add("@StartDate", inParams.StartDate.ToString("yyyyMMddHHmmssfff"));
            dynamic.Add("@EndDate", inParams.EndDate.ToString("yyyyMMddHHmmssfff"));

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Report.profit.outParams>(ConStr, dynamic).Result;

        }



        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        // ~Report()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
