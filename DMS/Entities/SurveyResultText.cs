namespace DMS.Entities
{
    public class SurveyResultText
    {
        public long SurveyResultId { get; set; }
        public long SurveyQuestionId { get; set; }
        public string Content { get; set; }

        public virtual SurveyQuestion SurveyQuestion { get; set; }
        public virtual SurveyResult SurveyResult { get; set; }
    }
}
