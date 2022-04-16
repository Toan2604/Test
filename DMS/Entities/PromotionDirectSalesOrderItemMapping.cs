using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PromotionDirectSalesOrderItemMapping : DataEntity, IEquatable<PromotionDirectSalesOrderItemMapping>
    {
        public long PromotionDirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public Item Item { get; set; }
        public PromotionDirectSalesOrder PromotionDirectSalesOrder { get; set; }

        public bool Equals(PromotionDirectSalesOrderItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PromotionDirectSalesOrderItemMappingFilter : FilterEntity
    {
        public IdFilter PromotionDirectSalesOrderId { get; set; }
        public IdFilter ItemId { get; set; }
        public LongFilter Quantity { get; set; }
        public List<PromotionDirectSalesOrderItemMappingFilter> OrFilter { get; set; }
        public PromotionDirectSalesOrderItemMappingOrder OrderBy { get; set; }
        public PromotionDirectSalesOrderItemMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionDirectSalesOrderItemMappingOrder
    {
        PromotionDirectSalesOrder = 0,
        Item = 1,
        Quantity = 2,
    }

    [Flags]
    public enum PromotionDirectSalesOrderItemMappingSelect : long
    {
        ALL = E.ALL,
        PromotionDirectSalesOrder = E._0,
        Item = E._1,
        Quantity = E._2,
    }
}
