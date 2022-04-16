using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store.report_store_state_change
{
    public class ReportStoreStateChange_StoreStatusHistoryTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public ReportStoreStateChange_StoreStatusHistoryTypeDTO() { }
        public ReportStoreStateChange_StoreStatusHistoryTypeDTO(StoreStatusHistoryType StoreStatusHistoryType)
        {

            this.Id = StoreStatusHistoryType.Id;

            this.Code = StoreStatusHistoryType.Code;

            this.Name = StoreStatusHistoryType.Name;

        }
    }

    public class ReportStoreStateChange_StoreStatusHistoryTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}