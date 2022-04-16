using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SurveyDAO
    {
        public SurveyDAO()
        {
            SurveyQuestions = new HashSet<SurveyQuestionDAO>();
            SurveyResults = new HashSet<SurveyResultDAO>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public long StatusId { get; set; }
        public long OrganizationId { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<SurveyQuestionDAO> SurveyQuestions { get; set; }
        public virtual ICollection<SurveyResultDAO> SurveyResults { get; set; }
    }
}
