using DMS.Entities;
namespace DMS.Rpc.survey
{
    public class Survey_SurveyQuestionFileMappingDTO
    {
        public long SurveyQuestionId { get; set; }
        public long FileId { get; set; }
        public Survey_FileDTO File { get; set; }
        public Survey_SurveyQuestionDTO SurveyQuestion { get; set; }

        public Survey_SurveyQuestionFileMappingDTO() { }

        public Survey_SurveyQuestionFileMappingDTO(SurveyQuestionFileMapping SurveyQuestionFileMapping)
        {
            this.SurveyQuestionId = SurveyQuestionFileMapping.SurveyQuestionId;
            this.FileId = SurveyQuestionFileMapping.FileId;
            this.File = SurveyQuestionFileMapping.File == null ? null : new Survey_FileDTO(SurveyQuestionFileMapping.File);
        }

    }
}
