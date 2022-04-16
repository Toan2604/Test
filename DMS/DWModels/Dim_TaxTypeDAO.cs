using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_TaxTypeDAO
    {
        public long TaxTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
    }
}
