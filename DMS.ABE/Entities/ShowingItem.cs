using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ShowingItem : DataEntity, IEquatable<ShowingItem>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ShowingCategoryId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public string ERPCode { get; set; }
        public string Description { get; set; }
        public long SaleStock { get; set; }
        public long StatusId { get; set; }
        public bool HasInventory { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public ShowingCategory ShowingCategory { get; set; }
        public Status Status { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public List<ShowingItemImageMapping> ShowingItemImageMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(ShowingItem other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.Name != other.Name) return false;
            if (this.ShowingCategoryId != other.ShowingCategoryId) return false;
            if (this.UnitOfMeasureId != other.UnitOfMeasureId) return false;
            if (this.SalePrice != other.SalePrice) return false;
            if (this.ERPCode != other.ERPCode) return false;
            if (this.Description != other.Description) return false;
            if (this.StatusId != other.StatusId) return false;
            if (this.Used != other.Used) return false;
            if (this.RowId != other.RowId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ShowingItemFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ShowingCategoryId { get; set; }
        public IdFilter ShowingWarehouseId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public StringFilter ERPCode { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public string Search { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ShowingItemFilter> OrFilter { get; set; }
        public ShowingItemOrder OrderBy { get; set; }
        public ShowingItemSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShowingItemOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        ShowingCategory = 3,
        UnitOfMeasure = 4,
        SalePrice = 5,
        Description = 6,
        Status = 7,
        Used = 11,
        Row = 12,
        ERPCode = 13,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ShowingItemSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        ShowingCategory = E._3,
        UnitOfMeasure = E._4,
        SalePrice = E._5,
        Description = E._6,
        Status = E._7,
        Used = E._11,
        Row = E._12,
        ERPCode = E._13,
    }
}
