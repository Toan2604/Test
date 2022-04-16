using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_albums
{
    public class MonitorStoreAlbum_StoreCheckingDTO : DataDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public DateTime? CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
        public MonitorStoreAlbum_AppUserDTO SaleEmployee { get; set; }
        public MonitorStoreAlbum_StoreDTO Store { get; set; }
        public List<MonitorStoreAlbum_StoreCheckingImageMappingDTO> StoreCheckingImageMappings { get; set; }
        public MonitorStoreAlbum_StoreCheckingDTO() { }
        public MonitorStoreAlbum_StoreCheckingDTO(StoreChecking StoreChecking)
        {
            this.Id = StoreChecking.Id;
            this.StoreId = StoreChecking.StoreId;
            this.SaleEmployeeId = StoreChecking.SaleEmployeeId;
            this.Longitude = StoreChecking.Longitude;
            this.Latitude = StoreChecking.Latitude;
            this.CheckInAt = StoreChecking.CheckInAt;
            this.CheckOutAt = StoreChecking.CheckOutAt;
            this.SaleEmployee = StoreChecking.SaleEmployee == null ? null : new MonitorStoreAlbum_AppUserDTO(StoreChecking.SaleEmployee);
            this.Store = StoreChecking.Store == null ? null : new MonitorStoreAlbum_StoreDTO(StoreChecking.Store);
            this.StoreCheckingImageMappings = StoreChecking.StoreCheckingImageMappings?.Select(x => new MonitorStoreAlbum_StoreCheckingImageMappingDTO(x)).ToList();
            this.Errors = StoreChecking.Errors;
        }
    }
}
