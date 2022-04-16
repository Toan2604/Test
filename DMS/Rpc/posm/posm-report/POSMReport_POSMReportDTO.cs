using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    public class POSMReport_POSMReportDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<POSMReport_POSMStoreDTO> Stores { get; set; }
    }

    public class POSMReport_POSMStoreDTO : DataDTO
    {
        public long STT { get; set; }
        public long OrganizationId { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public List<POSMReport_POSMReportContentDTO> Contents { get; set; }
    }

    public class POSMReport_POSMReportContentDTO : DataDTO
    {
        public long ShowingItemId { get; set; }
        public string ShowingItemCode { get; set; }
        public string ShowingItemName { get; set; }
        public long UnitOfMeasureId { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public long PreOrderQuantity { get; set; }
        public long CurrentOrderQuantity { get; set; }
        public long OrderQuantity { get; set; }
        public long OrderWithDrawQuantity { get; set; }
        public decimal Amount { get; set; }
    }

    public class POSMReport_POSMReportFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public IdFilter ShowingItemId { get; set; }
        public DateFilter Date { get; set; }
        internal bool HasValue => (OrganizationId != null && OrganizationId.HasValue) ||
            (StoreId != null && StoreId.HasValue) ||
            (StoreGroupingId != null && StoreGroupingId.HasValue) ||
            (ShowingItemId != null && ShowingItemId.HasValue) ||
            (Date != null && Date.HasValue) ||
            (StoreTypeId != null && StoreTypeId.HasValue);
    }
}
