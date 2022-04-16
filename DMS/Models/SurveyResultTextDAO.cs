using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SurveyResultTextDAO
    {
        public long SurveyResultId { get; set; }
        public long SurveyQuestionId { get; set; }
        public string Content { get; set; }

        public virtual SurveyQuestionDAO SurveyQuestion { get; set; }
        public virtual SurveyResultDAO SurveyResult { get; set; }
    }
}
