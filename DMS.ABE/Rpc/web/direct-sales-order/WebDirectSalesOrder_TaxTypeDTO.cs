using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_TaxTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public long StatusId { get; set; }
        public WebDirectSalesOrder_TaxTypeDTO() { }
        public WebDirectSalesOrder_TaxTypeDTO(TaxType TaxType)
        {
            this.Id = TaxType.Id;
            this.Code = TaxType.Code;
            this.Name = TaxType.Name;
            this.Percentage = TaxType.Percentage;
            this.StatusId = TaxType.StatusId;
            this.Errors = TaxType.Errors;
        }
    }
    public class WebDirectSalesOrder_TaxTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DecimalFilter Percentage { get; set; }
        public IdFilter StatusId { get; set; }
        public TaxTypeOrder OrderBy { get; set; }
    }
}
