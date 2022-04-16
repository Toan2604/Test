using DMS.Entities;
namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SurveyQuestionFileMappingDTO
    {
        public long SurveyQuestionId { get; set; }
        public long FileId { get; set; }
        public GeneralMobile_FileDTO File { get; set; }
        public GeneralMobile_SurveyQuestionDTO SurveyQuestion { get; set; }

        public GeneralMobile_SurveyQuestionFileMappingDTO() { }

        public GeneralMobile_SurveyQuestionFileMappingDTO(SurveyQuestionFileMapping SurveyQuestionFileMapping)
        {
            this.SurveyQuestionId = SurveyQuestionFileMapping.SurveyQuestionId;
            this.FileId = SurveyQuestionFileMapping.FileId;
            this.File = SurveyQuestionFileMapping.File == null ? null : new GeneralMobile_FileDTO(SurveyQuestionFileMapping.File);
        }

    }
}
