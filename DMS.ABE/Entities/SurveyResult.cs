using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class SurveyResult : DataEntity, IEquatable<SurveyResult>
    {
        public long Id { get; set; }
        public long SurveyId { get; set; }
        public string RespondentName { get; set; }
        public string RespondentPhone { get; set; }
        public string RespondentEmail { get; set; }
        public string RespondentAddress { get; set; }
        public long OrganizationId { get; set; }
        public long AppUserId { get; set; }
        public long? StoreId { get; set; }
        public long? StoreScoutingId { get; set; }
        public DateTime Time { get; set; }
        public Guid RowId { get; set; }

        public AppUser AppUser { get; set; }
        public Store Store { get; set; }
        public StoreScouting StoreScouting { get; set; }
        public Survey Survey { get; set; }
        public List<SurveyResultText> SurveyResultTexts { get; set; }
        public List<SurveyResultCell> SurveyResultCells { get; set; }
        public List<SurveyResultSingle> SurveyResultSingles { get; set; }

        public long SurveyRespondentTypeId { get; set; }
        public SurveyRespondentType SurveyRespondentType { get; set; }

        public bool Equals(SurveyResult other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class SurveyResultFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter SurveyId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreId { get; set; }
        public DateFilter Time { get; set; }
        public List<SurveyFilter> OrFilter { get; set; }
        public SurveyResultOrder OrderBy { get; set; }
        public SurveyResultSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SurveyResultOrder
    {
        Id = 0,
        Survey = 1,
        AppUser = 2,
        Store = 3,
        Time = 4,
    }

    [Flags]
    public enum SurveyResultSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Survey = E._1,
        AppUser = E._2,
        Store = E._3,
        Time = E._4,
        Content = E._5,
        StoreScouting = E._6,
        RespondentName = E._7,
        RespondentPhone = E._8,
        RespondentEmail = E._9,
        RespondentAddress = E._10,
        SurveyRespondentType = E._11,
    }
}
