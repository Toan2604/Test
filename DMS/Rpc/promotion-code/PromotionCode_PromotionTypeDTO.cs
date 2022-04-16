using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_PromotionTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public PromotionCode_PromotionTypeDTO() { }
        public PromotionCode_PromotionTypeDTO(PromotionType PromotionType)
        {

            this.Id = PromotionType.Id;

            this.Code = PromotionType.Code;

            this.Name = PromotionType.Name;

            this.Errors = PromotionType.Errors;
        }
    }

    public class PromotionCode_PromotionTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public PromotionTypeOrder OrderBy { get; set; }
    }
}