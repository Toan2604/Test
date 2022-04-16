using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Supplier : DataEntity, IEquatable<Supplier>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? NationId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string OwnerName { get; set; }
        public string Description { get; set; }
        public long? PersonInChargeId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public District District { get; set; }
        public Nation Nation { get; set; }
        public Province Province { get; set; }
        public Ward Ward { get; set; }
        public AppUser PersonInCharge { get; set; }
        public Status Status { get; set; }
        public List<Product> Products { get; set; }

        public bool Equals(Supplier other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class SupplierFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter TaxCode { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public IdFilter NationId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public StringFilter OwnerName { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter PersonInChargeId { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter UpdatedTime { get; set; }
        public List<SupplierFilter> OrFilter { get; set; }
        public SupplierOrder OrderBy { get; set; }
        public SupplierSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SupplierOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        TaxCode = 3,
        Phone = 4,
        Email = 5,
        Address = 6,
        Province = 7,
        District = 8,
        Ward = 9,
        OwnerName = 10,
        Description = 11,
        PersonInCharge = 12,
        Status = 13,
        UpdatedTime = 14,
        Nation = 15,
    }

    [Flags]
    public enum SupplierSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        TaxCode = E._3,
        Phone = E._4,
        Email = E._5,
        Address = E._6,
        Province = E._7,
        District = E._8,
        Ward = E._9,
        OwnerName = E._10,
        Description = E._11,
        PersonInCharge = E._12,
        Status = E._13,
        UpdatedAt = E._14,
        Nation = E._15,
    }
}
