using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class Inventory : DataEntity, IEquatable<Inventory>
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public decimal SaleStock { get; set; }
        public decimal AccountingStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Item Item { get; set; }
        public Warehouse Warehouse { get; set; }
        public Guid RowId { get; set; }
        public List<InventoryHistory> InventoryHistories { get; set; }

        public bool Equals(Inventory other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class InventoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter WarehouseId { get; set; }
        public IdFilter ItemId { get; set; }
        public DecimalFilter SaleStock { get; set; }
        public DecimalFilter AccountingStock { get; set; }
        public List<InventoryFilter> OrFilter { get; set; }
        public InventoryOrder OrderBy { get; set; }
        public InventorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum InventoryOrder
    {
        Id = 0,
        Warehouse = 1,
        Item = 2,
        SaleStock = 3,
        AccountingStock = 4,
        UpdatedAt = 5,
    }

    [Flags]
    public enum InventorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Warehouse = E._1,
        Item = E._2,
        SaleStock = E._3,
        AccountingStock = E._4,
        UpdatedAt = E._5,
    }
}
