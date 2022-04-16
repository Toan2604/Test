using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class SurveyOption : DataEntity, IEquatable<SurveyOption>
    {
        public long Id { get; set; }
        public long SurveyQuestionId { get; set; }
        public long SurveyOptionTypeId { get; set; }
        public string Content { get; set; }
        public bool Result { get; set; }
        public SurveyOptionType SurveyOptionType { get; set; }

        public bool Equals(SurveyOption other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class SurveyOptionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter SurveyQuestionId { get; set; }
        public IdFilter SurveyOptionTypeId { get; set; }
        public StringFilter Content { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<SurveyOptionFilter> OrFilter { get; set; }
        public SurveyOptionOrder OrderBy { get; set; }
        public SurveyOptionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyOptionOrder
    {
        Id = 0,
        SurveyQuestion = 1,
        SurveyOptionType = 2,
        Content = 3,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum SurveyOptionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        SurveyQuestion = E._1,
        SurveyOptionType = E._2,
        Content = E._3,
    }
}
