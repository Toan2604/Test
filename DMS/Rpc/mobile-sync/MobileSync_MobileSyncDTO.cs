using System;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_MobileSyncDTO
    {
        public DateTime Timestamp { get; set; }
    }

    public class MobileSync_ChangeDTO
    {
        public MobileSync_BannerSyncDTO Banner { get; set; }
        public MobileSync_IndirectSalesOrderSyncDTO IndirectSalesOrder { get; set; }
        public MobileSync_ProblemSyncDTO Problem { get; set; }
        public MobileSync_ProductSyncDTO Product { get; set; }
        public MobileSync_ItemSyncDTO Item { get; set; }
        public MobileSync_StoreScoutingSyncDTO StoreScouting { get; set; }
        public MobileSync_StoreSyncDTO StoreInScoped { get; set; }
        public MobileSync_StoreSyncDTO StorePlanned { get; set; }
        public MobileSync_StoreSyncDTO StoreUnplanned { get; set; }
        public MobileSync_SurveySyncDTO Survey { get; set; }
    }
}
