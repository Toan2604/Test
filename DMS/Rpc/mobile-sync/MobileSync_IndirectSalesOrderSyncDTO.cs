using System.Collections.Generic;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_IndirectSalesOrderSyncDTO
    {
        public List<MobileSync_IndirectSalesOrderDTO> Created { get; set; }
        public List<MobileSync_IndirectSalesOrderDTO> Updated { get; set; }
        public List<MobileSync_IndirectSalesOrderDTO> Deleted { get; set; }
    }


}
