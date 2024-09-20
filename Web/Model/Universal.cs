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
        public string _url, _jsonData;
        public HttpMethod _methodType;
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

        public class sys_Cart
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

        public class sys_Cart_travel
        {
            public class inParams
            {
                public string table_Name { get; set; }
                public string lot_Name { get; set; }
                public string lot_Code { get; set; }
                public decimal? QTY_Price { get; set; }
                public decimal? Count { get; set; }
                public decimal? Discount { get; set; }
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

        public class sys_Unit
        {

            /// <summary>
            /// https://cfgate.trade.gov.tw/boft_pw/do/PW303_SC1Action?code=ALL&type=3 <br/>
            /// 依照台灣輸出入電子貨品簽證
            /// </summary>
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

        public class sys_Table
        {
            public class outParams
            {

                /// <summary>
                /// 餐桌名稱，作為主鍵使用。
                /// </summary>
                [Display(Name = "餐桌編號")]
                public string table_Name { get; set; }

                /// <summary>
                /// 餐桌的座位數量。
                /// </summary>
                [Display(Name = "座位人數")]
                public int seat_Count { get; set; }

                /// <summary>
                /// 指示是否結帳。若為已結帳則為 true，否則為 false。
                /// </summary>
                [Display(Name = "是否結帳")]
                public bool isPrivate_Room { get; set; } = true;

                /// <summary>
                /// 餐桌的狀態。
                /// </summary>
                [Display(Name = "餐桌狀態")]

                public int table_Status { get; set; } = 0;

                /// <summary>
                /// 餐桌的備註信息。
                /// </summary>
                [Display(Name = "備註")]
                public string memo { get; set; }
            }
        }
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
