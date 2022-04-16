using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblem_StoreCheckingDTO : DataDTO
    {

        public long Id { get; set; }

        public long StoreId { get; set; }

        public long SaleEmployeeId { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public DateTime? CheckInAt { get; set; }

        public DateTime? CheckOutAt { get; set; }

        public long? IndirectSalesOrderCounter { get; set; }

        public long? ImageCounter { get; set; }


        public MonitorStoreProblem_StoreCheckingDTO() { }
        public MonitorStoreProblem_StoreCheckingDTO(StoreChecking StoreChecking)
        {

            this.Id = StoreChecking.Id;

            this.StoreId = StoreChecking.StoreId;

            this.SaleEmployeeId = StoreChecking.SaleEmployeeId;

            this.Longitude = StoreChecking.Longitude;

            this.Latitude = StoreChecking.Latitude;

            this.CheckInAt = StoreChecking.CheckInAt;

            this.CheckOutAt = StoreChecking.CheckOutAt;

            this.Errors = StoreChecking.Errors;
        }
    }

    public class MonitorStoreProblem_StoreCheckingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter StoreId { get; set; }

        public IdFilter SaleEmployeeId { get; set; }

        public DecimalFilter Longitude { get; set; }

        public DecimalFilter Latitude { get; set; }

        public DateFilter CheckInAt { get; set; }

        public DateFilter CheckOutAt { get; set; }

        public LongFilter IndirectSalesOrderCounter { get; set; }

        public LongFilter ImageCounter { get; set; }

        public StoreCheckingOrder OrderBy { get; set; }
    }
}