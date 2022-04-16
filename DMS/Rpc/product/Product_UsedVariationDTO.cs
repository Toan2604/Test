using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    public class Product_UsedVariationDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public Product_UsedVariationDTO() { }
        public Product_UsedVariationDTO(UsedVariation UsedVariation)
        {

            this.Id = UsedVariation.Id;

            this.Code = UsedVariation.Code;

            this.Name = UsedVariation.Name;

        }
    }

    public class Product_UsedVariationFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public UsedVariationOrder OrderBy { get; set; }
    }
}
