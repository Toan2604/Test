using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_ShowingItemDAO
    {
        public long ShowingItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ShowingCategoryId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public string ERPCode { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
    }
}
