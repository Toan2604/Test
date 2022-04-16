using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class AlbumDAO
    {
        public AlbumDAO()
        {
            AlbumImageMappings = new HashSet<AlbumImageMappingDAO>();
            StoreCheckingImageMappings = new HashSet<StoreCheckingImageMappingDAO>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<AlbumImageMappingDAO> AlbumImageMappings { get; set; }
        public virtual ICollection<StoreCheckingImageMappingDAO> StoreCheckingImageMappings { get; set; }
    }
}
