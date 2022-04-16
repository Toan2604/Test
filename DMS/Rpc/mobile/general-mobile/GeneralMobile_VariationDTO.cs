using DMS.Common; 
using TrueSight.Common;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_VariationDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long VariationGroupingId { get; set; }
        public GeneralMobile_VariationDTO() { }
        public GeneralMobile_VariationDTO(Variation Variation)
        {
            this.Id = Variation.Id;
            this.Code = Variation.Code;
            this.Name = Variation.Name;
            this.VariationGroupingId = Variation.VariationGroupingId;
        }
    }

    public class GeneralMobile_VariationFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter VariationGroupingId { get; set; }
        public VariationOrder OrderBy { get; set; }
    }
}
