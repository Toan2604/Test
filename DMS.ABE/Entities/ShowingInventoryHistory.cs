using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ShowingInventoryHistory : DataEntity, IEquatable<ShowingInventoryHistory>
    {
        public long Id { get; set; }
        public long ShowingInventoryId { get; set; }
        public long OldSaleStock { get; set; }
        public long SaleStock { get; set; }
        public long OldAccountingStock { get; set; }
        public long AccountingStock { get; set; }
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ShowingInventory ShowingInventory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(ShowingInventoryHistory other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.ShowingInventoryId != other.ShowingInventoryId) return false;
            if (this.OldSaleStock != other.OldSaleStock) return false;
            if (this.SaleStock != other.SaleStock) return false;
            if (this.OldAccountingStock != other.OldAccountingStock) return false;
            if (this.AccountingStock != other.AccountingStock) return false;
            if (this.AppUserId != other.AppUserId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ShowingInventoryHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter ShowingInventoryId { get; set; }
        public LongFilter OldSaleStock { get; set; }
        public LongFilter SaleStock { get; set; }
        public LongFilter OldAccountingStock { get; set; }
        public LongFilter AccountingStock { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ShowingInventoryHistoryFilter> OrFilter { get; set; }
        public ShowingInventoryHistoryOrder OrderBy { get; set; }
        public ShowingInventoryHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShowingInventoryHistoryOrder
    {
        Id = 0,
        ShowingInventory = 1,
        OldSaleStock = 2,
        SaleStock = 3,
        OldAccountingStock = 4,
        AccountingStock = 5,
        AppUser = 6,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ShowingInventoryHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        ShowingInventory = E._1,
        OldSaleStock = E._2,
        SaleStock = E._3,
        OldAccountingStock = E._4,
        AccountingStock = E._5,
        AppUser = E._6,
    }
}
