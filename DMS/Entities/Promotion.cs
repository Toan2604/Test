using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Promotion : DataEntity, IEquatable<Promotion>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long OrganizationId { get; set; }
        public long PromotionTypeId { get; set; }
        public string Note { get; set; }
        public long Priority { get; set; }
        public long StatusId { get; set; }
        public Organization Organization { get; set; }
        public PromotionType PromotionType { get; set; }
        public Status Status { get; set; }

        public List<PromotionDirectSalesOrder> PromotionDirectSalesOrders { get; set; }
        public List<PromotionStore> PromotionStores { get; set; }
        public List<PromotionStoreGrouping> PromotionStoreGroupings { get; set; }
        public List<PromotionStoreType> PromotionStoreTypes { get; set; }
        public List<PromotionProduct> PromotionProducts { get; set; }
        public List<PromotionProductGrouping> PromotionProductGroupings { get; set; }
        public List<PromotionProductType> PromotionProductTypes { get; set; }
        public List<PromotionSamePrice> PromotionSamePrices { get; set; }
        public List<PromotionCombo> PromotionCombos { get; set; }

        public List<PromotionPromotionPolicyMapping> PromotionPromotionPolicyMappings { get; set; }
        public List<PromotionStoreGroupingMapping> PromotionStoreGroupingMappings { get; set; }
        public List<PromotionStoreMapping> PromotionStoreMappings { get; set; }
        public List<PromotionStoreTypeMapping> PromotionStoreTypeMappings { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(Promotion other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PromotionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter PromotionTypeId { get; set; }
        public StringFilter Note { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<PromotionFilter> OrFilter { get; set; }
        public PromotionOrder OrderBy { get; set; }
        public PromotionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        StartDate = 3,
        EndDate = 4,
        Organization = 5,
        PromotionType = 6,
        Note = 7,
        Priority = 8,
        Status = 9,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum PromotionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        StartDate = E._3,
        EndDate = E._4,
        Organization = E._5,
        PromotionType = E._6,
        Note = E._7,
        Priority = E._8,
        Status = E._9,
    }
}
