using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    /// <summary>
    /// Danh s&#225;ch c&#225;c th&#432;&#417;ng hi&#7879;u trong 1 c&#7917;a h&#224;ng
    /// </summary>
    public partial class Fact_BrandInStoreDAO
    {
        public long BrandInStoreId { get; set; }
        public long StoreId { get; set; }
        public long BrandId { get; set; }
        public long Top { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
    }
}
