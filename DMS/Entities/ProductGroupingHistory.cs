using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ProductGroupingHistory : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ProductGroupingId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? AppUserId { get; set; }
        public long StatusId { get; set; }
        public ProductGrouping ProductGrouping { get; set; }
        public Status Status { get; set; }

        public bool Equals(ProductGroupingHistory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProductGroupingHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StatusId { get; set; }
        public ProductGroupingHistoryOrder OrderBy { get; set; }
        public ProductGroupingHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductGroupingHistoryOrder
    {
        Id = 0,
        ProductGrouping = 1,
        AppUser = 3,
        Status = 4,
        PreviousStatus = 5,
        CreatedAt = 50,
        PreviousCreatedAt = 52,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ProductGroupingHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ProductGrouping = E._1,
        AppUser = E._3,
        Status = E._4,
        PreviousStatus = E._5,
    }
}
