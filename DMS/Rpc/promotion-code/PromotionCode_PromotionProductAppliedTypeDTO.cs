using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_PromotionProductAppliedTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public PromotionCode_PromotionProductAppliedTypeDTO() { }
        public PromotionCode_PromotionProductAppliedTypeDTO(PromotionProductAppliedType PromotionProductAppliedType)
        {

            this.Id = PromotionProductAppliedType.Id;

            this.Code = PromotionProductAppliedType.Code;

            this.Name = PromotionProductAppliedType.Name;

            this.Errors = PromotionProductAppliedType.Errors;
        }
    }

    public class PromotionCode_PromotionProductAppliedTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public PromotionProductAppliedTypeOrder OrderBy { get; set; }
    }
}