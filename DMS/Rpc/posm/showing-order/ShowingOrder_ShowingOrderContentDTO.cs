using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order
{
    public class ShowingOrder_ShowingOrderContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long ShowingOrderId { get; set; }
        public long ShowingItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public string eSalePrice { get; set; }
        public long Quantity { get; set; }
        public string eQuantity { get; set; }
        public decimal Amount { get; set; }
        public string eAmount { get; set; }
        public ShowingOrder_ShowingItemDTO ShowingItem { get; set; }
        public ShowingOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public ShowingOrder_ShowingOrderContentDTO() { }
        public ShowingOrder_ShowingOrderContentDTO(ShowingOrderContent ShowingOrderContent)
        {
            this.Id = ShowingOrderContent.Id;
            this.ShowingOrderId = ShowingOrderContent.ShowingOrderId;
            this.ShowingItemId = ShowingOrderContent.ShowingItemId;
            this.UnitOfMeasureId = ShowingOrderContent.UnitOfMeasureId;
            this.SalePrice = ShowingOrderContent.SalePrice;
            this.Quantity = ShowingOrderContent.Quantity;
            this.Amount = ShowingOrderContent.Amount;
            this.ShowingItem = ShowingOrderContent.ShowingItem == null ? null : new ShowingOrder_ShowingItemDTO(ShowingOrderContent.ShowingItem);
            this.UnitOfMeasure = ShowingOrderContent.UnitOfMeasure == null ? null : new ShowingOrder_UnitOfMeasureDTO(ShowingOrderContent.UnitOfMeasure);
            this.Errors = ShowingOrderContent.Errors;
        }
    }

    public class ShowingOrder_ShowingOrderContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter ShowingOrderId { get; set; }

        public IdFilter ShowingItemId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public DecimalFilter SalePrice { get; set; }

        public LongFilter Quantity { get; set; }

        public DecimalFilter Amount { get; set; }

        public ShowingOrderContentOrder OrderBy { get; set; }
    }
}