using DMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.store_information
{
    [DisplayName("Dashboard thông tin điểm bán")]
    public class DashboardStoreInformationRoute : Root
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

        public const string ExportBrandStatistic = Default + "/export-brand-statistics";
        public const string ExportBrandUnStatistic = Default + "/export-brand-un-statistics";
        public const string ExportEstimatedRevenueStatistic = Default + "/export-estimated-revenue-statistics";
        public const string ExportProductGroupingStatistic = Default + "/export-product-grouping-statistics";
        public const string ExportProductGroupingNumberStatistic = Default + "/export-product-grouping-number-statistics";
        public const string ExportTopBrand = Default + "/export-top-brand";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListBrand = Default + "/filter-list-brand";
        public const string FilterListDistrict = Default + "/filter-list-district";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListAppUser = Default + "/filter-list-app-user";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(DashboardStoreInformation_BrandStatisticsFilterDTO.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(DashboardStoreInformation_StoreFilterDTO.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Hiển thị", new List<string> {
                Parent,
                Master,
                StoreCounter, BrandStatistic, BrandUnStatistic, StoreCoverage, ProductGroupingNumberStatistic, TopBrand, ProductGroupingStatistic,
                FilterListOrganization, FilterListBrand, FilterListDistrict, FilterListProvince, FilterListAppUser,
                EstimatedRevenueStatistic
            } },

            { "Xuất Excel", new List<string> {
                Parent,
                Master,
                StoreCounter, BrandStatistic, BrandUnStatistic, StoreCoverage, ProductGroupingNumberStatistic, TopBrand, EstimatedRevenueStatistic,
                FilterListOrganization, FilterListBrand, FilterListDistrict, FilterListProvince, FilterListAppUser,
                ProductGroupingStatistic,
                ExportBrandStatistic, ExportBrandUnStatistic, ExportEstimatedRevenueStatistic, ExportProductGroupingNumberStatistic, ExportTopBrand,
                ExportProductGroupingStatistic,
            } },
        };
    }
}
