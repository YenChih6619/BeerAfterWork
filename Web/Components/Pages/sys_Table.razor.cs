using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;
using Web.Model;

namespace Web.Components.Pages
{
    public partial class Sys_Table
    {
        private List<Model.Universal.sys_Table.outParams> outParams { set; get; } = new();

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
            IEnumerable<Model.Universal.sys_Table.outParams> items = outParams;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            return Task.FromResult(new QueryData<Model.Universal.sys_Table.outParams>() { Items = items, TotalCount = total, IsSorted = true, IsFiltered = true, IsSearch = true });
        }
    }
}
