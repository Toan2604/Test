using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class SurveyResultSingle : DataEntity, IEquatable<SurveyResultSingle>
    {
        public long SurveyQuestionId { get; set; }
        public long SurveyResultId { get; set; }
        public long SurveyOptionId { get; set; }
        public SurveyResult SurveyResult { get; set; }
        public SurveyOption SurveyOption { get; set; }

        public bool Equals(SurveyResultSingle other)
        {
            return other != null && SurveyQuestionId == other.SurveyQuestionId && SurveyResultId == other.SurveyResultId;
        }
        public override int GetHashCode()
        {
            return SurveyQuestionId.GetHashCode() ^ SurveyResultId.GetHashCode();
        }
    }

    public class SurveyResultSingleFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter SurveyQuestionId { get; set; }
        public IdFilter SurveyOptionId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter Time { get; set; }
        public List<SurveyResultSingleFilter> OrFilter { get; set; }
        public SurveyResultSingleOrder OrderBy { get; set; }
        public SurveyResultSingleSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyResultSingleOrder
    {
        Id = 0,
        SurveyQuestion = 1,
        SurveyOption = 2,
        AppUser = 3,
        Store = 4,
        Time = 5,
    }

    [Flags]
    public enum SurveyResultSingleSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        SurveyQuestion = E._1,
        SurveyOption = E._2,
        AppUser = E._3,
        Store = E._4,
        Time = E._5,
    }
}
