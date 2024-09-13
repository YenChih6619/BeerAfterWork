using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Web.Model;
using static Web.Model.Extension;

namespace Web.Components.Pages
{
    public partial class Sys_Manufacturerrazor
    {
        [Inject]
        [NotNull]
        private SwalService? SwalService { get; set; }


        [Inject]
        [NotNull]
        private DialogService? DialogService { get; set; }

        public Model.Universal.sys_Manufacturer.outParams init { set; get; } = new Model.Universal.sys_Manufacturer.outParams();

        List<Model.Universal.sys_Manufacturer.outParams> outParams { get; set; } = new List<Model.Universal.sys_Manufacturer.outParams>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            using var callApi = new Model.Call(
                Model.Universal.ExeUrl + "Universal/sys_Manufacturer_Query",
                new Model.Universal.sys_Manufacturer.inParams()
                {
                    Manufacturer_Name = init.Manufacturer_Name,
                    Manufacturer_Origin = init.Manufacturer_Origin

                }.toJson(),
                HttpMethod.Post);
            outParams = callApi.CallAPI().fromJson<List<Model.Universal.sys_Manufacturer.outParams>>();
        }

        private Task onClose(Model.Universal.sys_Manufacturer.outParams item, bool saved)
        {
            var aa = item;

            return Task.CompletedTask;
        }

        private Task<bool> OnDeleteAsync(IEnumerable<Model.Universal.sys_Manufacturer.outParams> items)
        {
            items.ToList().ForEach(i => outParams.Remove(i));
            return Task.FromResult(true);
        }

        private async Task onInsert()
        {
            var item = Utility.GenerateEditorItems<Model.Universal.sys_Manufacturer.outParams>();

            foreach (var ite in item)
            {
                ite.PlaceHolder = "請輸入";
                if (ite.Text == "製造商 ID")                
                {
                    ite.Readonly = true;
                    ite.SkipValidate = false;
                }
            }


            var option = new EditDialogOption<Model.Universal.sys_Manufacturer.outParams>()
            {
                CloseButtonText = "取消",
                SaveButtonText = "送出",
                Items = item,
                Model = new Model.Universal.sys_Manufacturer.outParams(),
                ItemsPerRow = 2,
                RowType = RowType.Inline,
                OnCloseAsync = () =>
                {
                    return Task.CompletedTask;
                },
                OnEditAsync = context =>
                {
  
                    using var callapi = new Call(
                        Model.Universal.ExeUrl + "/Universal/sys_Manufacturer_Insert",
                        context.Model.toJson(),
                        HttpMethod.Post
                        );
                    var res = callapi.CallAPI();

                    using var callApi = new Model.Call(
                        Model.Universal.ExeUrl + "Universal/sys_Manufacturer_Query",
                        new Model.Universal.sys_Manufacturer.inParams()
                        {
                            Manufacturer_Name = init.Manufacturer_Name,
                            Manufacturer_Origin = init.Manufacturer_Origin

                        }.toJson(),
                        HttpMethod.Post);
                    outParams = callApi.CallAPI().fromJson<List<Model.Universal.sys_Manufacturer.outParams>>();

                    return Task.FromResult(true);
                }
            };

            await DialogService.ShowEditDialog(option);


        }
    }
}
