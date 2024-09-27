namespace Web.Model
{
    public static class Report
    {
        public class profit
        {
            public class inParams
            {
                public string lot_Code { set; get; }

                public DateTime StartDate { set; get; }
                public DateTime EndDate { set; get; }
            }

            public class outParams
            {
                public string lot_Code { set; get; }
                public string lot_Name { set; get; }
                public string Total_QTY_Price { set; get; }
                public string Manufacturer_Name { set; get; }
                public string Manufacturer_Origin { set; get; }
            }

        }

    }
}
