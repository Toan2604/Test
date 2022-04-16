using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_EditedPriceStatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public WebDirectSalesOrder_EditedPriceStatusDTO() { }
        public WebDirectSalesOrder_EditedPriceStatusDTO(EditedPriceStatus EditedPriceStatus)
        {
            this.Id = EditedPriceStatus.Id;
            this.Code = EditedPriceStatus.Code;
            this.Name = EditedPriceStatus.Name;
            this.Errors = EditedPriceStatus.Errors;
        }
    }
    public class WebDirectSalesOrder_EditedPriceStatusFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public EditedPriceStatusOrder OrderBy { get; set; }
    }
}