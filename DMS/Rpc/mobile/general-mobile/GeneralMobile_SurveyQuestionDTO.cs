using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SurveyQuestionDTO : DataDTO
    {
        public long Id { get; set; }
        public long SurveyId { get; set; }
        public string Content { get; set; }
        public long SurveyQuestionTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public GeneralMobile_SurveyQuestionTypeDTO SurveyQuestionType { get; set; }
        public List<GeneralMobile_SurveyOptionDTO> SurveyOptions { get; set; }
        public Dictionary<long, Dictionary<long, bool>> TableResult { get; set; }
        public Dictionary<long, bool> ListResult { get; set; }
        public List<GeneralMobile_SurveyQuestionImageMappingDTO> SurveyQuestionImageMappings { get; set; }
        public List<GeneralMobile_SurveyQuestionFileMappingDTO> SurveyQuestionFileMappings { get; set; }
        public string TextResult { get; set; }
        public GeneralMobile_SurveyQuestionDTO() { }
        public GeneralMobile_SurveyQuestionDTO(SurveyQuestion SurveyQuestion)
        {
            this.Id = SurveyQuestion.Id;
            this.SurveyId = SurveyQuestion.SurveyId;
            this.Content = SurveyQuestion.Content;
            this.SurveyQuestionTypeId = SurveyQuestion.SurveyQuestionTypeId;
            this.IsMandatory = SurveyQuestion.IsMandatory;
            this.TableResult = SurveyQuestion.TableResult;
            this.ListResult = SurveyQuestion.ListResult;
            this.TextResult = SurveyQuestion.TextResult;
            this.SurveyQuestionType = SurveyQuestion.SurveyQuestionType == null ? null : new GeneralMobile_SurveyQuestionTypeDTO(SurveyQuestion.SurveyQuestionType);
            this.SurveyOptions = SurveyQuestion.SurveyOptions?.Select(so => new GeneralMobile_SurveyOptionDTO(so)).ToList();
            this.SurveyQuestionImageMappings = SurveyQuestion.SurveyQuestionImageMappings == null ? null : SurveyQuestion.SurveyQuestionImageMappings.Select(x => new GeneralMobile_SurveyQuestionImageMappingDTO(x)).ToList();
            this.SurveyQuestionFileMappings = SurveyQuestion.SurveyQuestionFileMappings == null ? null : SurveyQuestion.SurveyQuestionFileMappings.Select(x => new GeneralMobile_SurveyQuestionFileMappingDTO(x)).ToList();
            this.Errors = SurveyQuestion.Errors;
        }
    }

    public class GeneralMobile_SurveyQuestionFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter SurveyId { get; set; }

        public StringFilter Content { get; set; }

        public IdFilter SurveyQuestionTypeId { get; set; }

        public SurveyQuestionOrder OrderBy { get; set; }
    }
}