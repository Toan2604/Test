using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_PrintContentDTO : DataDTO
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
        public IndirectSalesOrder_ItemDTO Item { get; set; }
        public IndirectSalesOrder_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public IndirectSalesOrder_TaxTypeDTO TaxType { get; set; }
        public IndirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }

        public IndirectSalesOrder_PrintContentDTO() { }
        public IndirectSalesOrder_PrintContentDTO(IndirectSalesOrderContent IndirectSalesOrderContent)
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
            this.Item = IndirectSalesOrderContent.Item == null ? null : new IndirectSalesOrder_ItemDTO(IndirectSalesOrderContent.Item);
            this.PrimaryUnitOfMeasure = IndirectSalesOrderContent.PrimaryUnitOfMeasure == null ? null : new IndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderContent.PrimaryUnitOfMeasure);
            this.UnitOfMeasure = IndirectSalesOrderContent.UnitOfMeasure == null ? null : new IndirectSalesOrder_UnitOfMeasureDTO(IndirectSalesOrderContent.UnitOfMeasure);
            this.Errors = IndirectSalesOrderContent.Errors;
        }
    }
}
