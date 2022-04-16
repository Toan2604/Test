using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class SurveyOptionDAO
    {
        public SurveyOptionDAO()
        {
            SurveyResultCellColumnOptions = new HashSet<SurveyResultCellDAO>();
            SurveyResultCellRowOptions = new HashSet<SurveyResultCellDAO>();
            SurveyResultSingles = new HashSet<SurveyResultSingleDAO>();
        }

        public long Id { get; set; }
        public long SurveyQuestionId { get; set; }
        public long SurveyOptionTypeId { get; set; }
        public string Content { get; set; }

        public virtual SurveyOptionTypeDAO SurveyOptionType { get; set; }
        public virtual SurveyQuestionDAO SurveyQuestion { get; set; }
        public virtual ICollection<SurveyResultCellDAO> SurveyResultCellColumnOptions { get; set; }
        public virtual ICollection<SurveyResultCellDAO> SurveyResultCellRowOptions { get; set; }
        public virtual ICollection<SurveyResultSingleDAO> SurveyResultSingles { get; set; }
    }
}
