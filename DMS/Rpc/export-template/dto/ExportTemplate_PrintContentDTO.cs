using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_PrintContentDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string QuantityString { get; set; }
        public string RequestedQuantityString { get; set; }
        public string PrimaryPriceString { get; set; }
        public string SalePriceString { get; set; }
        public string DiscountString { get; set; }
        public string AmountString { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TaxPercentageString { get; set; }
        public long? Factor { get; set; }
        public ExportTemplate_ItemDTO Item { get; set; }
        public ExportTemplate_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
        public ExportTemplate_TaxTypeDTO TaxType { get; set; }
        public ExportTemplate_UnitOfMeasureDTO UnitOfMeasure { get; set; }
    }
}
