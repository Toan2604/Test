using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public class ShowingOrderWithDraw_ShowingOrderContentWithDrawDTO : DataDTO
    {
        public long Id { get; set; }
        public long ShowingOrderWithDrawId { get; set; }
        public long ShowingItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public string eSalePrice { get; set; } // for export excel
        public long Quantity { get; set; }
        public string eQuantity { get; set; } // for export excel
        public decimal Amount { get; set; }
        public string eAmount { get; set; } // for export excel
        public ShowingOrderWithDraw_ShowingItemDTO ShowingItem { get; set; }
        public ShowingOrderWithDraw_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public ShowingOrderWithDraw_ShowingOrderContentWithDrawDTO() { }
        public ShowingOrderWithDraw_ShowingOrderContentWithDrawDTO(ShowingOrderContentWithDraw ShowingOrderContentWithDraw)
        {
            this.Id = ShowingOrderContentWithDraw.Id;
            this.ShowingOrderWithDrawId = ShowingOrderContentWithDraw.ShowingOrderWithDrawId;
            this.ShowingItemId = ShowingOrderContentWithDraw.ShowingItemId;
            this.UnitOfMeasureId = ShowingOrderContentWithDraw.UnitOfMeasureId;
            this.SalePrice = ShowingOrderContentWithDraw.SalePrice;
            this.Quantity = ShowingOrderContentWithDraw.Quantity;
            this.Amount = ShowingOrderContentWithDraw.Amount;
            this.ShowingItem = ShowingOrderContentWithDraw.ShowingItem == null ? null : new ShowingOrderWithDraw_ShowingItemDTO(ShowingOrderContentWithDraw.ShowingItem);
            this.UnitOfMeasure = ShowingOrderContentWithDraw.UnitOfMeasure == null ? null : new ShowingOrderWithDraw_UnitOfMeasureDTO(ShowingOrderContentWithDraw.UnitOfMeasure);
            this.Errors = ShowingOrderContentWithDraw.Errors;
        }
    }

    public class ShowingOrderWithDraw_ShowingOrderWithDrawContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter ShowingOrderWithDrawId { get; set; }

        public IdFilter ShowingItemId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public DecimalFilter SalePrice { get; set; }

        public LongFilter Quantity { get; set; }

        public DecimalFilter Amount { get; set; }

        public ShowingOrderContentWithDrawOrder OrderBy { get; set; }
    }
}