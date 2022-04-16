using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_unchecked
{
    public class ReportStoreUnchecked_SaleEmployeeDTO : DataDTO
    {
        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<ReportStoreUnchecked_StoreDTO> Stores { get; set; }
    }

    public class ReportStoreUnchecked_StoreDTO : DataDTO, IEquatable<ReportStoreUnchecked_StoreDTO>
    {
        public long STT { get; set; }
        public DateTime Date { get; set; }
        public string DateDisplay { get { return Date.ToString("dd-MM-yyyy"); } }
        public long AppUserId { get; set; }
        public string StoreCode { get; set; }
        public string StoreCodeDraft { get; set; }
        public string StoreName { get; set; }
        public string StoreStatusName { get; set; }
        public string StoreTypeName { get; set; }
        public string StorePhone { get; set; }
        public string StoreAddress { get; set; }

        public bool Equals(ReportStoreUnchecked_StoreDTO other)
        {
            if (other == null)
                return false;
            return Date == other.Date &&
                 AppUserId == other.AppUserId &&
                 StoreCode == other.StoreCode;
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode() ^ AppUserId.GetHashCode() ^ StoreCode.GetHashCode();
        }
    }
}
