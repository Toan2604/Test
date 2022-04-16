using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ShowingInventory : DataEntity, IEquatable<ShowingInventory>
    {
        public long Id { get; set; }
        public long ShowingWarehouseId { get; set; }
        public long ShowingItemId { get; set; }
        public long SaleStock { get; set; }
        public long? AccountingStock { get; set; }
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ShowingItem ShowingItem { get; set; }
        public ShowingWarehouse ShowingWarehouse { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<ShowingInventoryHistory> ShowingInventoryHistories { get; set; }
        public bool Equals(ShowingInventory other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.ShowingWarehouseId != other.ShowingWarehouseId) return false;
            if (this.ShowingItemId != other.ShowingItemId) return false;
            if (this.SaleStock != other.SaleStock) return false;
            if (this.AccountingStock != other.AccountingStock) return false;
            if (this.AppUserId != other.AppUserId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ShowingInventoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ShowingWarehouseId { get; set; }
        public IdFilter ShowingItemId { get; set; }
        public LongFilter SaleStock { get; set; }
        public LongFilter AccountingStock { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ShowingInventoryFilter> OrFilter { get; set; }
        public ShowingInventoryOrder OrderBy { get; set; }
        public ShowingInventorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShowingInventoryOrder
    {
        Id = 0,
        ShowingWarehouse = 1,
        ShowingItem = 2,
        SaleStock = 3,
        AccountingStock = 4,
        AppUser = 5,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ShowingInventorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ShowingWarehouse = E._1,
        ShowingItem = E._2,
        SaleStock = E._3,
        AccountingStock = E._4,
        AppUser = E._5,
    }
}
