using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.sync_fast
{
    public class Sync_PriceListItemMappingDTO : DataDTO
    {
        public long PriceListId { get; set; }
        public long ItemId { get; set; }
        public decimal Price { get; set; }
        public List<Sync_PriceListItemHistoryDTO> PriceListItemHistories { get; set; }
        public Sync_PriceListItemMappingDTO() { }
        public Sync_PriceListItemMappingDTO(PriceListItemMapping PriceListItemMapping)
        {
            this.PriceListId = PriceListItemMapping.PriceListId;
            this.ItemId = PriceListItemMapping.ItemId;
            this.Price = PriceListItemMapping.Price;
            this.PriceListItemHistories = PriceListItemMapping.PriceListItemHistories?.Select(x => new Sync_PriceListItemHistoryDTO(x)).ToList();
            this.Errors = PriceListItemMapping.Errors;
        }
    }

    public class PriceList_PriceListItemMappingFilterDTO : FilterDTO
    {

        public IdFilter PriceListId { get; set; }

        public IdFilter ItemId { get; set; }

        public DecimalFilter Price { get; set; }

        public PriceListItemMappingOrder OrderBy { get; set; }
    }
}