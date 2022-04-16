using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_StoreApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileIndirectSalesOrder_StoreApprovalStateDTO() { }
        public MobileIndirectSalesOrder_StoreApprovalStateDTO(StoreApprovalState StoreApprovalState)
        {
            this.Id = StoreApprovalState.Id;
            this.Code = StoreApprovalState.Code;
            this.Name = StoreApprovalState.Name;
            this.Errors = StoreApprovalState.Errors;
        }
    }

    public class MobileIndirectSalesOrder_StoreApprovalStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public RequestStateOrder OrderBy { get; set; }
    }
}
