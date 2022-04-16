using System;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_state_change
{
    public class ReportStoreStateChange_ReportStoreStateChangeDetailDTO : DataDTO
    {
        public long Stt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StringCreatedAt { get { return CreatedAt.ToString("dd-MM-yyyy"); } }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhoneNumber { get; set; }
        public string ApproverName { get; set; }
        public string PreviousStoreStatus { get; set; }
        public string StoreStatus { get; set; }
        public DateTime? PreviousCreatedAt { get; set; }
        public string StringPreviousCreatedAt { get { return PreviousCreatedAt?.ToString("dd-MM-yyyy"); } }
        public string OrganizationName { get; set; }
    }
}
