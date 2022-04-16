using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ProblemImageMappingDAO
    {
        public long ProblemId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual ProblemDAO Problem { get; set; }
    }
}
