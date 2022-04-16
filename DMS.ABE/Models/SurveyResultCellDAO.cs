using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class SurveyResultCellDAO
    {
        public long SurveyResultId { get; set; }
        public long SurveyQuestionId { get; set; }
        public long RowOptionId { get; set; }
        public long ColumnOptionId { get; set; }

        public virtual SurveyOptionDAO ColumnOption { get; set; }
        public virtual SurveyOptionDAO RowOption { get; set; }
        public virtual SurveyQuestionDAO SurveyQuestion { get; set; }
        public virtual SurveyResultDAO SurveyResult { get; set; }
    }
}
