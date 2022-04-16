using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_checked
{
    public class ReportStoreChecked_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long StatusId { get; set; }


        public ReportStoreChecked_StoreTypeDTO() { }
        public ReportStoreChecked_StoreTypeDTO(StoreType StoreType)
        {

            this.Id = StoreType.Id;

            this.Code = StoreType.Code;

            this.Name = StoreType.Name;

            this.StatusId = StoreType.StatusId;

        }
    }

    public class ReportStoreChecked_StoreTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter StatusId { get; set; }

        public StoreTypeOrder OrderBy { get; set; }
    }
}