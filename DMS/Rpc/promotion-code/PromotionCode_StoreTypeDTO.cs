using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ColorId { get; set; }

        public long StatusId { get; set; }

        public bool Used { get; set; }


        public PromotionCode_StoreTypeDTO() { }
        public PromotionCode_StoreTypeDTO(StoreType StoreType)
        {

            this.Id = StoreType.Id;

            this.Code = StoreType.Code;

            this.Name = StoreType.Name;

            this.ColorId = StoreType.ColorId;

            this.StatusId = StoreType.StatusId;

            this.Used = StoreType.Used;

            this.Errors = StoreType.Errors;
        }
    }

    public class PromotionCode_StoreTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ColorId { get; set; }

        public IdFilter StatusId { get; set; }

        public StoreTypeOrder OrderBy { get; set; }
    }
}