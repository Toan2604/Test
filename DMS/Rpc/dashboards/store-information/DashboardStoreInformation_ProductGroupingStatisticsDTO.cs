using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    public class DashboardStoreInformation_ProductGroupingStatisticsDTO : DataDTO
    {
        public long Total { get; set; }
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public List<DashboardStoreInformation_ProductGroupingDTO> ProductGroupings { get; set; }
    }

    public class DashboardStoreInformation_ProductGroupingStatisticsFilterDTO : DashboardStoreInformation_FilterDTO
    {
    }

    public class DashboardStoreInformation_ProductGroupingDTO : DataDTO
    {
        public long ProductGroupingId { get; set; }
        public string ProductGroupingName { get; set; }
        public long Total { get; set; }
        public long Value { get; set; }
        public decimal Rate => Total == 0 ? 0 : Math.Round(((decimal)Value / Total)*100, 2);
    }

    public class DashboardStoreInformation_ProductGroupingStatisticsExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<DashboardStoreInformation_StoreDTO> Stores { get; set; }
    }
}
