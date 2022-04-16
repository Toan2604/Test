using DMS.Common;
using DMS.Rpc.dashboards.store_information;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Controllers.dashboards.store_information
{
    [DisplayName("Dashboard thông tin điểm bán")]
    public class DashboardStoreInformationViewRoute : ViewRoot
    {
        public const string Parent = Module + "/dashboards";
        public const string Master = Module + "/dashboards/store-information";
        private const string Default = Rpc + Module + "/dashboards/store-information";

        public const string StoreCounter = Default + "/store-counter";
        public const string BrandStatistic = Default + "/brand-statistics";
        public const string BrandUnStatistic = Default + "/brand-un-statistics";
        public const string StoreCoverage = Default + "/store-coverage";
        public const string ProductGroupingStatistic = Default + "/product-grouping-statistics";
        public const string ProductGroupingNumberStatistic = Default + "/product-grouping-number-statistics";
        public const string TopBrand = Default + "/top-brand";
        public const string EstimatedRevenueStatistic = Default + "/estimated-revenue-statistics";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(DashboardStoreInformation_StoreFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };
    }
}
