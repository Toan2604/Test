using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_StoreTypeDAO
    {
        public long StoreTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ColorId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
