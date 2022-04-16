using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    public class DashboardStoreInformation_BrandStatisticsDTO : DataDTO
    {
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public long Value { get; set; }
        public long Total { get; set; }
        public decimal Rate => Total == 0 ? 0 : Math.Round(((decimal)Value / Total) * 100, 2);
    }

    public class DashboardStoreInformation_BrandStatisticsFilterDTO : DashboardStoreInformation_FilterDTO
    {
    }
}
