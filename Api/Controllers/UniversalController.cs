using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    public class UniversalController : Controller
    {
        private readonly IConfiguration _configuration;

        public UniversalController(IConfiguration configuration)
        {
            _configuration = configuration;

            Model.Universal.mySqlConStr = _configuration.GetConnectionString("MySqlConStr");
            Model.Universal.sqliteConStr = _configuration.GetConnectionString("sqliteConStt");
        }

        [HttpPost("Universal/sys_unit_Query")]
        public ActionResult sys_unit_Query([FromBody] Model.Universal.unit.inParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.sys_unit_Query(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/sys_Manufacturer_Query")]
        public ActionResult sys_Manufacturer_Query([FromBody] Model.Universal.sys_Manufacturer.inParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.sys_Manufacturer_Query(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/wms_Stock_Query")]
        public ActionResult wms_Stock_Query([FromBody] Model.Universal.wms_Stock.inParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.wms_Stock_Query(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/wms_Stock_Query_Page")]
        public ActionResult wms_Stock_Query_Page([FromBody] Model.Universal.wms_Stock.inParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.wms_Stock_Query_Page(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/wms_Stock_Query_Count")]
        public ActionResult wms_Stock_Query_Count([FromBody] Model.Universal.wms_Stock.inParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.wms_Stock_Query_Count(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/wms_Stock_Insert")]
        public ActionResult wms_Stock_Insert([FromBody] List<Model.Universal.wms_Stock.outParams> inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.wms_Stock_Insert(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/wms_Stock_delete")]
        public ActionResult wms_Stock_delete([FromBody] List<Model.Universal.wms_Stock.outParams> inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.wms_Stock_delete(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/sys_Manufacturer_Insert")]
        public ActionResult sys_Manufacturer_Insert([FromBody] Model.Universal.sys_Manufacturer.outParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.sys_Manufacturer_Insert(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/sys_Table_Query")]
        public ActionResult sys_Table_Query([FromBody] Model.Universal.sys_Table.inParams inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.sys_Table_Query(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/sys_Table_Insert")]
        public ActionResult sys_Table_Insert([FromBody] List<Model.Universal.sys_Table.outParams> inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.sys_Table_Insert(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }

        [HttpPost("Universal/sys_Table_delete")]
        public ActionResult sys_Table_delete([FromBody] List<Model.Universal.sys_Table.outParams> inParams)
        {
            using (CS.Universal Universal = new CS.Universal())
            {
                var Result = Universal.sys_Table_delete(inParams);
                return Content(JsonConvert.SerializeObject(Result, formatting: Formatting.Indented), "application/json");
            }
        }
    }
}
