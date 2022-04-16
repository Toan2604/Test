using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class LuckyDrawDAO
    {
        public LuckyDrawDAO()
        {
            LuckyDrawRegistrations = new HashSet<LuckyDrawRegistrationDAO>();
            LuckyDrawStoreGroupingMappings = new HashSet<LuckyDrawStoreGroupingMappingDAO>();
            LuckyDrawStoreMappings = new HashSet<LuckyDrawStoreMappingDAO>();
            LuckyDrawStoreTypeMappings = new HashSet<LuckyDrawStoreTypeMappingDAO>();
            LuckyDrawStructures = new HashSet<LuckyDrawStructureDAO>();
            LuckyDrawWinners = new HashSet<LuckyDrawWinnerDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long LuckyDrawTypeId { get; set; }
        public long OrganizationId { get; set; }
        public decimal RevenuePerTurn { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public long AvatarImageId { get; set; }
        public long ImageId { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual ImageDAO AvatarImage { get; set; }
        public virtual ImageDAO Image { get; set; }
        public virtual LuckyDrawTypeDAO LuckyDrawType { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<LuckyDrawRegistrationDAO> LuckyDrawRegistrations { get; set; }
        public virtual ICollection<LuckyDrawStoreGroupingMappingDAO> LuckyDrawStoreGroupingMappings { get; set; }
        public virtual ICollection<LuckyDrawStoreMappingDAO> LuckyDrawStoreMappings { get; set; }
        public virtual ICollection<LuckyDrawStoreTypeMappingDAO> LuckyDrawStoreTypeMappings { get; set; }
        public virtual ICollection<LuckyDrawStructureDAO> LuckyDrawStructures { get; set; }
        public virtual ICollection<LuckyDrawWinnerDAO> LuckyDrawWinners { get; set; }
    }
}
