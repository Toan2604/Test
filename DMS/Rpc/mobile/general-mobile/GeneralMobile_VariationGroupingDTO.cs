using DMS.Common; 
using TrueSight.Common;
using DMS.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_VariationGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProductId { get; set; }
        public List<GeneralMobile_VariationDTO> Variations { get; set; }
        public GeneralMobile_VariationGroupingDTO() { }
        public GeneralMobile_VariationGroupingDTO(VariationGrouping VariationGrouping)
        {
            this.Id = VariationGrouping.Id;
            this.Name = VariationGrouping.Name;
            this.ProductId = VariationGrouping.ProductId;
            this.Variations = VariationGrouping.Variations?.Select(x => new GeneralMobile_VariationDTO(x)).ToList();
        }
    }

    public class GeneralMobile_VariationGroupingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ProductId { get; set; }

        public VariationGroupingOrder OrderBy { get; set; }
    }
}