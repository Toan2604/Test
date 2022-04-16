using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    public class DashboardStoreInformation_ProductGroupingNumberStatisticsDTO : DataDTO
    {
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public long Value8 { get; set; }
        public long Value7 { get; set; }
        public long Value6 { get; set; }
        public long Value5 { get; set; }
        public long Value4 { get; set; }
        public long Value3 { get; set; }
        public long Total { get; set; }
        public decimal Rate8 => Total == 0 ? 0 : Math.Round(((decimal)Value8 / Total) * 100, 2);
        public decimal Rate7 => Total == 0 ? 0 : Math.Round(((decimal)Value7 / Total) * 100, 2);
        public decimal Rate6 => Total == 0 ? 0 : Math.Round(((decimal)Value6 / Total) * 100, 2);
        public decimal Rate5 => Total == 0 ? 0 : Math.Round(((decimal)Value5 / Total) * 100, 2);
        public decimal Rate4 => Total == 0 ? 0 : Math.Round(((decimal)Value4 / Total) * 100, 2);
        public decimal Rate3 => Total == 0 ? 0 : Math.Round(((decimal)Value3 / Total) * 100, 2);
    }

    public class DashboardStoreInformation_ProductGroupingNumberStatisticsFilterDTO : DashboardStoreInformation_FilterDTO
    {
    }

    public class DashboardStoreInformation_ProductGroupingNumberStatisticsExportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<DashboardStoreInformation_StoreDTO> Stores { get; set; }
    }

}
