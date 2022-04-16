using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_AddressMappingDAO
    {
        public long AddressMappingId { get; set; }
        public long? WardId { get; set; }
        public long? DistrictId { get; set; }
        public long? ProvinceId { get; set; }
        public long? CountryId { get; set; }
    }
}
