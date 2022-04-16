using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_IndirectSalesOrderContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long IndirectSalesOrderId { get; set; }
        public long ItemId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal PrimaryPrice { get; set; }
        public decimal SalePrice { get; set; }
        public long EditedPriceStatusId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public long? Factor { get; set; }
        public MobileSync_EditedPriceStatusDTO EditedPriceStatus { get; set; }
        public MobileSync_ItemDTO Item { get; set; }
        public MobileSync_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public MobileSync_TaxTypeDTO TaxType { get; set; }
        public MobileSync_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public MobileSync_IndirectSalesOrderContentDTO() { }
        public MobileSync_IndirectSalesOrderContentDTO(IndirectSalesOrderContent IndirectSalesOrderContent)
        {
            this.Id = IndirectSalesOrderContent.Id;
            this.IndirectSalesOrderId = IndirectSalesOrderContent.IndirectSalesOrderId;
            this.ItemId = IndirectSalesOrderContent.ItemId;
            this.UnitOfMeasureId = IndirectSalesOrderContent.UnitOfMeasureId;
            this.Quantity = IndirectSalesOrderContent.Quantity;
            this.PrimaryUnitOfMeasureId = IndirectSalesOrderContent.PrimaryUnitOfMeasureId;
            this.RequestedQuantity = IndirectSalesOrderContent.RequestedQuantity;
            this.PrimaryPrice = IndirectSalesOrderContent.PrimaryPrice;
            this.SalePrice = IndirectSalesOrderContent.SalePrice;
            this.EditedPriceStatusId = IndirectSalesOrderContent.EditedPriceStatusId;
            this.DiscountPercentage = IndirectSalesOrderContent.DiscountPercentage;
            this.DiscountAmount = IndirectSalesOrderContent.DiscountAmount;
            this.GeneralDiscountPercentage = IndirectSalesOrderContent.GeneralDiscountPercentage;
            this.GeneralDiscountAmount = IndirectSalesOrderContent.GeneralDiscountAmount;
            this.Amount = IndirectSalesOrderContent.Amount;
            this.TaxPercentage = IndirectSalesOrderContent.TaxPercentage;
            this.TaxAmount = IndirectSalesOrderContent.TaxAmount;
            this.Factor = IndirectSalesOrderContent.Factor;
            this.EditedPriceStatus = IndirectSalesOrderContent.EditedPriceStatus == null ? null : new MobileSync_EditedPriceStatusDTO(IndirectSalesOrderContent.EditedPriceStatus);
            this.Item = IndirectSalesOrderContent.Item == null ? null : new MobileSync_ItemDTO(IndirectSalesOrderContent.Item);
            this.PrimaryUnitOfMeasure = IndirectSalesOrderContent.PrimaryUnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO(IndirectSalesOrderContent.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = IndirectSalesOrderContent.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO(IndirectSalesOrderContent.UnitOfMeasure);

        }
    }
}