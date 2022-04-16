using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.product
{
    public class Product_VariationDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long VariationGroupingId { get; set; }
        public Product_VariationDTO() { }
        public Product_VariationDTO(Variation Variation)
        {
            this.Id = Variation.Id;
            this.Code = Variation.Code;
            this.Name = Variation.Name;
            this.VariationGroupingId = Variation.VariationGroupingId;
        }
    }

    public class Product_VariationFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter VariationGroupingId { get; set; }
        public VariationOrder OrderBy { get; set; }
    }
}
