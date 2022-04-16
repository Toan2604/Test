using DMS.ABE.Entities;
using TrueSight.Common;

namespace DMS.Rpc.ABE.mobile.general_mobile
{
    public class DirectSalesOrder_ErpApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DirectSalesOrder_ErpApprovalStateDTO() { }
        public DirectSalesOrder_ErpApprovalStateDTO(ErpApprovalState ErpApprovalState)
        {
            this.Id = ErpApprovalState.Id;
            this.Code = ErpApprovalState.Code;
            this.Name = ErpApprovalState.Name;
            this.Errors = ErpApprovalState.Errors;
        }
    }

    public class DirectSalesOrder_ErpApprovalStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public RequestStateOrder OrderBy { get; set; }
    }
}
