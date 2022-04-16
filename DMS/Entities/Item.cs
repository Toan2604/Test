using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Item : DataEntity, IEquatable<Item>
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string SearchName { get; set; }
        public string ScanCode { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal SaleStock { get; set; }
        public long StatusId { get; set; }
        public bool HasInventory { get; set; }
        public DateTime? LastUpdateInventory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public Product Product { get; set; }
        public Status Status { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<ItemImageMapping> ItemImageMappings { get; set; }
        public List<ItemHistory> ItemHistories { get; set; }
        public bool Equals(Item other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ItemFilter : FilterEntity
    {
        public string Search { get; set; }
        public IdFilter Id { get; set; }
        public IdFilter ProductId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter ERPCode { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter ScanCode { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter SupplierId { get; set; }
        public IdFilter BrandId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter CategoryId { get; set; }
        public bool? IsNew { get; set; }
        public List<ItemFilter> OrFilter { get; set; }
        public ItemOrder OrderBy { get; set; }
        public ItemSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemOrder
    {
        Id = 0,
        Product = 1,
        Code = 2,
        Name = 3,
        ScanCode = 4,
        SalePrice = 5,
        RetailPrice = 6,
        Score = 7
    }

    [Flags]
    public enum ItemSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Product = E._1,
        Code = E._2,
        Name = E._3,
        ScanCode = E._4,
        SalePrice = E._5,
        RetailPrice = E._6,
        Status = E._7,
        ProductId = E._8,
        ERPCode = E._9,
    }
}
