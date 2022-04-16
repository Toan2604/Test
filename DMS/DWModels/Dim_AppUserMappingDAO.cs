using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_AppUserMappingDAO
    {
        public long AppUserMappingId { get; set; }
        public long AppUserId { get; set; }
        public long? OrganizationId { get; set; }
    }
}
