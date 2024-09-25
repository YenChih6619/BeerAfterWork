namespace Api.Model
{
    public static partial class Universal
    {
        public static string mySqlConStr = string.Empty;
        public static string sqliteConStr = string.Empty;
        public class unit
        {
            public class inParams
            {
                public int unit_id { get; set; }
                public string unit_no { get; set; }
            }

            public class outParams
            {
                public int unit_id { get; set; }
                public string unit_desc { get; set; }
                public string unit_no { get; set; }
            }
        }

        public class sys_Manufacturer
        {
            public class inParams
            {
                /// <summary>
                /// 製造商名稱
                /// </summary>
                public string Manufacturer_Name { set; get; }

                /// <summary>
                /// 酒廠產地
                /// </summary>
                public string Manufacturer_Origin { set; get; }

            }

            public class outParams
            {
                /// <summary>
                /// 製造商 ID
                /// </summary>
                public string Manufacturer_ID { set; get; }

                /// <summary>
                /// 製造商名稱
                /// </summary>
                public string Manufacturer_Name { set; get; }

                /// <summary>
                /// 製造商簡介
                /// </summary>
                public string Manufacturer_Desc { set; get; }

                /// <summary>
                /// 酒廠產地
                /// </summary>
                public string Manufacturer_Origin { set; get; }

                /// <summary>
                /// 酒廠連絡電話
                /// </summary>
                public string Manufacturer_Tel { set; get; }
            }
        }

        public class wms_Stock
        {
            public class inParams
            {
                /// <summary>
                /// 分頁數
                /// </summary>
                public int PageNumber { set; get; } = 1;

                /// <summary>
                /// 製造商 ID
                /// </summary>
                public string Manufacturer_ID { set; get; }

                /// <summary>
                /// 產品名稱
                /// </summary>
                public string Lot_Name { set; get; }

                /// <summary>
                /// 標籤編號
                /// </summary>
                public string Lot_Code { set; get; }
            }

            public class outParams
            {
                /// <summary>
                /// 製造商 ID
                /// </summary>
                public string Manufacturer_ID { set; get; }

                /// <summary>
                /// 產品名稱
                /// </summary>
                public string Lot_Name { set; get; }

                /// <summary>
                /// 標籤編號
                /// </summary>
                public string Lot_Code { set; get; }

                /// <summary>
                /// 庫存數量
                /// </summary>
                public decimal QTY_NORMAL { set; get; }

                /// <summary>
                /// 單位價格
                /// </summary>
                public decimal QTY_Price { set; get; }

                /// <summary>
                /// 單位成本
                /// </summary>
                public decimal QTY_Cost { set; get; }

                /// <summary>
                /// 單位 ID
                /// </summary>
                public int UNIT_ID { set; get; }

            }
        }

        public class Wms_Stock_Travel
        {
            public class inParams
            {
                public string Manufacturer_ID { get; set; }
                public string Lot_Code { get; set; }
                public DateTime StartDate { set; get; }
                public DateTime EndDate { set; get; }
            }

            public class outParams
            {
                public string Manufacturer_ID { get; set; }
                public string Lot_Name { get; set; }
                public string Lot_Code { get; set; }
                public decimal? QTY_NORMAL { get; set; }
                public decimal? QTY_Price { get; set; }
                public decimal? QTY_Cost { get; set; }
                public string UNIT_ID { get; set; }
                public string Date_Time { get; set; }
            }
        }

        public class sys_Product
        {
            public class inParams
            {

            }

            public class outParams
            {
                /// <summary>
                /// 製造商名稱
                /// </summary>
                public string manufacturer_Name { set; get; }

                /// <summary>
                /// 酒廠產地
                /// </summary>
                public string manufacturer_Origin { set; get; }

                /// <summary>
                /// 產品名稱
                /// </summary>
                public string lot_Name { set; get; }

                /// <summary>
                /// 標籤編號
                /// </summary>
                public string lot_Code { set; get; }

                /// <summary>
                /// 庫存數量
                /// </summary>
                public decimal Qty_Noraml { set; get; }

                /// <summary>
                /// 單位價格
                /// </summary>
                public decimal Qty_Price { set; get; }

                public string Unit_No { set; get; }

                public int Count { set; get; }
            }
        }

        public class sys_Table
        {
            public class inParams
            {
                /// <summary>
                /// 餐桌名稱，作為主鍵使用。
                /// </summary>
                public string table_Name { get; set; }

             
            }

            public class outParams
            {
                /// <summary>
                /// 餐桌名稱，作為主鍵使用。
                /// </summary>
                public string table_Name { get; set; }

                /// <summary>
                /// 餐桌的座位數量。
                /// </summary>
                public int seat_Count { get; set; }

                /// <summary>
                /// 指示是否結帳。若為已結帳則為 true，否則為 false。
                /// </summary>
                public bool isPrivate_Room { get; set; }

                /// <summary>
                /// 餐桌的狀態。
                /// </summary>
                public int table_Status { get; set; }

                /// <summary>
                /// 餐桌的備註信息。
                /// </summary>
                public string memo { get; set; }
            }      
        }

        public class sys_Cart
        {
            public class inParams
            {
                /// <summary>
                /// 桌號
                /// </summary>
                public string table_Name { set; get; }
            }

            public class outParams
            {

                /// <summary>
                /// 桌號
                /// </summary>
                public string table_Name { set; get; }

                /// <summary>
                /// 產品名稱
                /// </summary>
                public string Lot_Name { set; get; }

                /// <summary>
                /// 標籤編號
                /// </summary>
                public string Lot_Code { set; get; }

                /// <summary>
                /// 單位價格
                /// </summary>
                public decimal QTY_Price { set; get; }

                /// <summary>
                /// 消費數量
                /// </summary>
                public decimal Count { set; get; }

                /// <summary>
                /// 折扣數 <br></br>
                /// 預設:100 <br></br>
                /// 九折的話就是 90
                /// </summary>
                public int Discount { set; get; } = 100;

                /// <summary>
                /// 消費金額
                /// </summary>
                public decimal Amount
                {
                    get
                    {
                        if (this.Count == 0) { return 0; }
                        if (this.Discount == 0) { return 0; }
                        return QTY_Price * this.Count * (this.Discount / 100);
                    }
                }


            }
        }

        public class sys_Cart_travel
        {
            public class inParams
            {
                public string table_Name { get; set; }
                public string lot_Code { get; set; }
                public DateTime StartDate { set; get; }
                public DateTime EndDate { set; get; }
            }

            public class outParams
            {
                public string table_Name { get; set; }
                public string lot_Name { get; set; }
                public string lot_Code { get; set; }
                public decimal? QTY_Price { get; set; }
                public decimal? Count { get; set; }
                public decimal? Discount { get; set; }
                public string Date_Time { get; set; }
            }
        }

        public class outCheck
        {
            public Model.Universal.sys_Table.outParams sys_Table { set; get; }

            public List<Model.Universal.sys_Cart.outParams> sys_Cart { set; get; }
        }
    }
}
