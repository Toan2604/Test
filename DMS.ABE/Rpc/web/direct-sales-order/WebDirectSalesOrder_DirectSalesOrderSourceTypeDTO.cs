using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_DirectSalesOrderSourceTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public WebDirectSalesOrder_DirectSalesOrderSourceTypeDTO() { }
        public WebDirectSalesOrder_DirectSalesOrderSourceTypeDTO(DirectSalesOrderSourceType DirectSalesOrderSourceType)
        {
            this.Id = DirectSalesOrderSourceType.Id;
            this.Code = DirectSalesOrderSourceType.Code;
            this.Name = DirectSalesOrderSourceType.Name;
            this.Errors = DirectSalesOrderSourceType.Errors;
        }
    }
    public class WebDirectSalesOrder_DirectSalesOrderSourceTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DirectSalesOrderSourceTypeOrder OrderBy { get; set; }
    }
}