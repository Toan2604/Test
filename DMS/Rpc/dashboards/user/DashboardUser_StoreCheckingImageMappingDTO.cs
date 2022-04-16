using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public class DashboardUser_StoreCheckingImageMappingDTO : DataDTO
    {
        public long Sum => StoreCheckingImageMappingHours.Sum(x => x.Counter);
        public List<DashboardUser_StoreCheckingImageMappingHourDTO> StoreCheckingImageMappingHours { get; set; }
    }

    public class DashboardUser_StoreCheckingImageMappingHourDTO : DataDTO
    {
        public string Hour { get; set; }
        public long Counter { get; set; }
    }
}
