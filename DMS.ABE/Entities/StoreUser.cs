using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class StoreUser : DataEntity, IEquatable<StoreUser>
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string OtpCode { get; set; }
        public DateTime? OtpExpired { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public Store Store { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Token { get; set; }
        public bool Equals(StoreUser other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class StoreUserFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public StringFilter Password { get; set; }
        public IdFilter StatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<StoreUserFilter> OrFilter { get; set; }
        public StoreUserOrder OrderBy { get; set; }
        public StoreUserSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreUserOrder
    {
        Id = 0,
        Store = 1,
        Username = 2,
        DisplayName = 3,
        Password = 4,
        Status = 5,
        OtpCode = 6,
        OtpExpired = 7,
        Row = 9,
        Used = 10,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum StoreUserSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Store = E._1,
        Username = E._2,
        DisplayName = E._3,
        Password = E._4,
        Status = E._5,
        OtpCode = E._6,
        OtpExpired = E._7,
        Row = E._9,
        Used = E._10,
    }
}
