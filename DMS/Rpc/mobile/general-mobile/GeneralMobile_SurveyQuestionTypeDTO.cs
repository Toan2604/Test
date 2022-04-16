using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SurveyQuestionTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public GeneralMobile_SurveyQuestionTypeDTO() { }
        public GeneralMobile_SurveyQuestionTypeDTO(SurveyQuestionType SurveyQuestionType)
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