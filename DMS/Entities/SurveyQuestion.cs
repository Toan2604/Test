using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class SurveyQuestion : DataEntity, IEquatable<SurveyQuestion>
    {
        public long Id { get; set; }
        public long SurveyId { get; set; }
        public string Content { get; set; }
        public long SurveyQuestionTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public SurveyQuestionType SurveyQuestionType { get; set; }
        public List<SurveyOption> SurveyOptions { get; set; }
        public Dictionary<long, Dictionary<long, bool>> TableResult { get; set; }
        public Dictionary<long, bool> ListResult { get; set; }
        public string TextResult { get; set; }
        public List<SurveyQuestionImageMapping> SurveyQuestionImageMappings { get; set; }
        public List<SurveyQuestionFileMapping> SurveyQuestionFileMappings { get; set; }
        public bool Equals(SurveyQuestion other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class SurveyQuestionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter SurveyId { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter SurveyQuestionTypeId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<SurveyQuestionFilter> OrFilter { get; set; }
        public SurveyQuestionOrder OrderBy { get; set; }
        public SurveyQuestionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyQuestionOrder
    {
        Id = 0,
        Survey = 1,
        Content = 2,
        SurveyQuestionType = 3,
        IsMandatory = 4,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum SurveyQuestionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Survey = E._1,
        Content = E._2,
        SurveyQuestionType = E._3,
        IsMandatory = E._4,
    }
}
