using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SurveyOptionDTO : DataDTO
    {
        public long Id { get; set; }
        public long SurveyId { get; set; }
        public string Content { get; set; }
        public long SurveyOptionTypeId { get; set; }
        public GeneralMobile_SurveyOptionTypeDTO SurveyOptionType { get; set; }

        public GeneralMobile_SurveyOptionDTO() { }
        public GeneralMobile_SurveyOptionDTO(SurveyOption SurveyOption)
        {
            this.Id = SurveyOption.Id;
            this.Content = SurveyOption.Content;
            this.SurveyOptionTypeId = SurveyOption.SurveyOptionTypeId;
            this.SurveyOptionType = SurveyOption.SurveyOptionType == null ? null : new GeneralMobile_SurveyOptionTypeDTO(SurveyOption.SurveyOptionType);
            this.Errors = SurveyOption.Errors;
        }
    }

    public class GeneralMobile_SurveyOptionFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter SurveyId { get; set; }

        public StringFilter Content { get; set; }

        public IdFilter SurveyOptionTypeId { get; set; }

        public SurveyOptionOrder OrderBy { get; set; }
    }
}