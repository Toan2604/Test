using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_PromotionPolicyDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Promotion_PromotionPromotionPolicyMappingDTO> PromotionPromotionPolicyMappings { get; set; }
        public List<Promotion_PromotionDirectSalesOrderDTO> PromotionDirectSalesOrders { get; set; }
        public List<Promotion_PromotionStoreDTO> PromotionStores { get; set; }
        public List<Promotion_PromotionStoreGroupingDTO> PromotionStoreGroupings { get; set; }
        public List<Promotion_PromotionStoreTypeDTO> PromotionStoreTypes { get; set; }
        public List<Promotion_PromotionProductDTO> PromotionProducts { get; set; }
        public List<Promotion_PromotionProductGroupingDTO> PromotionProductGroupings { get; set; }
        public List<Promotion_PromotionProductTypeDTO> PromotionProductTypes { get; set; }
        public List<Promotion_PromotionComboDTO> PromotionCombos { get; set; }
        public List<Promotion_PromotionSamePriceDTO> PromotionSamePrices { get; set; }

        public Promotion_PromotionPolicyDTO() { }
        public Promotion_PromotionPolicyDTO(PromotionPolicy PromotionPolicy)
        {
            this.Id = PromotionPolicy.Id;
            this.Code = PromotionPolicy.Code;
            this.Name = PromotionPolicy.Name;
            this.PromotionPromotionPolicyMappings = PromotionPolicy.PromotionPromotionPolicyMappings?.Select(x => new Promotion_PromotionPromotionPolicyMappingDTO(x)).ToList();
            this.PromotionDirectSalesOrders = PromotionPolicy.PromotionDirectSalesOrders?.Select(x => new Promotion_PromotionDirectSalesOrderDTO(x)).ToList();
            this.PromotionStores = PromotionPolicy.PromotionStores?.Select(x => new Promotion_PromotionStoreDTO(x)).ToList();
            this.PromotionStoreGroupings = PromotionPolicy.PromotionStoreGroupings?.Select(x => new Promotion_PromotionStoreGroupingDTO(x)).ToList();
            this.PromotionStoreTypes = PromotionPolicy.PromotionStoreTypes?.Select(x => new Promotion_PromotionStoreTypeDTO(x)).ToList();
            this.PromotionProducts = PromotionPolicy.PromotionProducts?.Select(x => new Promotion_PromotionProductDTO(x)).ToList();
            this.PromotionProductGroupings = PromotionPolicy.PromotionProductGroupings?.Select(x => new Promotion_PromotionProductGroupingDTO(x)).ToList();
            this.PromotionProductTypes = PromotionPolicy.PromotionProductTypes?.Select(x => new Promotion_PromotionProductTypeDTO(x)).ToList();
            this.PromotionCombos = PromotionPolicy.PromotionCombos?.Select(x => new Promotion_PromotionComboDTO(x)).ToList();
            this.PromotionSamePrices = PromotionPolicy.PromotionSamePrices?.Select(x => new Promotion_PromotionSamePriceDTO(x)).ToList();
            this.Errors = PromotionPolicy.Errors;
        }
    }

    public class Promotion_PromotionPolicyFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public PromotionPolicyOrder OrderBy { get; set; }
    }
}