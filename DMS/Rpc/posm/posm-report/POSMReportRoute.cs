using DMS.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    [DisplayName("Báo cáo POSM")]
    public class POSMReportRoute : Root
    {
        public const string Parent = Module + "/posm-report";
        public const string Master = Module + "/posm-report/posm-report-master";
        public const string Preview = Module + "/posm-report/posm-report-preview";
        private const string Default = Rpc + Module + "/posm-report";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Export = Default + "/export";

        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListShowingItem = Default + "/filter-list-showing-item";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ShowingOrderFilter.AppUserId), FieldTypeEnum.ID.Id },
            { nameof(ShowingOrderFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(CurrentContext.UserId), FieldTypeEnum.ID.Id },
        };

        private static List<string> FilterList = new List<string> {
            FilterListOrganization, FilterListStoreType, FilterListStoreGrouping, FilterListStore, FilterListShowingItem,
        };
        private static List<string> SingleList = new List<string>
        {
        };
        private static List<string> CountList = new List<string>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                    Parent,
                    Master, Preview, Count, List,
                }.Concat(FilterList)
            },

            { "Xuất excel", new List<string> {
                    Parent,
                    Master, Preview, Count, List,
                    Export
                }.Concat(FilterList)
            },
        };
    }
}
