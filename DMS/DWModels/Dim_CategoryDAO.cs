using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_CategoryDAO
    {
        public long CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? CategoryParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public long? ImageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
    }
}
