using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_state_change
{
    public class ReportStoreStateChange_StoreDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long OrganizationId { get; set; }
        public long StoreTypeId { get; set; }
        public long StoreStatusId { get; set; }
        public long StatusId { get; set; }
        public ReportStoreStateChange_OrganizationDTO Organization { get; set; }
        public ReportStoreStateChange_StoreStatusDTO StoreStatus { get; set; }
        public ReportStoreStateChange_StoreDTO() { }
        public ReportStoreStateChange_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.CodeDraft = Store.CodeDraft;
            this.Name = Store.Name;
            this.Address = Store.Address;
            this.Phone = Store.Telephone;
            this.OrganizationId = Store.OrganizationId;
            this.StoreTypeId = Store.StoreTypeId;
            this.StoreStatusId = Store.StoreStatusId;
            this.Organization = Store.Organization == null ? null : new ReportStoreStateChange_OrganizationDTO(Store.Organization);
            this.StoreStatus = Store.StoreStatus == null ? null : new ReportStoreStateChange_StoreStatusDTO(Store.StoreStatus);
            this.StatusId = Store.StatusId;
        }
    }

    public class ReportStoreStateChange_StoreFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }
        public StringFilter CodeDraft { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentStoreId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter StoreTypeId { get; set; }

        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StatusId { get; set; }
    }
}
