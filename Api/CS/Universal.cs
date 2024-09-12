using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SQLite;
namespace Api.CS
{
    public class Universal : IDisposable
    {
        public IDbConnection sqliteConnect()
        {
            return new SQLiteConnection(Model.Universal.sqliteConStr);
        }

        public IEnumerable<Model.Universal.unit.outParams> sys_unit_Query(Model.Universal.unit.inParams inParams)
        {
            string ConStr = $@"SELECT 
                                *
                            FROM
                                sys_unit
                            WHERE
                                1 = 1 
                                    {(!string.IsNullOrEmpty(inParams.unit_no) ? " AND unit_no = :unit_no" : string.Empty)}
                                    {((inParams.unit_id) != 0 ? " AND unit_id = :unit_id" : string.Empty)}";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add(":unit_id", inParams.unit_id);
            dynamic.Add(":unit_no", inParams.unit_no);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.unit.outParams>(ConStr, dynamic).Result;

        }

        public IEnumerable<Model.Universal.sys_Manufacturer.outParams> sys_Manufacturer_Query(Model.Universal.sys_Manufacturer.inParams inParams)
        {
            string ConStr = @"SELECT * 
                            FROM sys_Manufacturer
                            WHERE 
                                (Manufacturer_Name = :Manufacturer_Name OR :Manufacturer_Name IS NULL OR :Manufacturer_Name = '') 
                                AND 
                                (Manufacturer_Origin = :Manufacturer_Origin OR :Manufacturer_Origin IS NULL OR :Manufacturer_Origin = '')";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add(":Manufacturer_Name", inParams.Manufacturer_Name);
            dynamic.Add(":Manufacturer_Origin", inParams.Manufacturer_Origin);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.sys_Manufacturer.outParams>(ConStr, dynamic).Result;

        }

        public IEnumerable<Model.Universal.wms_Stock.outParams> wms_Stock_Query(Model.Universal.wms_Stock.inParams inParams)
        {
            string ConStr = @"SELECT *
                            FROM wms_Stock
                            WHERE
                                (Manufacturer_ID = :Manufacturer_ID OR :Manufacturer_ID IS NULL OR :Manufacturer_ID = '')
                                AND
                                (Lot_Name = :Lot_Name OR :Lot_Name IS NULL OR :Lot_Name = '')
                                AND
                                (Lot_Code = :Lot_Code OR :Lot_Code IS NULL OR :Lot_Code = '')";

            DynamicParameters dynamic = new ();

            dynamic.Add("Manufacturer_ID", inParams.Manufacturer_ID);
            dynamic.Add("Lot_Name", inParams.Lot_Name);
            dynamic.Add("Lot_Code", inParams.Lot_Code);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.wms_Stock.outParams>(ConStr, dynamic).Result;
        }

        public int wms_Stock_Insert(Model.Universal.wms_Stock.outParams outParams)
        {
            string ConStr = @"INSERT INTO wms_Stock (
                            Manufacturer_ID,
                            Lot_Name,
                            Lot_Code,
                            QTY_NORMAL,
                            QTY_Price,
                            QTY_Cost,
                            UNIT_ID
                        ) VALUES (
                            @Manufacturer_ID,
                            @Lot_Name,
                            @Lot_Code,
                            @QTY_NORMAL,
                            @QTY_Price,
                            @QTY_Cost,
                            @UNIT_ID
                        )
                        ON CONFLICT(Lot_Code) DO UPDATE SET
                            Lot_Name = excluded.Lot_Name,
                            QTY_NORMAL = excluded.QTY_NORMAL,
                            QTY_Price = excluded.QTY_Price,
                            QTY_Cost = excluded.QTY_Cost,
                            UNIT_ID = excluded.UNIT_ID";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add("@Manufacturer_ID", outParams.Manufacturer_ID);
            dynamic.Add("@Lot_Name", outParams.Lot_Name);
            dynamic.Add("@Lot_Code", outParams.Lot_Code);
            dynamic.Add("@QTY_NORMAL", outParams.QTY_NORMAL);
            dynamic.Add("@QTY_Price", outParams.QTY_Price);
            dynamic.Add("@QTY_Cost", outParams.QTY_Cost);
            dynamic.Add("@UNIT_ID", outParams.UNIT_ID);

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamic).Result;
        }

        public int sys_Manufacturer_Insert(Model.Universal.sys_Manufacturer.outParams outParams)
        {
            string ConStr = @"INSERT INTO sys_Manufacturer 
                            (Manufacturer_Name, Manufacturer_Desc, Manufacturer_Origin, Manufacturer_Tel) 
                        VALUES 
                            (@Manufacturer_Name, @Manufacturer_Desc, @Manufacturer_Origin, @Manufacturer_Tel)
                        ON CONFLICT(Manufacturer_Name) DO UPDATE SET 
                            Manufacturer_Desc = excluded.Manufacturer_Desc,
                            Manufacturer_Origin = excluded.Manufacturer_Origin,
                            Manufacturer_Tel = excluded.Manufacturer_Tel";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add("@Manufacturer_Name", outParams.Manufacturer_Name);
            dynamic.Add("@Manufacturer_Desc", outParams.Manufacturer_Desc);
            dynamic.Add("@Manufacturer_Origin", outParams.Manufacturer_Origin);
            dynamic.Add("@Manufacturer_Tel", outParams.Manufacturer_Tel);

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamic).Result;
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
        // ~Universal()
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
