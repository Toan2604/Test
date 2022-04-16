using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SurveyRespondentTypeDAO
    {
        public SurveyRespondentTypeDAO()
        {
            SurveyResults = new HashSet<SurveyResultDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SurveyResultDAO> SurveyResults { get; set; }
    }
}
