using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_ProblemTypeDAO
    {
        public long ProblemTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
    }
}
