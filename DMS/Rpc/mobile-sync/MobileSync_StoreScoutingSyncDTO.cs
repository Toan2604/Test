using System.Collections.Generic;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_StoreScoutingSyncDTO
    {
        public List<MobileSync_StoreScoutingDTO> Created { get; set; }
        public List<MobileSync_StoreScoutingDTO> Updated { get; set; }
        public List<MobileSync_StoreScoutingDTO> Deleted { get; set; }
    }

    public class MobileSync_StoreScoutingDTO
    {

    }
}
