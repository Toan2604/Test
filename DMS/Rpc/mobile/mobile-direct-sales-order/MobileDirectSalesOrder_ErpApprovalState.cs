using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_ErpApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileDirectSalesOrder_ErpApprovalStateDTO() { }
        public MobileDirectSalesOrder_ErpApprovalStateDTO(ErpApprovalState ErpApprovalState)
        {
            this.Id = ErpApprovalState.Id;
            this.Code = ErpApprovalState.Code;
            this.Name = ErpApprovalState.Name;
            this.Errors = ErpApprovalState.Errors;
        }
    }

    public class MobileDirectSalesOrder_ErpApprovalStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public RequestStateOrder OrderBy { get; set; }
    }
}
