using DMS.ABE.Common; using TrueSight.Common;

namespace DMS.ABE.Entities
{
    public class SurveyRespondentType : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
