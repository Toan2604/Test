using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_PrintIndirectOrderContentDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityString { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string RequestedQuantityString { get; set; }
        public decimal PrimaryPrice { get; set; }
        public string PrimaryPriceString { get; set; }
        public decimal SalePrice { get; set; }
        public string SalePriceString { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GeneralDiscountPercentage { get; set; }
        public decimal? GeneralDiscountAmount { get; set; }
        public string DiscountString { get; set; }
        public decimal Amount { get; set; }
        public string AmountString { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TaxPercentageString { get; set; }
        public long? Factor { get; set; }
        public MobileIndirectSalesOrder_ItemDTO Item { get; set; }
        public MobileIndirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public MobileIndirectSalesOrder_TaxTypeDTO TaxType { get; set; }
        public MobileIndirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public MobileIndirectSalesOrder_PrintIndirectOrderContentDTO() { }
        public MobileIndirectSalesOrder_PrintIndirectOrderContentDTO(IndirectSalesOrderContent IndirectSalesOrderContent)
        {
            this.Id = IndirectSalesOrderContent.Id;
            this.Quantity = IndirectSalesOrderContent.Quantity;
            this.PrimaryPrice = IndirectSalesOrderContent.PrimaryPrice;
            this.RequestedQuantity = IndirectSalesOrderContent.RequestedQuantity;
            this.SalePrice = IndirectSalesOrderContent.SalePrice;
            this.DiscountPercentage = IndirectSalesOrderContent.DiscountPercentage;
            this.DiscountAmount = IndirectSalesOrderContent.DiscountAmount;
            this.GeneralDiscountPercentage = IndirectSalesOrderContent.GeneralDiscountPercentage;
            this.GeneralDiscountAmount = IndirectSalesOrderContent.GeneralDiscountAmount;
            this.Amount = IndirectSalesOrderContent.Amount;
            this.TaxPercentage = IndirectSalesOrderContent.TaxPercentage;
            this.TaxAmount = IndirectSalesOrderContent.TaxAmount;
            this.Factor = IndirectSalesOrderContent.Factor;
            this.Item = IndirectSalesOrderContent.Item == null ? null : new MobileIndirectSalesOrder_ItemDTO(IndirectSalesOrderContent.Item);
            this.PrimaryUnitOfMeasure = IndirectSalesOrderContent.PrimaryUnitOfMeasure == null ? null : new MobileIndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderContent.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = IndirectSalesOrderContent.UnitOfMeasure == null ? null : new MobileIndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderContent.UnitOfMeasure);
            this.Errors = IndirectSalesOrderContent.Errors;
        }
    }
}
