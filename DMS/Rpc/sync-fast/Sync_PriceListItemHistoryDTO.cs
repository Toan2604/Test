using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.sync_fast
{
    public class Sync_PriceListItemHistoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long PriceListId { get; set; }
        public long ItemId { get; set; }
        public long ModifierId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Sync_PriceListItemHistoryDTO() { }
        public Sync_PriceListItemHistoryDTO(PriceListItemHistory PriceListItemHistory)
        {
            this.Id = PriceListItemHistory.Id;
            this.PriceListId = PriceListItemHistory.PriceListId;
            this.ItemId = PriceListItemHistory.ItemId;
            this.ModifierId = PriceListItemHistory.ModifierId;
            this.OldPrice = PriceListItemHistory.OldPrice;
            this.NewPrice = PriceListItemHistory.NewPrice;
            this.UpdatedAt = PriceListItemHistory.UpdatedAt;
        }
    }

    public class PriceList_PriceListItemHistoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter PriceListId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter ModifierId { get; set; }
        public DecimalFilter OldPrice { get; set; }
        public DecimalFilter NewPrice { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public ItemHistoryOrder OrderBy { get; set; }
    }
}
