using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_StoreCheckingDAO
    {
        public long StoreCheckingId { get; set; }
        public long StoreId { get; set; }
        public long OrganizationId { get; set; }
        public long SaleEmployeeId { get; set; }
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
        public bool? Checked { get; set; }
    }
}
