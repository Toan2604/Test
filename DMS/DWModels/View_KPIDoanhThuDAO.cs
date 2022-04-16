using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class View_KPIDoanhThuDAO
    {
        public int? Month1 { get; set; }
        public int? Year1 { get; set; }
        public string Province { get; set; }
        public string KPI { get; set; }
        public decimal? Revenue { get; set; }
    }
}
