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
    }
}
