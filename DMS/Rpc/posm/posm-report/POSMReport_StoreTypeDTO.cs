using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    public class POSMReport_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ColorId { get; set; }
        public long StatusId { get; set; }

        public POSMReport_StoreTypeDTO() { }
        public POSMReport_StoreTypeDTO(StoreType StoreType)
        {

            this.Id = StoreType.Id;

            this.Code = StoreType.Code;

            this.Name = StoreType.Name;

            this.ColorId = StoreType.ColorId;
            this.StatusId = StoreType.StatusId;

        }
    }

    public class POSMReport_StoreTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter StatusId { get; set; }

        public StoreTypeOrder OrderBy { get; set; }
    }
}