using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class SurveyQuestionFileMapping : DataEntity, IEquatable<SurveyQuestionFileMapping>
    {
        public long SurveyQuestionId { get; set; }
        public long FileId { get; set; }
        public File File { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }

        public bool Equals(SurveyQuestionFileMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class SurveyQuestionFileMappingFilter : FilterEntity
    {
        public IdFilter SurveyQuestionId { get; set; }
        public IdFilter FileId { get; set; }
        public List<SurveyQuestionFileMappingFilter> OrFilter { get; set; }
        public SurveyQuestionFileMappingOrder OrderBy { get; set; }
        public SurveyQuestionFileMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyQuestionFileMappingOrder
    {
        SurveyQuestion = 0,
        File = 1,
    }

    [Flags]
    public enum SurveyQuestionFileMappingSelect : long
    {
        ALL = E.ALL,
        SurveyQuestion = E._0,
        File = E._1,
    }
}
