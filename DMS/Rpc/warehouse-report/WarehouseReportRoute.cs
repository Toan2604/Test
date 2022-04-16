using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.warehouse_report
{
    [DisplayName("Báo cáo tồn kho có thể bán")]
    public class WarehouseReportRoute : Root
    {
        public const string Parent = Module + "/inventory";
        public const string Master = Module + "/inventory/warehouse-report/warehouse-report-master";
        public const string Detail = Module + "/inventory/warehouse/warehouse-report-detail/*";
        private const string Default = Rpc + Module + "/warehouse-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListWarehouse = Default + "/filter-list-warehouse";
        public const string FilterListStatus = Default + "/filter-list-status";

        public const string SingleListItem = Default + "/single-list-item";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(WarehouseFilter.OrganizationId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get, Export,
                FilterListOrganization, FilterListWarehouse, FilterListStatus,
                } },
            { "Xuất excel", new List<string> {
                Parent,
                Master, Count, List, Get, Export, FilterListOrganization,  FilterListWarehouse, FilterListStatus,
                Detail,  } },
        };
    }
}
