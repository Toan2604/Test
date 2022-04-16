using DMS.Entities;

namespace DMS.Rpc.survey
{
    public class Survey_SurveyQuestionImageMappingDTO
    {
        public long SurveyQuestionId { get; set; }
        public long ImageId { get; set; }
        public Survey_ImageDTO Image { get; set; }
        public Survey_SurveyQuestionDTO SurveyQuestion { get; set; }

        public Survey_SurveyQuestionImageMappingDTO() { }

        public Survey_SurveyQuestionImageMappingDTO(SurveyQuestionImageMapping SurveyQuestionImageMapping)
        {
            this.SurveyQuestionId = SurveyQuestionImageMapping.SurveyQuestionId;
            this.ImageId = SurveyQuestionImageMapping.ImageId;
            this.Image = SurveyQuestionImageMapping.Image == null ? null : new Survey_ImageDTO(SurveyQuestionImageMapping.Image);
        }
    }
}
