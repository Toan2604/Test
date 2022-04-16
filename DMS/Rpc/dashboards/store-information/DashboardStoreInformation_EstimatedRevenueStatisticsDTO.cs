using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    public class DashboardStoreInformation_EstimatedRevenueStatisticsDTO : DataDTO
    {
        public long EstimatedRevenueId { get; set; }
        public string EstimatedRevenueName { get; set; }
        public long Value { get; set; }
        public long Total { get; set; }
        public decimal Rate => Total == 0 ? 0 : Math.Round(((decimal)Value / Total) * 100, 2);
    }

    public class DashboardStoreInformation_EstimatedRevenueStatisticsFilterDTO : DashboardStoreInformation_FilterDTO
    {
        public IdFilter EstimatedRevenueId { get; set; }
    }
}
