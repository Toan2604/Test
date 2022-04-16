using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class BrandInStoreShowingCategoryMappingDAO
    {
        public long BrandInStoreId { get; set; }
        public long ShowingCategoryId { get; set; }

        public virtual BrandInStoreDAO BrandInStore { get; set; }
        public virtual ShowingCategoryDAO ShowingCategory { get; set; }
    }
}
