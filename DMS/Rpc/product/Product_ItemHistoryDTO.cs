using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    public class Product_ItemHistoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public DateTime Time { get; set; }
        public long ModifierId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public Product_AppUserDTO Modifier { get; set; }
        public Product_ItemHistoryDTO() { }
        public Product_ItemHistoryDTO(ItemHistory ItemHistory)
        {
            this.Id = ItemHistory.Id;
            this.ItemId = ItemHistory.ItemId;
            this.Time = ItemHistory.Time;
            this.ModifierId = ItemHistory.ModifierId;
            this.OldPrice = ItemHistory.OldPrice;
            this.NewPrice = ItemHistory.NewPrice;
            this.Modifier = ItemHistory.Modifier == null ? null : new Product_AppUserDTO(ItemHistory.Modifier);
            this.Errors = ItemHistory.Errors;
        }
    }

    public class Product_ItemHistoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ItemId { get; set; }
        public DateFilter Time { get; set; }
        public IdFilter ModifierId { get; set; }
        public DecimalFilter OldPrice { get; set; }
        public DecimalFilter NewPrice { get; set; }
        public ItemHistoryOrder OrderBy { get; set; }
    }
}
