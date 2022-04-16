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
    public class BrandInStoreShowingCategoryMapping : DataEntity, IEquatable<BrandInStoreShowingCategoryMapping>
    {
        public long BrandInStoreId { get; set; }
        public long ShowingCategoryId { get; set; }

        public BrandInStore BrandInStore { get; set; }
        public ShowingCategory ShowingCategory { get; set; }

        public bool Equals(BrandInStoreShowingCategoryMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class BrandInStoreShowingCategoryMappingFilter : FilterEntity
    {
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter ShowingCategoryId { get; set; }
        public List<BrandInStoreShowingCategoryMappingFilter> OrFilter { get; set; }
        public BrandInStoreShowingCategoryMappingOrder OrderBy { get; set; }
        public BrandInStoreShowingCategoryMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrandInStoreShowingCategoryMappingOrder
    {
        BrandInStore = 0,
        ShowingCategory = 1,
    }

    [Flags]
    public enum BrandInStoreShowingCategoryMappingSelect : long
    {
        ALL = E.ALL,
        BrandInStore = E._0,
        ShowingCategory = E._1,
    }
}
