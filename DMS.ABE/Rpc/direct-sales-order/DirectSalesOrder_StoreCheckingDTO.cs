using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_StoreCheckingDTO : DataDTO
    {

        public long Id { get; set; }

        public long StoreId { get; set; }

        public long SaleEmployeeId { get; set; }

        public long OrganizationId { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? CheckOutLongitude { get; set; }

        public decimal? CheckOutLatitude { get; set; }

        public DateTime? CheckInAt { get; set; }

        public DateTime? CheckOutAt { get; set; }

        public long? CheckInDistance { get; set; }

        public long? CheckOutDistance { get; set; }

        public long? IndirectSalesOrderCounter { get; set; }

        public long? ImageCounter { get; set; }

        public bool Planned { get; set; }

        public bool IsOpenedStore { get; set; }

        public string DeviceName { get; set; }


        public DirectSalesOrder_StoreCheckingDTO() { }
        public DirectSalesOrder_StoreCheckingDTO(StoreChecking StoreChecking)
        {

            this.Id = StoreChecking.Id;

            this.StoreId = StoreChecking.StoreId;

            this.SaleEmployeeId = StoreChecking.SaleEmployeeId;

            this.OrganizationId = StoreChecking.OrganizationId;

            this.Longitude = StoreChecking.Longitude;

            this.Latitude = StoreChecking.Latitude;

            this.CheckOutLongitude = StoreChecking.CheckOutLongitude;

            this.CheckOutLatitude = StoreChecking.CheckOutLatitude;

            this.CheckInAt = StoreChecking.CheckInAt;

            this.CheckOutAt = StoreChecking.CheckOutAt;

            this.CheckInDistance = StoreChecking.CheckInDistance;

            this.CheckOutDistance = StoreChecking.CheckOutDistance;

            this.ImageCounter = StoreChecking.ImageCounter;

            this.Planned = StoreChecking.Planned;

            this.IsOpenedStore = StoreChecking.IsOpenedStore;

            this.DeviceName = StoreChecking.DeviceName;

            this.Errors = StoreChecking.Errors;
        }
    }

    public class DirectSalesOrder_StoreCheckingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter StoreId { get; set; }

        public IdFilter SaleEmployeeId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public DecimalFilter Longitude { get; set; }

        public DecimalFilter Latitude { get; set; }

        public DecimalFilter CheckOutLongitude { get; set; }

        public DecimalFilter CheckOutLatitude { get; set; }

        public DateFilter CheckInAt { get; set; }

        public DateFilter CheckOutAt { get; set; }

        public LongFilter CheckInDistance { get; set; }

        public LongFilter CheckOutDistance { get; set; }

        public LongFilter IndirectSalesOrderCounter { get; set; }

        public LongFilter ImageCounter { get; set; }

        public StringFilter DeviceName { get; set; }

        public StoreCheckingOrder OrderBy { get; set; }
    }
}