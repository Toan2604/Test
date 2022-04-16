using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ProblemHistoryDAO
    {
        public long Id { get; set; }
        public long ProblemId { get; set; }
        public DateTime Time { get; set; }
        public long ModifierId { get; set; }
        public long ProblemStatusId { get; set; }

        public virtual AppUserDAO Modifier { get; set; }
        public virtual ProblemDAO Problem { get; set; }
        public virtual ProblemStatusDAO ProblemStatus { get; set; }
    }
}
