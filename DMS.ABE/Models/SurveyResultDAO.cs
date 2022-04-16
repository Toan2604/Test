using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class SurveyResultDAO
    {
        public SurveyResultDAO()
        {
            SurveyResultCells = new HashSet<SurveyResultCellDAO>();
            SurveyResultSingles = new HashSet<SurveyResultSingleDAO>();
            SurveyResultTexts = new HashSet<SurveyResultTextDAO>();
        }

        public long Id { get; set; }
        public long SurveyId { get; set; }
        public long OrganizationId { get; set; }
        public long AppUserId { get; set; }
        public long? StoreScoutingId { get; set; }
        public long? StoreId { get; set; }
        public DateTime Time { get; set; }
        public Guid RowId { get; set; }
        public long SurveyRespondentTypeId { get; set; }
        public string RespondentName { get; set; }
        public string RespondentPhone { get; set; }
        public string RespondentEmail { get; set; }
        public string RespondentAddress { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual StoreScoutingDAO StoreScouting { get; set; }
        public virtual SurveyDAO Survey { get; set; }
        public virtual SurveyRespondentTypeDAO SurveyRespondentType { get; set; }
        public virtual ICollection<SurveyResultCellDAO> SurveyResultCells { get; set; }
        public virtual ICollection<SurveyResultSingleDAO> SurveyResultSingles { get; set; }
        public virtual ICollection<SurveyResultTextDAO> SurveyResultTexts { get; set; }
    }
}
