using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.survey
{
    public class Survey_SurveyQuestionTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Survey_SurveyQuestionTypeDTO() { }
        public Survey_SurveyQuestionTypeDTO(SurveyQuestionType SurveyQuestionType)
        {

            this.Id = SurveyQuestionType.Id;

            this.Code = SurveyQuestionType.Code;

            this.Name = SurveyQuestionType.Name;

            this.Errors = SurveyQuestionType.Errors;
        }
    }

    public class Survey_SurveyQuestionTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public SurveyQuestionTypeOrder OrderBy { get; set; }
    }
}