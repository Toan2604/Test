using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SurveyQuestionFileMappingDAO
    {
        public long SurveyQuestionId { get; set; }
        public long FileId { get; set; }

        public virtual FileDAO File { get; set; }
        public virtual SurveyQuestionDAO SurveyQuestion { get; set; }
    }
}
