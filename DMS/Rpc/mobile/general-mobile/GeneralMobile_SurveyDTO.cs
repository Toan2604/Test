using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SurveyDTO : DataDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RespondentAddress { get; set; }
        public string RespondentEmail { get; set; }
        public string RespondentName { get; set; }
        public string RespondentPhone { get; set; }
        public bool Used { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public long? StoreId { get; set; }
        public long? StoreScoutingId { get; set; }
        public long SurveyRespondentTypeId { get; set; }
        public long ResultCounter { get; set; }
        public List<GeneralMobile_SurveyQuestionDTO> SurveyQuestions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public GeneralMobile_AppUserDTO Creator { get; set; }
        public GeneralMobile_SurveyDTO() { }
        public GeneralMobile_SurveyDTO(Survey Survey)
        {
            this.Id = Survey.Id;
            this.Title = Survey.Title;
            this.Description = Survey.Description;
            this.Used = Survey.Used;
            this.StartAt = Survey.StartAt;
            this.CreatorId = Survey.CreatorId;
            this.EndAt = Survey.EndAt;
            this.StatusId = Survey.StatusId;
            this.StoreScoutingId = Survey.StoreScoutingId;
            this.RespondentAddress = Survey.RespondentAddress;
            this.RespondentEmail = Survey.RespondentEmail;
            this.RespondentName = Survey.RespondentName;
            this.RespondentPhone = Survey.RespondentPhone;
            this.SurveyRespondentTypeId = Survey.SurveyRespondentTypeId;
            this.SurveyQuestions = Survey.SurveyQuestions?.Select(x => new GeneralMobile_SurveyQuestionDTO(x)).ToList();
            this.CreatedAt = Survey.CreatedAt;
            this.UpdatedAt = Survey.UpdatedAt;
            this.Creator = Survey.Creator == null ? null : new GeneralMobile_AppUserDTO(Survey.Creator);
            this.Errors = Survey.Errors;
        }
    }

    public class GeneralMobile_SurveyFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Title { get; set; }
        public StringFilter Description { get; set; }
        public DateFilter StartAt { get; set; }
        public DateFilter EndAt { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter CreatorId { get; set; }
        public SurveyOrder OrderBy { get; set; }
    }
}
