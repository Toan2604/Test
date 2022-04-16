using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SurveyQuestionDAO
    {
        public SurveyQuestionDAO()
        {
            SurveyOptions = new HashSet<SurveyOptionDAO>();
            SurveyQuestionFileMappings = new HashSet<SurveyQuestionFileMappingDAO>();
            SurveyQuestionImageMappings = new HashSet<SurveyQuestionImageMappingDAO>();
            SurveyResultCells = new HashSet<SurveyResultCellDAO>();
            SurveyResultSingles = new HashSet<SurveyResultSingleDAO>();
            SurveyResultTexts = new HashSet<SurveyResultTextDAO>();
        }

        public long Id { get; set; }
        public long SurveyId { get; set; }
        public string Content { get; set; }
        public long SurveyQuestionTypeId { get; set; }
        public bool IsMandatory { get; set; }

        public virtual SurveyDAO Survey { get; set; }
        public virtual SurveyQuestionTypeDAO SurveyQuestionType { get; set; }
        public virtual ICollection<SurveyOptionDAO> SurveyOptions { get; set; }
        public virtual ICollection<SurveyQuestionFileMappingDAO> SurveyQuestionFileMappings { get; set; }
        public virtual ICollection<SurveyQuestionImageMappingDAO> SurveyQuestionImageMappings { get; set; }
        public virtual ICollection<SurveyResultCellDAO> SurveyResultCells { get; set; }
        public virtual ICollection<SurveyResultSingleDAO> SurveyResultSingles { get; set; }
        public virtual ICollection<SurveyResultTextDAO> SurveyResultTexts { get; set; }
    }
}
