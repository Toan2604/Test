using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public Promotion_PromotionTypeDTO() { }
        public Promotion_PromotionTypeDTO(PromotionType PromotionType)
        {

            this.Id = PromotionType.Id;

            this.Code = PromotionType.Code;

            this.Name = PromotionType.Name;

            this.Errors = PromotionType.Errors;
        }
    }

    public class Promotion_PromotionTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public PromotionTypeOrder OrderBy { get; set; }
    }
}