using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.Rpc.sync_fast
{
    [DisplayName("Đồng bộ FAST")]
    public class SyncRoute : Root
    {
        private const string Default = Rpc + Module + "/sync";

        public const string BulkMergeStore = Default + "/bulk-merge-store";
        public const string CreateStore = Default + "/create-store";
        public const string UpdateStore = Default + "/update-store";

        public const string BulkMergeStoreGrouping = Default + "/bulk-merge-store-grouping";
        public const string CreateStoreGrouping = Default + "/create-store-grouping";
        public const string UpdateStoreGrouping = Default + "/update-store-grouping";

        public const string BulkMergeStoreType = Default + "/bulk-merge-store-type";
        public const string CreateStoreType = Default + "/create-store-type";
        public const string UpdateStoreType = Default + "/update-store-type";

        public const string CreateWarehouse = Default + "/create-warehouse";
        public const string UpdateWarehouse = Default + "/update-warehouse";

        public const string ListInventory = Default + "/list-inventory";
        public const string CreateInventory = Default + "/create-inventory";
        public const string UpdateInventory = Default + "/update-inventory";

        public const string CreatePriceList = Default + "/create-price-list";
        public const string UpdatePriceList = Default + "/update-price-list";

        public const string CreateDirectSalesOrder = Default + "/create-direct-sales-order";
        public const string UpdateDirectSalesOrder = Default + "/update-direct-sales-order";
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                ListInventory
                } 
            },
            { "Thêm", new List<string> {
                ListInventory,
                BulkMergeStore, CreateStore, CreateStoreGrouping, CreateStoreType, BulkMergeStoreGrouping, BulkMergeStoreType, CreateWarehouse, CreatePriceList, CreateDirectSalesOrder, CreateInventory
                }
            },
            { "Sửa", new List<string> {
                ListInventory,
                BulkMergeStore, UpdateStore, UpdateStoreGrouping, UpdateStoreType, BulkMergeStoreGrouping, BulkMergeStoreType, UpdateWarehouse, UpdatePriceList,UpdateDirectSalesOrder, UpdateInventory
                }
            }
        };
    }

}
