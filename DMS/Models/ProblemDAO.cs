using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ProblemDAO
    {
        public ProblemDAO()
        {
            ProblemHistories = new HashSet<ProblemHistoryDAO>();
            ProblemImageMappings = new HashSet<ProblemImageMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public long? StoreCheckingId { get; set; }
        public long StoreId { get; set; }
        public long CreatorId { get; set; }
        public long ProblemTypeId { get; set; }
        public DateTime NoteAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Content { get; set; }
        public long ProblemStatusId { get; set; }
        public Guid RowId { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual ProblemStatusDAO ProblemStatus { get; set; }
        public virtual ProblemTypeDAO ProblemType { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual StoreCheckingDAO StoreChecking { get; set; }
        public virtual ICollection<ProblemHistoryDAO> ProblemHistories { get; set; }
        public virtual ICollection<ProblemImageMappingDAO> ProblemImageMappings { get; set; }
    }
}
