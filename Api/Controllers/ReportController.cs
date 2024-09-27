using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    public class ReportController : Controller
    {
        [HttpPost("Report/profit_Query")]
        public ActionResult profit_Query([FromBody] Model.Report.profit.inParams inParams)
        {
            using (CS.Report report = new())
            {
                var Result = report.profit_Query(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }




    }
}
