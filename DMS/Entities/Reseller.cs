using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Reseller : DataEntity, IEquatable<Reseller>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string DeputyName { get; set; }
        public string Description { get; set; }
        public long OrganizationId { get; set; }
        public long StatusId { get; set; }
        public long ResellerTypeId { get; set; }
        public long ResellerStatusId { get; set; }
        public long StaffId { get; set; }
        public Organization Organization { get; set; }
        public Status Status { get; set; }
        public ResellerStatus ResellerStatus { get; set; }
        public ResellerType ResellerType { get; set; }
        public AppUser Staff { get; set; }

        public bool Equals(Reseller other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ResellerFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter TaxCode { get; set; }
        public StringFilter CompanyName { get; set; }
        public StringFilter DeputyName { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ResellerTypeId { get; set; }
        public IdFilter ResellerStatusId { get; set; }
        public IdFilter StaffId { get; set; }
        public List<ResellerFilter> OrFilter { get; set; }
        public ResellerOrder OrderBy { get; set; }
        public ResellerSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResellerOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Phone = 3,
        Email = 4,
        Address = 5,
        TaxCode = 6,
        CompanyName = 7,
        DeputyName = 8,
        Description = 9,
        Organization = 10,
        Status = 11,
        ResellerType = 12,
        ResellerStatus = 13,
        Staff = 14,
    }

    [Flags]
    public enum ResellerSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Phone = E._3,
        Email = E._4,
        Address = E._5,
        TaxCode = E._6,
        CompanyName = E._7,
        DeputyName = E._8,
        Description = E._9,
        Organization = 10,
        Status = E._11,
        ResellerType = E._12,
        ResellerStatus = E._13,
        Staff = E._14,
    }
}
