using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_StoreApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileDirectSalesOrder_StoreApprovalStateDTO() { }
        public MobileDirectSalesOrder_StoreApprovalStateDTO(StoreApprovalState StoreApprovalState)
        {
            this.Id = StoreApprovalState.Id;
            this.Code = StoreApprovalState.Code;
            this.Name = StoreApprovalState.Name;
            this.Errors = StoreApprovalState.Errors;
        }
    }

    public class MobileDirectSalesOrder_StoreApprovalStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public RequestStateOrder OrderBy { get; set; }
    }
}
