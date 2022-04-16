using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Entities
{
    public class EstimatedRevenue : DataEntity, IEquatable<EstimatedRevenue>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool Equals(EstimatedRevenue other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class EstimatedRevenueFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<EstimatedRevenueFilter> OrFilter { get; set; }
        public EstimatedRevenueOrder OrderBy { get; set; }
        public EstimatedRevenueSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EstimatedRevenueOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum EstimatedRevenueSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
