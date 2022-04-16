using System.Collections.Generic;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_ItemSyncDTO
    {
        public List<MobileSync_ItemDTO> Created { get; set; }
        public List<MobileSync_ItemDTO> Updated { get; set; }
        public List<MobileSync_ItemDTO> Deleted { get; set; }
    }
}
