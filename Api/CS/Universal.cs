using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
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

            DynamicParameters dynamic = new();

            dynamic.Add("Manufacturer_ID", inParams.Manufacturer_ID);
            dynamic.Add("Lot_Name", inParams.Lot_Name);
            dynamic.Add("Lot_Code", inParams.Lot_Code);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.wms_Stock.outParams>(ConStr, dynamic).Result;
        }

        public IEnumerable<Model.Universal.wms_Stock.outParams> wms_Stock_Query_Page(Model.Universal.wms_Stock.inParams inParams)
        {
            string ConStr = @"SELECT *
                            FROM wms_Stock
                            WHERE
                                (Manufacturer_ID = :Manufacturer_ID OR :Manufacturer_ID IS NULL OR :Manufacturer_ID = '')
                                AND
                                (Lot_Name = :Lot_Name OR :Lot_Name IS NULL OR :Lot_Name = '')
                                AND
                                (Lot_Code = :Lot_Code OR :Lot_Code IS NULL OR :Lot_Code = '')
                            ORDER BY Manufacturer_ID 
                            LIMIT 10
                            OFFSET (:PageNumber - 1) * 10";

            DynamicParameters dynamic = new();

            dynamic.Add("Manufacturer_ID", inParams.Manufacturer_ID);
            dynamic.Add("Lot_Name", inParams.Lot_Name);
            dynamic.Add("Lot_Code", inParams.Lot_Code);
            dynamic.Add("PageNumber", inParams.PageNumber);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.wms_Stock.outParams>(ConStr, dynamic).Result;
        }

        public int wms_Stock_Query_Count(Model.Universal.wms_Stock.inParams inParams)
        {
            string ConStr = @"SELECT count(*) as c
                            FROM wms_Stock
                            WHERE
                                (Manufacturer_ID = :Manufacturer_ID OR :Manufacturer_ID IS NULL OR :Manufacturer_ID = '')
                                AND
                                (Lot_Name = :Lot_Name OR :Lot_Name IS NULL OR :Lot_Name = '')
                                AND
                                (Lot_Code = :Lot_Code OR :Lot_Code IS NULL OR :Lot_Code = '')";

            DynamicParameters dynamic = new();

            dynamic.Add("Manufacturer_ID", inParams.Manufacturer_ID);
            dynamic.Add("Lot_Name", inParams.Lot_Name);
            dynamic.Add("Lot_Code", inParams.Lot_Code);

            using (var con = sqliteConnect())
                return con.QueryAsync<int>(ConStr, dynamic).Result.First();
        }

        public int wms_Stock_Insert(List<Model.Universal.wms_Stock.outParams> _outParams)
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

            List<DynamicParameters> dynamics = new List<DynamicParameters>();
            _outParams.ForEach(outParams =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@Manufacturer_ID", outParams.Manufacturer_ID);
                dynamic.Add("@Lot_Name", outParams.Lot_Name);
                dynamic.Add("@Lot_Code", outParams.Lot_Code);
                dynamic.Add("@QTY_NORMAL", outParams.QTY_NORMAL);
                dynamic.Add("@QTY_Price", outParams.QTY_Price);
                dynamic.Add("@QTY_Cost", outParams.QTY_Cost);
                dynamic.Add("@UNIT_ID", outParams.UNIT_ID);

                dynamics.Add(dynamic);
            });

            try
            {
                Wms_Stock_Travel_Insert(_outParams);
            }
            catch
            {

            }

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
        }

        public int wms_Stock_delete(List<Model.Universal.wms_Stock.outParams> _outParams)
        {
            string ConStr = @"delete from wms_Stock where Lot_Code = @Lot_Code";
            List<DynamicParameters> dynamics = new List<DynamicParameters>();
            _outParams.ForEach(outParams =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@Lot_Code", outParams.Lot_Code);
                dynamics.Add(dynamic);
            });

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
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

        public int sys_Manufacturer_delete(Model.Universal.sys_Manufacturer.outParams outParams)
        {
            string ConStr = @"delete from sys_Manufacturer where Manufacturer_ID = :Manufacturer_ID";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add(":Manufacturer_ID", outParams.Manufacturer_ID);

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamic).Result;
        }

        public IEnumerable<Model.Universal.sys_Table.outParams> sys_Table_Query(Model.Universal.sys_Table.inParams inParams)
        {
            string ConStr = @"SELECT
                              *
                            FROM sys_Table
                            WHERE table_Name = :table_Name
                            OR :table_Name IS NULL
                            OR :table_Name = ''";

            DynamicParameters dynamic = new();

            dynamic.Add(":table_Name", inParams.table_Name);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.sys_Table.outParams>(ConStr, dynamic).Result;

        }

        public int sys_Table_Insert(List<Model.Universal.sys_Table.outParams> _outParams)
        {
            string ConStr = @"INSERT INTO sys_Table (
                            table_Name,
                            seat_Count,
                            isPrivate_Room,
                            table_Status,
                            memo
                        ) VALUES (
                            @table_Name,
                            @seat_Count,
                            @isPrivate_Room,
                            @table_Status,
                            @memo
                        )
                        ON CONFLICT(table_Name) DO UPDATE SET
                            seat_Count = excluded.seat_Count,
                            isPrivate_Room = excluded.isPrivate_Room,
                            table_Status = excluded.table_Status,
                            memo = excluded.memo";

            List<DynamicParameters> dynamics = new List<DynamicParameters>();
            _outParams.ForEach(outParams =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@table_Name", outParams.table_Name);
                dynamic.Add("@seat_Count", outParams.seat_Count);
                dynamic.Add("@isPrivate_Room", outParams.isPrivate_Room);
                dynamic.Add("@table_Status", outParams.table_Status);
                dynamic.Add("@memo", outParams.memo);

                dynamics.Add(dynamic);
            });

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
        }

        public int sys_Table_delete(List<Model.Universal.sys_Table.outParams> _outParams)
        {
            string ConStr = @"DELETE FROM sys_Table
                            WHERE table_Name = :table_Name";

            List<DynamicParameters> dynamics = new List<DynamicParameters>();
            _outParams.ForEach(outParams =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add(":table_Name", outParams.table_Name);

                dynamics.Add(dynamic);
            });

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
        }

        public IEnumerable<Model.Universal.sys_Cart.outParams> sys_Cart_temp_Query(Model.Universal.sys_Cart.inParams inParams)
        {
            string ConStr = @"SELECT
                              *
                            FROM sys_Cart_temp
                            WHERE table_Name = :table_Name";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add(":table_Name", inParams.table_Name);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.sys_Cart.outParams>(ConStr, dynamic).Result;
        }

        public int sys_Cart_temp_Insert(List<Model.Universal.sys_Cart.outParams> outParams)
        {
            string ConStr = @"INSERT INTO sys_Cart_temp (
                            table_Name,
                            lot_Name,
                            lot_Code,
                            QTY_Price,
                            Count,
                            Discount
                        ) VALUES (
                            @table_Name,
                            @lot_Name,
                            @lot_Code,
                            @QTY_Price,
                            @Count,
                            @Discount
                        )
                        ON CONFLICT(table_Name, lot_Code) DO UPDATE SET
                            lot_Name = excluded.lot_Name,
                            QTY_Price = excluded.QTY_Price,
                            Count = excluded.Count,
                            Discount = excluded.Discount";

            List<DynamicParameters> dynamics = new List<DynamicParameters>();
            outParams.ForEach(outParam =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@table_Name", outParam.table_Name);
                dynamic.Add("@lot_Name", outParam.Lot_Name);
                dynamic.Add("@lot_Code", outParam.Lot_Code);
                dynamic.Add("@QTY_Price", outParam.QTY_Price);
                dynamic.Add("@Count", outParam.Count);
                dynamic.Add("@Discount", outParam.Discount);

                dynamics.Add(dynamic);
            });

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
        }

        public int sys_Cart_temp_delete(Model.Universal.sys_Cart.inParams inParams)
        {
            string ConStr = @"delete from sys_Cart_temp where table_Name = :table_Name";
            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add(":table_Name", inParams.table_Name);

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamic).Result;
        }

        public IEnumerable<Model.Universal.sys_Cart_travel.outParams> sys_Cart_travel_Query(Model.Universal.sys_Cart_travel.inParams inParams)
        {
            string ConStr = @"SELECT
                              *
                            FROM sys_Cart_travel
                            WHERE (lot_Code = @lot_Code
                            OR @lot_Code IS NULL
                            OR @lot_Code = '')
                            AND (table_Name = @table_Name
                            OR @table_Name IS NULL
                            OR @table_Name = '')
                            AND Date_Time BETWEEN @StartDate AND @EndDate";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add("@lot_Code", inParams.lot_Code);
            dynamic.Add("@table_Name", inParams.table_Name);
            dynamic.Add("@StartDate", inParams.StartDate.ToString("yyyyMMddHHmmssfff"));
            dynamic.Add("@EndDate", inParams.EndDate.ToString("yyyyMMddHHmmssfff"));

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.sys_Cart_travel.outParams>(ConStr, dynamic).Result;
        }

        public int sys_Cart_travel_Insert(List<Model.Universal.sys_Cart.outParams> inParams)
        {
            var t = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string ConStr = @"INSERT INTO sys_Cart_travel (
                            table_Name,
                            lot_Name,
                            lot_Code,
                            QTY_Price,
                            Count,
                            Discount,
                            Date_Time
                        ) VALUES (
                            @table_Name,
                            @lot_Name,
                            @lot_Code,
                            @QTY_Price,
                            @Count,
                            @Discount,
                            @Date_Time
                        )";

            List<DynamicParameters> dynamics = new List<DynamicParameters>();
            inParams.ForEach(inParam =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@table_Name", inParam.table_Name);
                dynamic.Add("@lot_Name", inParam.Lot_Name);
                dynamic.Add("@lot_Code", inParam.Lot_Code);
                dynamic.Add("@QTY_Price", inParam.QTY_Price);
                dynamic.Add("@Count", inParam.Count);
                dynamic.Add("@Discount", inParam.Discount);
                dynamic.Add("@Date_Time", t);

                dynamics.Add(dynamic);
            });

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
        }

        public IEnumerable<Model.Universal.Wms_Stock_Travel.outParams> Wms_Stock_Travel_Query(Model.Universal.Wms_Stock_Travel.inParams inParams)
        {
            string ConStr = @"SELECT
                              *
                            FROM wms_Stock_travel
                            WHERE (Manufacturer_ID = @Manufacturer_ID
                            OR @Manufacturer_ID IS NULL
                            OR @Manufacturer_ID = '')
                            AND (Lot_Code = @Lot_Code
                            OR @Lot_Code IS NULL
                            OR @Lot_Code = '')
                            AND Date_Time BETWEEN @StartDate AND @EndDate";

            DynamicParameters dynamic = new DynamicParameters();
            dynamic.Add("@Manufacturer_ID", inParams.Manufacturer_ID);
            dynamic.Add("@Lot_Code", inParams.Lot_Code);
            dynamic.Add("@StartDate", inParams.StartDate);
            dynamic.Add("@EndDate", inParams.EndDate);

            using (var con = sqliteConnect())
                return con.QueryAsync<Model.Universal.Wms_Stock_Travel.outParams>(ConStr, dynamic).Result;
        }

        public int Wms_Stock_Travel_Insert(List<Model.Universal.wms_Stock.outParams> inParams)
        {
            var t = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string ConStr = @"INSERT INTO wms_Stock_travel (
                            Manufacturer_ID,
                            Lot_Name,
                            Lot_Code,
                            QTY_NORMAL,
                            QTY_Price,
                            QTY_Cost,
                            UNIT_ID,
                            Date_Time
                        ) VALUES (
                            @Manufacturer_ID,
                            @Lot_Name,
                            @Lot_Code,
                            @QTY_NORMAL,
                            @QTY_Price,
                            @QTY_Cost,
                            @UNIT_ID,
                            @Date_Time
                        )";

            List<DynamicParameters> dynamics = new List<DynamicParameters>();

            inParams.ForEach(inParam =>
            {
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@Manufacturer_ID", inParam.Manufacturer_ID);
                dynamic.Add("@Lot_Name", inParam.Lot_Name);
                dynamic.Add("@Lot_Code", inParam.Lot_Code);
                dynamic.Add("@QTY_NORMAL", inParam.QTY_NORMAL);
                dynamic.Add("@QTY_Price", inParam.QTY_Price);
                dynamic.Add("@QTY_Cost", inParam.QTY_Cost);
                dynamic.Add("@UNIT_ID", inParam.UNIT_ID);
                dynamic.Add("@Date_Time", t);

                dynamics.Add(dynamic);
            });

            using (var con = sqliteConnect())
                return con.ExecuteAsync(ConStr, dynamics).Result;
        }

        public int checkOut(Model.Universal.outCheck outParams)
        {
            int res = 0;
            foreach (var item in outParams.sys_Cart)
            {
                var stocks = wms_Stock_Query(new Model.Universal.wms_Stock.inParams() { Lot_Code = item.Lot_Code });

                foreach (var stock in stocks)
                {
                    stock.QTY_NORMAL = stock.QTY_NORMAL - item.Count;
                    res += wms_Stock_Insert(new List<Model.Universal.wms_Stock.outParams>() { stock });
                }

                item.table_Name = outParams.sys_Table.table_Name;

                sys_Cart_travel_Insert(new List<Model.Universal.sys_Cart.outParams>() { item });
            }

            sys_Cart_temp_delete(new Model.Universal.sys_Cart.inParams() { table_Name = outParams.sys_Table.table_Name });
            outParams.sys_Table.isPrivate_Room = true;
            sys_Table_Insert(new List<Model.Universal.sys_Table.outParams>() { outParams.sys_Table });

            return res;
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
