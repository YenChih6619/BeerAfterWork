using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using Web.Model;

namespace Web.Components.Pages
{
    public partial class Sys_cashRegister
    {
        [Inject]
        [NotNull]
        private SwalService? SwalService { get; set; }

        private Model.Universal.sys_Table.outParams sys_Table { set; get; } = new Universal.sys_Table.outParams();

        private List<Model.Universal.sys_Table.outParams> sys_Table_Query { set; get; } = new List<Universal.sys_Table.outParams>();

        private List<Model.Universal.sys_ProductList.outParams> outParams { set; get; } = new List<Universal.sys_ProductList.outParams>();

        private List<Model.Universal.sys_Manufacturer.outParams> sys_Manufacturer { set; get; } = new List<Universal.sys_Manufacturer.outParams>();

        private List<Model.Universal.sys_Cart.outParams> Consumption { set; get; } = new List<Model.Universal.sys_Cart.outParams>();

        private List<Model.Universal.sys_Unit.outParams> sys_Unit { set; get; } = new List<Universal.sys_Unit.outParams>();

        private BootstrapInput<string> lotCodeRef { set; get; }

        private static string? GetTextCallback(Model.Universal.sys_Table.outParams v) => v.table_Name;

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

            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/wms_Stock_Query",
                new { }.toJson(),
                HttpMethod.Post))
            {
                outParams = call.CallAPI()
                .fromJson<List<Model.Universal.Wms_Stock.outParams>>()
                .Select(x =>
                {
                    string manu = string.Empty;
                    string org = string.Empty;
                    string unit = string.Empty;

                    var _manu = sys_Manufacturer.Where(v => v.Manufacturer_ID == x.Manufacturer_ID);
                    if (_manu != null)
                    {
                        manu = _manu.FirstOrDefault().Manufacturer_Name;
                        org = _manu.FirstOrDefault().Manufacturer_Origin;
                    }

                    var _unit = sys_Unit.Where(v => v.UNIT_ID == x.UNIT_ID);
                    if (_unit != null)
                    {
                        unit = _unit.FirstOrDefault().UNIT_NO;
                    }

                    return new Universal.sys_ProductList.outParams()
                    {
                        Count = 0,
                        lot_Code = x.Lot_Code,
                        lot_Name = x.Lot_Name,
                        manufacturer_Name = manu,
                        manufacturer_Origin = org,
                        Qty_Noraml = x.QTY_NORMAL,
                        Qty_Price = x.QTY_Price,
                        Unit_No = unit
                    };

                }).ToList();
            };

            using (Call call = new(
                Model.Universal.ExeUrl + "Universal/sys_Table_Query",
                new { }.toJson(),
                HttpMethod.Post
               ))
            {
                sys_Table_Query = call.CallAPI().fromJson<List<Model.Universal.sys_Table.outParams>>();
            }
        }

        private void onConfirm(Model.Universal.sys_ProductList.outParams item)
        {
            if (string.IsNullOrEmpty(sys_Table.table_Name)) { return; }
            if (sys_Table.isPrivate_Room == true) { return; }

            var _item = Consumption.Where(x => x.Lot_Code == item.lot_Code).FirstOrDefault();

            if (_item != null)
            {
                _item.Count++;
            }
            else
            {
                Consumption.Insert(0, new()
                {
                    Lot_Code = item.lot_Code,
                    Discount = 100,
                    Count = 1,
                    Lot_Name = item.lot_Name,
                    QTY_Price = item.Qty_Price
                });
            }
        }

        private void onEnter(string obj)
        {
            if (string.IsNullOrEmpty(obj)) { return; }

            var item = outParams.Where(x => x.lot_Code == obj);
            if (item.Count() <= 0)
            {
                lotCodeRef.SetValue(string.Empty);
            }
            else
            {
                onConfirm(item.First());
                lotCodeRef.SetValue(string.Empty);
                lotCodeRef.FocusAsync();
            }
        }

        private async Task OnListViewItemClick(Model.Universal.sys_Table.outParams raw, ISelectObjectContext<Model.Universal.sys_Table.outParams?> context)
        {
            onChange();

            // 设置组件值
            context.SetValue(raw);

            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Cart_temp_Query",
                new { table_Name = raw.table_Name }.toJson(),
                HttpMethod.Post
                ))
            {
                Consumption = call.CallAPI().fromJson<List<Model.Universal.sys_Cart.outParams>>();
            }

            // 当前模式为单选，主动关闭弹窗
            await context.CloseAsync();
        }

        private async Task onTake()
        {
            if (string.IsNullOrEmpty(sys_Table.table_Name)) { return; }
            if (sys_Table.isPrivate_Room == false)
            {
                await SwalService.ShowModal(new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Content = sys_Table.table_Name,
                    Title = "尚未結帳?",
                    IsConfirm = true,
                    ShowClose = false,
                    ConfirmButtonText = "確定",
                    CancelButtonText = "取消"
                });

                return;
            }

            var open = await SwalService.ShowModal(new SwalOption()
            {
                Category = SwalCategory.Information,
                Content = sys_Table.table_Name,
                Title = "確定開桌?",
                IsConfirm = true,
                ShowClose = true,
                ConfirmButtonText = "確定",
                CancelButtonText = "取消"
            });

            if (open == false) { return; }

            sys_Table.isPrivate_Room = false;
            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Table_Insert",
                new List<object>() { sys_Table }.toJson(),
                HttpMethod.Post
                ))
            {
                call.CallAPI();
            }

            StateHasChanged();
        }

        private void onChange()
        {
            if (sys_Table.isPrivate_Room == true) { return; }

            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Cart_temp_delete",
                new { table_Name = sys_Table.table_Name }.toJson(),
                HttpMethod.Post
                ))
            {
                call.CallAPI();
            }

            foreach (var item in Consumption)
            {
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                pairs.Add("table_Name", sys_Table.table_Name);
                pairs.Add("lot_Name", item.Lot_Name);
                pairs.Add("lot_Code", item.Lot_Code);
                pairs.Add("qtY_Price", item.QTY_Price);
                pairs.Add("count", item.Count);
                pairs.Add("discount", item.Discount);

                items.Add(pairs);
            }

            var aa = items.toJson();

            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Cart_temp_Insert",
                items.toJson(),
                HttpMethod.Post
                ))
            {
                call.CallAPI();
            }
        }

        private async Task checkOut()
        {
            
        }
    }
}
