using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class SurveyResultSingleDAO
    {
        public long SurveyResultId { get; set; }
        public long SurveyQuestionId { get; set; }
        public long SurveyOptionId { get; set; }

        public virtual SurveyOptionDAO SurveyOption { get; set; }
        public virtual SurveyQuestionDAO SurveyQuestion { get; set; }
        public virtual SurveyResultDAO SurveyResult { get; set; }
    }
}
