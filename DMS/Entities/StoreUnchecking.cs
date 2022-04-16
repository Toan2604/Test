using System;
using TrueSight.Common;

namespace DMS.Entities
{
    public partial class StoreUnchecking : DataEntity, IEquatable<StoreUnchecking>
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime Date { get; set; }
        public Store Store { get; set; }
        public Organization Organization { get; set; }
        public AppUser AppUser { get; set; }

        public bool Equals(StoreUnchecking other)
        {
            return other != null && Id == other.Id;
        }
    }
}
