using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Web.Model;

namespace Web.Components.Pages
{
    public partial class Sys_cashRegister
    {
      



        private List<Model.Universal.sys_ProductList.outParams> outParams { set; get; } = new List<Universal.sys_ProductList.outParams>();

        public List<Model.Universal.sys_Manufacturer.outParams> sys_Manufacturer { set; get; } = new List<Universal.sys_Manufacturer.outParams>();

        private List<Model.Universal.sys_Cart.outParams> Consumption { set; get; } = new List<Model.Universal.sys_Cart.outParams>();

        public List<Model.Universal.sys_Unit.outParams> sys_Unit { set; get; } = new List<Universal.sys_Unit.outParams>();

        private BootstrapInput<string> lotCodeRef { set; get; }

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
        }

        private void onConfirm(Model.Universal.sys_ProductList.outParams item)
        {
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
    }
}
