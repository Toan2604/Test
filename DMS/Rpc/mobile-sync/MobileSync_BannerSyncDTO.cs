using System.Collections.Generic;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_BannerSyncDTO
    {
        public List<MobileSync_BannerDTO> Created { get; set; }
        public List<MobileSync_BannerDTO> Updated { get; set; }
        public List<MobileSync_BannerDTO> Deleted { get; set; }
    }


}
