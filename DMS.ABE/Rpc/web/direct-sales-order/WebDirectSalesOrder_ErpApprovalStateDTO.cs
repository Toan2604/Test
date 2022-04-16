using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_ErpApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public WebDirectSalesOrder_ErpApprovalStateDTO() { }
        public WebDirectSalesOrder_ErpApprovalStateDTO(ErpApprovalState ErpApprovalState)
        {
            this.Id = ErpApprovalState.Id;
            this.Code = ErpApprovalState.Code;
            this.Name = ErpApprovalState.Name;
            this.Errors = ErpApprovalState.Errors;
        }
    }
    public class WebDirectSalesOrder_ErpApprovalStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public ErpApprovalStateOrder OrderBy { get; set; }
    }
}