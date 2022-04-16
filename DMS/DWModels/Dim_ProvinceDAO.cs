using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_ProvinceDAO
    {
        public long ProvinceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
    }
}
