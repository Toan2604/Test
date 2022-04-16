using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_statistic_problem
{
    public class ReportStatisticProblem_StoreDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long OrganizationId { get; set; }
        public long StoreTypeId { get; set; }
        public long? StoreStatusId { get; set; }
        public ReportStatisticProblem_StoreStatusDTO StoreStatus { get; set; }
        public List<ReportStatisticProblem_ContentDTO> Contents { get; set; }
        public ReportStatisticProblem_StoreDTO() { }
        public ReportStatisticProblem_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.CodeDraft = Store.CodeDraft;
            this.Name = Store.Name;
            this.Address = Store.Address;
            this.Phone = Store.Telephone;
            this.OrganizationId = Store.OrganizationId;
            this.StoreTypeId = Store.StoreTypeId;
        }
    }

    public class ReportStatisticProblem_StoreFilterDTO : FilterDTO
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
