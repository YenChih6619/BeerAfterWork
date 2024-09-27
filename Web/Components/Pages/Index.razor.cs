using Web.Model;

namespace Web.Components.Pages
{
    public partial class Index
    {
        private IEnumerable<Model.Report.profit.outParams> profit_Today { set; get; } = new List<Model.Report.profit.outParams>();
        private IEnumerable<Model.Report.profit.outParams> profit_Month { set; get; } = new List<Model.Report.profit.outParams>();
        private IEnumerable<Model.Report.profit.outParams> profit_Year { set; get; } = new List<Model.Report.profit.outParams>();

        private static List<string> Images =>
            [
            "./images/Pic0.jpg",
            "./images/Pic1.jpg",
            "./images/Pic2.jpg"
            ];

        protected override void OnInitialized()
        {
            base.OnInitialized();



            using (Call call = new Call(
                Model.Universal.ExeUrl + "Universal/sys_Cart_travel_Query",
                new { startDate = DateTime.Now.ToString("yyyyMMdd000000000"), endDate = DateTime.Now.ToString("yyyyMMdd235959999") }.toJson(),
                HttpMethod.Post
                ))
            {
                

            }
        }

    }

   
}
