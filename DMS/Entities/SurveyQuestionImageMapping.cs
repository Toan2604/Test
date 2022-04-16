using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class SurveyQuestionImageMapping : DataEntity, IEquatable<SurveyQuestionImageMapping>
    {
        public long SurveyQuestionId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }

        public bool Equals(SurveyQuestionImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class SurveyQuestionImageMappingFilter : FilterEntity
    {
        public IdFilter SurveyQuestionId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<SurveyQuestionImageMappingFilter> OrFilter { get; set; }
        public SurveyQuestionImageMappingOrder OrderBy { get; set; }
        public SurveyQuestionImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyQuestionImageMappingOrder
    {
        SurveyQuestion = 0,
        Image = 1,
    }

    [Flags]
    public enum SurveyQuestionImageMappingSelect : long
    {
        ALL = E.ALL,
        SurveyQuestion = E._0,
        Image = E._1,
    }
}
