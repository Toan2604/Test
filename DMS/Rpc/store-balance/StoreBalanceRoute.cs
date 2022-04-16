using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.store_balance
{
    [DisplayName("Công nợ đại lý")]
    public class StoreBalanceRoute : Root
    {
        public const string Parent = Module + "/location";
        public const string Master = Module + "/location/store-balance/store-balance-master";
        public const string Detail = Module + "/location/store-balance/store-balance-detail/*";
        public const string Preview = Module + "/store-balance/store-balance-preview";
        private const string Default = Rpc + Module + "/store-balance";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string BulkDelete = Default + "/bulk-delete";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListStore = Default + "/single-list-store";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(StoreBalanceFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(StoreBalanceFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(StoreBalanceFilter.StoreId), FieldTypeEnum.ID.Id },
            { nameof(StoreBalanceFilter.CreditAmount), FieldTypeEnum.DECIMAL.Id },
            { nameof(StoreBalanceFilter.DebitAmount), FieldTypeEnum.DECIMAL.Id },
        };
        private static List<string> FilterList = new List<string> {
            FilterListOrganization,FilterListStore,
        };
        private static List<string> SingleList = new List<string> {
            SingleListOrganization,SingleListStore,
        };
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> { 
                    Parent,
                    Master, Preview, Count, List,
                    Get,  
                }.Concat(SingleList).Concat(FilterList)
            },
            { "Thêm", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    Detail, Create, 
                }.Concat(SingleList).Concat(FilterList)
            },

            { "Sửa", new List<string> { 
                    Parent,            
                    Master, Preview, Count, List, Get,
                    Detail, Update, 
                }.Concat(SingleList).Concat(FilterList)
            },

            { "Xoá", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    Delete, 
                }.Concat(SingleList).Concat(FilterList) 
            },

            { "Xoá nhiều", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    BulkDelete 
                }.Concat(SingleList).Concat(FilterList) 
            },

            { "Xuất excel", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    Export 
                }.Concat(SingleList).Concat(FilterList) 
            },

            { "Nhập excel", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    ExportTemplate, Import 
                }.Concat(SingleList).Concat(FilterList) 
            },
        };
    }
}
