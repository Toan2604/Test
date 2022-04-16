using System;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_checker
{
    public class MonitorStoreChecker_StoreCheckingImageMappingDTO : DataDTO
    {
        public long ImageId { get; set; }
        public long StoreCheckingId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime ShootingAt { get; set; }
        public decimal? Distance { get; set; }
        public MonitorStoreChecker_AlbumDTO Album { get; set; }
        public MonitorStoreChecker_AppUserDTO SaleEmployee { get; set; }
        public MonitorStoreChecker_ImageDTO Image { get; set; }
        public MonitorStoreChecker_StoreDTO Store { get; set; }
        public MonitorStoreChecker_StoreCheckingDTO StoreChecking { get; set; }
    }
}
