using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SurveyQuestionImageMappingDTO
    {
        public long SurveyQuestionId { get; set; }
        public long ImageId { get; set; }
        public GeneralMobile_ImageDTO Image { get; set; }
        public GeneralMobile_SurveyQuestionDTO SurveyQuestion { get; set; }

        public GeneralMobile_SurveyQuestionImageMappingDTO() { }

        public GeneralMobile_SurveyQuestionImageMappingDTO(SurveyQuestionImageMapping SurveyQuestionImageMapping)
        {
            this.SurveyQuestionId = SurveyQuestionImageMapping.SurveyQuestionId;
            this.ImageId = SurveyQuestionImageMapping.ImageId;
            this.Image = SurveyQuestionImageMapping.Image == null ? null : new GeneralMobile_ImageDTO(SurveyQuestionImageMapping.Image);
        }
    }
}
