using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SurveyQuestionTypeDAO
    {
        public SurveyQuestionTypeDAO()
        {
            SurveyQuestions = new HashSet<SurveyQuestionDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SurveyQuestionDAO> SurveyQuestions { get; set; }
    }
}
