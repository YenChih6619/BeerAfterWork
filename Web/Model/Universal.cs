using BootstrapBlazor.Components;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using static Web.Model.Universal;

namespace Web.Model
{
    public class Call : IDisposable
    {
        string _url, _jsonData;
        HttpMethod _methodType;
        private bool disposedValue;

        public Call(string url, string jsonData, HttpMethod methodType)
        {
            this._url = url;
            this._jsonData = jsonData;
            this._methodType = methodType;
        }

        public string CallAPI()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30); // 設定合理的超時值

                    var requestMessage = new HttpRequestMessage(_methodType, _url)
                    {
                        Content = new StringContent(_jsonData, Encoding.UTF8, "application/json")
                    };

                    var response = httpClient.Send(requestMessage);

                    response.EnsureSuccessStatusCode(); // 確保 HTTP 響應狀態碼是成功的（200-299）

                    string result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                // 日誌記錄錯誤或處理異常
                return $"HTTP Request error: {httpRequestException.Message}";
            }
            catch (Exception ex)
            {
                // 日誌記錄錯誤或處理異常
                return $"General error: {ex.Message}";
            }
        }

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
        // ~Call()
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
    public static class Universal
    {
        public static string ExeUrl { get; set; } = @"http://localhost:5000/";

        public class SYS_Cart
        {
            public class inParams
            {

            }

            public class outParams
            {
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

        public class sys_ProductList
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

        public class SYS_Unit
        {
            public class inParams
            {

            }

            public class outParams
            {
                public int UNIT_ID { set; get; }

                public string UNIT_NO { set; get; }

                public string UNIT_DESC { set; get; }

            }
        }

        public class SYS_Flavor
        {
            public class inParams
            {

            }

            public class outParams
            {
                public int Flavor_ID { set; get; }

                public string Flavor_Name { set; get; }

            }
        }

        public class sys_Manufacturer
        {
            public class inParams
            {
                /// <summary>
                /// 製造商名稱
                /// </summary>
                [Display(Name = "製造商名稱")]
                public string Manufacturer_Name { set; get; }

                /// <summary>
                /// 酒廠產地
                /// </summary>
                [Display(Name = "酒廠產地")]
                public string Manufacturer_Origin { set; get; }
            }

            public class outParams
            {
                /// <summary>
                /// 製造商 ID
                /// </summary>
                [Display(Name = "製造商 ID")]
                [ReadOnly(true)]
                public string Manufacturer_ID { set; get; }

                /// <summary>
                /// 製造商名稱
                /// </summary>
                [Display(Name = "製造商名稱")]
                public string Manufacturer_Name { set; get; }

                /// <summary>
                /// 製造商簡介
                /// </summary>
                [Display(Name = "製造商簡介")]
                public string Manufacturer_Desc { set; get; }

                /// <summary>
                /// 酒廠產地
                /// </summary>
                [Display(Name = "酒廠產地")]
                public string Manufacturer_Origin { set; get; }

                /// <summary>
                /// 酒廠連絡電話
                /// </summary>
                [Display(Name = "酒廠連絡電話")]
                public string Manufacturer_Tel { set; get; }
            }
        }

        public class Wms_Stock
        {
            public class inParams
            {

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
            /// <summary>
            /// 歷程
            /// </summary>
            /// <value>
            /// Format: yyyyMMddHHmmssfff
            /// </value>
            public string Travel_id { set; get; }

            /// <summary>
            /// 製造商 ID
            /// </summary>
            public string Manufacturer_ID { set; get; }


        }

    }

    public static class Data
    {
        public static List<sys_Manufacturer.outParams> _Manufacturer = new List<sys_Manufacturer.outParams>()
        {
           new ()
           {
               Manufacturer_ID ="10001",
               Manufacturer_Desc = "",
               Manufacturer_Name = "臺虎精釀",
               Manufacturer_Origin = "台灣",
               Manufacturer_Tel = "0800-000-000"
           },
           new ()
           {
               Manufacturer_ID ="10002",
               Manufacturer_Desc = "",
               Manufacturer_Name = "掌門精釀",
               Manufacturer_Origin = "台灣",
               Manufacturer_Tel = "0800-000-000"
           },
           new ()
           {
               Manufacturer_ID ="10003",
               Manufacturer_Desc = "",
               Manufacturer_Name = "吉姆老爹",
               Manufacturer_Origin = "台灣",
               Manufacturer_Tel = "0800-000-000"
           },
           new ()
           {
               Manufacturer_ID ="10004",
               Manufacturer_Desc = "",
               Manufacturer_Name = "啤酒頭",
               Manufacturer_Origin = "台灣",
               Manufacturer_Tel = "0800-000-000"
           },
           new ()
           {
               Manufacturer_ID ="10005",
               Manufacturer_Desc = "",
               Manufacturer_Name = "酉鬼啤酒",
               Manufacturer_Origin = "台灣",
               Manufacturer_Tel = "0800-000-000"
           },
           new ()
           {
               Manufacturer_ID ="10006",
               Manufacturer_Desc = "",
               Manufacturer_Name = "台中禾樂精釀所",
               Manufacturer_Origin = "台灣",
               Manufacturer_Tel = "0800-000-000"
           }
        };

        public static List<SYS_Unit.outParams> _Unit = new List<SYS_Unit.outParams>()
        {
            new ()
            {
                UNIT_ID = 10001,
                UNIT_DESC = "罐",
                UNIT_NO = "CAN"
            },
            new ()
            {
                UNIT_ID = 10002,
                UNIT_DESC = "Bottle 瓶",
                UNIT_NO = "BOT"
            },

            new ()
            {
                UNIT_ID = 10003,
                UNIT_DESC = "Bucket 桶",
                UNIT_NO = "BKT"
            },

            new()
            {
                UNIT_ID = 10004,
                UNIT_DESC = "Liter 公升",
                UNIT_NO = "LTR"
            },

            new ()
            {
                UNIT_ID = 10005,
                UNIT_DESC = "Milliliter 豪升",
                UNIT_NO = "LTR"
            },

            new ()
            {
                UNIT_ID = 10001,
                UNIT_DESC = "British Gallon (Imperial Gallon) 加侖",
                UNIT_NO = "BGA"
            },

        };

        public static List<Model.Universal.Wms_Stock.outParams> wms_Stocks = new List<Universal.Wms_Stock.outParams>()
        {
            new()
            {
                Manufacturer_ID = "10001",
                Lot_Code = "0123456789",
                Lot_Name = "台虎生啤(嗨)",
                QTY_NORMAL = 10,
                QTY_Cost = 150,
                QTY_Price = 240,
                UNIT_ID = 10001
            },
            new()
            {
                Manufacturer_ID = "10001",
                Lot_Code = "0123456781",
                Lot_Name = "希密爾之歌",
                QTY_NORMAL = 10,
                QTY_Cost = 150,
                QTY_Price = 240,
                UNIT_ID = 10001
            },
            new()
            {
                Manufacturer_ID = "10005",
                Lot_Code = "0123456782",
                Lot_Name = "R&D 88 無果汁 Hazy Sour",
                QTY_NORMAL = 10,
                QTY_Cost = 150,
                QTY_Price = 240,
                UNIT_ID = 10002
            },
            new()
            {
                Manufacturer_ID = "10005",
                Lot_Code = "0123456783",
                Lot_Name = "芭樂鹽小麥",
                QTY_NORMAL = 10,
                QTY_Cost = 150,
                QTY_Price = 240,
                UNIT_ID = 10002
            }
        };
    }

    public static class Extension
    {
        public static string toJson<T>(this T raw)
        {
            return JsonConvert.SerializeObject(raw, Formatting.Indented);
        }

        public static T fromJson<T>(this string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw);
        }
    }
}
