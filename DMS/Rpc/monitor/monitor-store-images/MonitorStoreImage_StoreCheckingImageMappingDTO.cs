using System;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_images
{
    public class MonitorStoreImage_StoreCheckingImageMappingDTO : DataDTO
    {
        public long ImageId { get; set; }
        public long StoreCheckingId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime ShootingAt { get; set; }
        public decimal? Distance { get; set; }
        public MonitorStoreImage_AlbumDTO Album { get; set; }
        public MonitorStoreImage_AppUserDTO SaleEmployee { get; set; }
        public MonitorStoreImage_ImageDTO Image { get; set; }
        public MonitorStoreImage_StoreDTO Store { get; set; }
        public MonitorStoreImage_DetailDTO StoreChecking { get; set; }
    }
}
