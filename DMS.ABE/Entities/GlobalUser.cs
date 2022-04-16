using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DMS.ABE.Entities
{
    public class GlobalUser : DataEntity, IEquatable<GlobalUser>
    {
        public long Id { get; set; }
        public long GlobalUserTypeId { get; set; }
        public Guid RowId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public GlobalUserType GlobalUserType { get; set; }

        public bool Equals(GlobalUser other)
        {
            return true;
        }
    }

    public class GlobalUserFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter GlobalUserTypeId { get; set; }
        public GuidFilter RowId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public StringFilter Avatar { get; set; }
       
        public List<GlobalUserFilter> OrFilter { get; set; }
        public GlobalUserOrder OrderBy { get; set; }
        public GlobalUserSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GlobalUserOrder
    {
        RowId = 0,
        Id = 1,
        Username = 2,
        DisplayName = 3,
        Avatar = 4,
    }

    [Flags]
    public enum GlobalUserSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        RowId = E._1,
        Username = E._2,
        DisplayName = E._3,
        Avatar = E._4,
        GlobalUserType = E._5,
    }
}
