using System;

namespace DMS.Models
{
    public partial class BrandInStoreProductGroupingMappingDAO : IEquatable<BrandInStoreProductGroupingMappingDAO>
    {
        public bool Equals(BrandInStoreProductGroupingMappingDAO other)
        {
            if (other == null) return false;
            if (other.BrandInStoreId == this.BrandInStoreId) return false;
            if (other.ProductGroupingId == this.ProductGroupingId) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return BrandInStoreId.GetHashCode() ^ ProductGroupingId.GetHashCode();
        }
    }
}
