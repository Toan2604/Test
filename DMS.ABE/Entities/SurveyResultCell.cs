using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class SurveyResultCell : DataEntity, IEquatable<SurveyResultCell>
    {
        public long SurveyQuestionId { get; set; }
        public long SurveyResultId { get; set; }
        public long RowOptionId { get; set; }
        public long ColumnOptionId { get; set; }
        public SurveyResult SurveyResult { get; set; }
        public SurveyOption ColumnOption { get; set; }
        public SurveyOption RowOption { get; set; }

        public bool Equals(SurveyResultCell other)
        {
            return other != null && SurveyQuestionId == other.SurveyQuestionId && SurveyResultId == other.SurveyResultId;
        }
        public override int GetHashCode()
        {
            return SurveyQuestionId.GetHashCode() ^ SurveyResultId.GetHashCode();
        }
    }

    public class SurveyResultCellFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter SurveyQuestionId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter RowOptionId { get; set; }
        public IdFilter ColumnOptionId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter Time { get; set; }
        public List<SurveyResultCellFilter> OrFilter { get; set; }
        public SurveyResultCellOrder OrderBy { get; set; }
        public SurveyResultCellSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyResultCellOrder
    {
        Id = 0,
        SurveyQuestion = 1,
        AppUser = 2,
        RowOption = 3,
        ColumnOption = 4,
        Store = 5,
        Time = 6,
    }

    [Flags]
    public enum SurveyResultCellSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        SurveyQuestion = E._1,
        AppUser = E._2,
        RowOption = E._3,
        ColumnOption = E._4,
        Store = E._5,
        Time = E._6,
    }
}
