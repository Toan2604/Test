using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class SurveyQuestionImageMappingDAO
    {
        public long SurveyQuestionId { get; set; }
        public long ImageId { get; set; }

        public virtual ImageDAO Image { get; set; }
        public virtual SurveyQuestionDAO SurveyQuestion { get; set; }
    }
}
