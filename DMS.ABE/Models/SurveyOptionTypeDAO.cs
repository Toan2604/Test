using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class SurveyOptionTypeDAO
    {
        public SurveyOptionTypeDAO()
        {
            SurveyOptions = new HashSet<SurveyOptionDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SurveyOptionDAO> SurveyOptions { get; set; }
    }
}
