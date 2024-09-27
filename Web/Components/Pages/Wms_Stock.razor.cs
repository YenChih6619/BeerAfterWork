using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;
using Web.Model;

namespace Web.Components.Pages
{
    public partial class Wms_Stock
    {


        [Inject]
        [NotNull]
        private SwalService? SwalService { get; set; }

        private BootstrapInput<string> lotCodeRef { set; get; }

        public Model.Universal.Wms_Stock.outParams init { set; get; } = new Model.Universal.Wms_Stock.outParams();

        public List<Model.Universal.Wms_Stock.outParams> outParams { set; get; } = new List<Model.Universal.Wms_Stock.outParams>();

        public List<Model.Universal.sys_Manufacturer.outParams> sys_Manufacturer { set; get; } = new List<Universal.sys_Manufacturer.outParams>();

        public List<Model.Universal.sys_Unit.outParams> sys_Unit { set; get; } = new List<Universal.sys_Unit.outParams>();

        private int pageCount { get; set; } = 0;

        private int pageIndex { get; set; } = 0;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            using (Call callapi = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Manufacturer_Query",
                new Dictionary<string, object>().toJson(),
                HttpMethod.Post))
            {
                sys_Manufacturer = callapi.CallAPI().fromJson<List<Model.Universal.sys_Manufacturer.outParams>>();
                sys_Manufacturer.Insert(0, new Universal.sys_Manufacturer.outParams() 
                {
                    Manufacturer_ID = string.Empty,
                    Manufacturer_Name = "全選"
                });
            };

 

            using (Call callapi = new Call(
                Model.Universal.ExeUrl + "Universal/sys_unit_Query",
                new Dictionary<string, object>().toJson(),
                HttpMethod.Post))
            {
                sys_Unit = callapi.CallAPI().fromJson<List<Model.Universal.sys_Unit.outParams>>();
            };

            Travel_Count();
            Travel_Query(1);
        }

        private Task onClose(Model.Universal.Wms_Stock.outParams item, bool saved)
        {
            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Insert",
                new List<Model.Universal.Wms_Stock.outParams>() { item }.toJson(),
                HttpMethod.Post
                ))
            {
                var res = call.CallAPI();
                var intbool = int.TryParse(res, out int intres);

                SwalService.ShowModal(new SwalOption()
                {
                    Category = SwalCategory.Information,
                    Content = intbool ? $"本次異動資料 {intres} 筆" : res,
                    Title = "入庫結果",
                    IsConfirm = false,
                    ShowClose = true,
                    ConfirmButtonText = "確定",
                    CancelButtonText = "取消"
                });

                using var callApi = new Model.Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Query_Page",
                    new
                    {
                        pageNumber = 1,
                        Manufacturer_ID = init.Manufacturer_ID,
                        Lot_Code = init.Lot_Code,
                        Lot_Name = init.Lot_Name
                    }.toJson(),
                HttpMethod.Post);

                Travel_Count();

                outParams = callApi.CallAPI().fromJson<List<Model.Universal.Wms_Stock.outParams>>();
            }

            StateHasChanged();
            return Task.CompletedTask;


            return Task.CompletedTask;
        }

        private async Task<bool> OnDeleteAsync(IEnumerable<Model.Universal.Wms_Stock.outParams> items)
        {
            items.ToList().ForEach(async i =>
            {
                outParams.Remove(i);

            });

            await delete(items.ToList());

            return await Task.FromResult(true);
        }

        private async void onAdd(string obj)
        {
            using Call callapi = new Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Query",
                new Model.Universal.Wms_Stock.inParams()
                {
                    Lot_Code = obj,
                }.toJson()
                , HttpMethod.Post);

            var raw = callapi.CallAPI().fromJson<List<Model.Universal.Wms_Stock.outParams>>();


            if (
                raw.Where(x => x.Lot_Code == obj.Trim()).Count() <= 0 &&
                outParams.Where(x => x.Lot_Code == obj.Trim()).Count() <= 0
                )
            {
                outParams.Insert(0, new Model.Universal.Wms_Stock.outParams()
                {
                    Lot_Code = obj,
                    Manufacturer_ID = null,
                    UNIT_ID = 0
                });

                SwalOption option = new SwalOption()
                {
                    Category = SwalCategory.Success,
                    Title = "新增庫存成功",
                    Content = "請至下方表格編輯庫存",
                    CancelButtonText = "取消",
                    ConfirmButtonText = "確認",
                    Delay = 2000,
                    ShowClose = false
                };

                await SwalService.ShowModal(option);
            }
            else
            {
                if (
                    raw.Count > 0 &&
                    outParams.Where(x => x.Lot_Code == obj.Trim()).Count() <= 0
                    )
                {
                    foreach (var item in raw)
                    {
                        outParams.Insert(0, item);
                    }
                }

                SwalOption option = new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Title = "條碼已存在",
                    Content = "條碼已存在，請至下方編輯",
                    CancelButtonText = "取消",
                    ConfirmButtonText = "確認",
                    Delay = 2000,
                    ShowClose = false
                };

                await SwalService.ShowModal(option);
            }

            lotCodeRef.SetValue(string.Empty);
            await lotCodeRef.FocusAsync();
        }

        private Task OnPageLinkClick(int _pageIndex)
        {
            pageIndex = _pageIndex;
            Travel_Query(pageIndex);
            StateHasChanged();
            return Task.CompletedTask;
        }

       

        private void Travel_Query(int page)
        {
            using var callApi = new Model.Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Query_Page",
                new
                {
                    pageNumber = page,
                    Manufacturer_ID = init.Manufacturer_ID,
                    Lot_Code = init.Lot_Code,
                    Lot_Name = init.Lot_Name
                }.toJson(),
                HttpMethod.Post);

            outParams = callApi.CallAPI().fromJson<List<Model.Universal.Wms_Stock.outParams>>();

            StateHasChanged();
        }

        private void Travel_Count()
        {
            int pageC = 10;

            using var callApi = new Model.Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Query_Count",
                new Model.Universal.Wms_Stock.inParams()
                {
                    Manufacturer_ID = init.Manufacturer_ID,
                    Lot_Code = init.Lot_Code,
                    Lot_Name = init.Lot_Name
                }.toJson(),
                HttpMethod.Post);

            pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToInt32((callApi.CallAPI().fromJson<int>()) / pageC))));

            StateHasChanged();
        }

        private Task insert()
        {
            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Insert",
                outParams.toJson(),
                HttpMethod.Post
                ))
            {
                var res = call.CallAPI();
                var intbool = int.TryParse(res, out int intres);

                SwalService.ShowModal(new SwalOption()
                {
                    Category = SwalCategory.Information,
                    Content = intbool ? $"本次異動資料 {intres} 筆" : res,
                    Title = "入庫結果",
                    IsConfirm = false,
                    ShowClose = true,
                    ConfirmButtonText = "確定",
                    CancelButtonText = "取消"
                });

                using var callApi = new Model.Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Query_Page",
                    new
                    {
                        pageNumber = 1,
                        Manufacturer_ID = init.Manufacturer_ID,
                        Lot_Code = init.Lot_Code,
                        Lot_Name = init.Lot_Name
                    }.toJson(),
                HttpMethod.Post);

                Travel_Count();

                outParams = callApi.CallAPI().fromJson<List<Model.Universal.Wms_Stock.outParams>>();
            }

            StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task delete(List<Model.Universal.Wms_Stock.outParams> raw)
        {
            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_delete",
                raw.toJson(),
                HttpMethod.Post))
            {
                string res = call.CallAPI();
                var intbool = int.TryParse(res, out int intres);

                await SwalService.ShowModal(new SwalOption()
                {
                    Category = SwalCategory.Information,
                    Content = intbool ? $"本次異動資料 {intres} 筆" : res,
                    Title = "入庫結果",
                    IsConfirm = false,
                    ShowClose = true,
                    ConfirmButtonText = "確定",
                    CancelButtonText = "取消"
                });
            }

            StateHasChanged();
        }


    }
}
