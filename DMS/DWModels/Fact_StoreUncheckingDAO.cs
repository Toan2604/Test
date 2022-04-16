using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_StoreUncheckingDAO
    {
        public long StoreUncheckingId { get; set; }
        public long OrganizationId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public DateTime Date { get; set; }
    }
}
