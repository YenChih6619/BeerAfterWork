using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;
using Web.Model;

namespace Web.Components.Pages
{
    public partial class Sys_Table
    {
        private List<Model.Universal.sys_Table.outParams> outParams { set; get; } = new();
        int res;
        bool canParse;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            using (Call call = new(
                 Model.Universal.ExeUrl + "Universal/sys_Table_Query",
                 new { }.toJson(),
                 HttpMethod.Post
                ))
            {
                outParams = call.CallAPI().fromJson<List<Model.Universal.sys_Table.outParams>>();
            }
        }

        private Task<QueryData<Model.Universal.sys_Table.outParams>> OnQueryAsync(QueryPageOptions options)
        {     
            using (Call call = new(
               Model.Universal.ExeUrl + "Universal/sys_Table_Query",
               new { }.toJson(),
               HttpMethod.Post
              ))
            {
                outParams = call.CallAPI().fromJson<List<Model.Universal.sys_Table.outParams>>();
            }

            IEnumerable<Model.Universal.sys_Table.outParams> items = outParams;

            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<Model.Universal.sys_Table.outParams>() { Items = items, TotalCount = total, IsSorted = true, IsFiltered = true, IsSearch = true });
        }

        private Task<bool> OnSaveAsync(Model.Universal.sys_Table.outParams raw, ItemChangedType changedType)
        {
            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Table_Insert",
                new List<object>() { raw }.toJson(),
                HttpMethod.Post
                ))
            {
                switch (changedType)
                {
                    case ItemChangedType.Add:
                        canParse = int.TryParse(call.CallAPI(), out res);
                        break;


                    case ItemChangedType.Update:
                        canParse = int.TryParse(call.CallAPI(), out res);
                        break;
                }

                if (canParse == true && res > 0)
                {
                    using (Call api = new Call(
                       Model.Universal.ExeUrl + "Universal/sys_Table_Query",
                        new { }.toJson(),
                        HttpMethod.Post
                        ))
                    {
                        outParams = api.CallAPI().fromJson<List<Model.Universal.sys_Table.outParams>>();
                    }

                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
        }

        private Task<Model.Universal.sys_Table.outParams> OnAddAsync()
        {
            return Task.FromResult(new Model.Universal.sys_Table.outParams() { seat_Count = 4, table_Status = 0, isPrivate_Room = true });
        }
    }
}
