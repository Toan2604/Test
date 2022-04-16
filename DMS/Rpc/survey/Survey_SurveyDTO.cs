using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.survey
{
    public class Survey_SurveyDTO : DataDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Used { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public long ResultCounter { get; set; }
        public List<Survey_SurveyQuestionDTO> SurveyQuestions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Survey_AppUserDTO Creator { get; set; }
        public Survey_OrganizationDTO Organization { get; set; }
        public Survey_StatusDTO Status { get; set; }
        public Survey_SurveyDTO() { }
        public Survey_SurveyDTO(Survey Survey)
        {
            this.Id = Survey.Id;
            this.Title = Survey.Title;
            this.Description = Survey.Description;
            this.Used = Survey.Used;
            this.StartAt = Survey.StartAt;
            this.CreatorId = Survey.CreatorId;
            this.OrganizationId = Survey.OrganizationId;
            this.EndAt = Survey.EndAt;
            this.StatusId = Survey.StatusId;
            this.ResultCounter = Survey.ResultCounter;
            this.SurveyQuestions = Survey.SurveyQuestions?.Select(x => new Survey_SurveyQuestionDTO(x)).ToList();
            this.CreatedAt = Survey.CreatedAt;
            this.UpdatedAt = Survey.UpdatedAt;
            this.Creator = Survey.Creator == null ? null : new Survey_AppUserDTO(Survey.Creator);
            this.Organization = Survey.Organization == null ? null : new Survey_OrganizationDTO(Survey.Organization);
            this.Status = Survey.Status == null ? null : new Survey_StatusDTO(Survey.Status);
            this.Errors = Survey.Errors;
        }
    }

    public class Survey_SurveyFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Title { get; set; }
        public StringFilter Description { get; set; }
        public DateFilter StartAt { get; set; }
        public DateFilter EndAt { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public IdFilter CreatorId { get; set; }
        public SurveyOrder OrderBy { get; set; }
    }
}
