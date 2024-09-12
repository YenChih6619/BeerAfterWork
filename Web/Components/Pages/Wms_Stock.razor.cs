using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

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

        private int pageCount { get; set; } = 0;
        private int pageIndex { get; set; } = 0;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            outParams = Model.Data.wms_Stocks;
            Travel_Query(1);
        }

        private Task onClose(Model.Universal.Wms_Stock.outParams item, bool saved)
        {
            var aa = item;

            return Task.CompletedTask;
        }

        private Task<bool> OnDeleteAsync(IEnumerable<Model.Universal.Wms_Stock.outParams> items)
        {
            items.ToList().ForEach(i => outParams.Remove(i));
            return Task.FromResult(true);
        }

        private async void onAdd(string obj)
        {

            if (outParams.Where(x => x.Lot_Code == obj.Trim()).Count() <= 0)
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
            int pageC = 10;
            pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToInt32(Model.Data.wms_Stocks.Count()) / pageC)));
            StateHasChanged();
        }
    }
}
