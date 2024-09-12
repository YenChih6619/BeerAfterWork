using BootstrapBlazor.Components;
using Web.Model;

namespace Web.Components.Pages
{
    public partial class Sys_cashRegister
    {
        private List<Model.Universal.sys_ProductList.outParams> outParams { set; get; }

        private List<Model.Universal.SYS_Cart.outParams> Consumption { set; get; } = new List<Model.Universal.SYS_Cart.outParams>();

        private BootstrapInput<string> lotCodeRef { set; get; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            outParams = Model.Data.wms_Stocks.Select(x =>
            {
                string manu = string.Empty;
                string org = string.Empty;
                string unit = string.Empty;

                var _manu = Model.Data._Manufacturer.Where(v => v.Manufacturer_ID == x.Manufacturer_ID);
                if (_manu != null)
                {
                    manu = _manu.FirstOrDefault().Manufacturer_Name;
                    org = _manu.FirstOrDefault().Manufacturer_Origin;
                }

                var _unit = Model.Data._Unit.Where(v => v.UNIT_ID == x.UNIT_ID);
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
