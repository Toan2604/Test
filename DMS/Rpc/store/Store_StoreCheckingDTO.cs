using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreCheckingDTO : DataDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public DateTime? CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
        public long? CountIndirectSalesOrder { get; set; }
        public long? CountImage { get; set; }
        public Store_AppUserDTO SaleEmployee { get; set; }
        public Store_StoreCheckingDTO() { }
        public Store_StoreCheckingDTO(StoreChecking StoreChecking)
        {
            this.Id = StoreChecking.Id;
            this.StoreId = StoreChecking.StoreId;
            this.SaleEmployeeId = StoreChecking.SaleEmployeeId;
            this.Longitude = StoreChecking.Longitude;
            this.Latitude = StoreChecking.Latitude;
            this.CheckInAt = StoreChecking.CheckInAt;
            this.CheckOutAt = StoreChecking.CheckOutAt;
            this.CountIndirectSalesOrder = StoreChecking.CountIndirectSalesOrder;
            this.CountImage = StoreChecking.ImageCounter;
            this.SaleEmployee = StoreChecking.SaleEmployee == null ? null : new Store_AppUserDTO(StoreChecking.SaleEmployee);
            this.Errors = StoreChecking.Errors;
        }
    }
}
