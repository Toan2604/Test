using System;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    public class DashboardStoreInformation_StoreCounterDTO : DataDTO
    {
        public long SurveyedStoreCounter { get; set; }
        public long StoreCounter { get; set; }
        public decimal Rate => StoreCounter == 0 ? 0 : Math.Round(((decimal)SurveyedStoreCounter / StoreCounter) * 100, 2);
    }

    public class DashboardStoreInformation_StoreCounterFilterDTO : DashboardStoreInformation_FilterDTO
    {
    }
}
