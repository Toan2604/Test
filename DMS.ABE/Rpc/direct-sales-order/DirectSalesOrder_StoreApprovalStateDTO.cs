using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_StoreApprovalStateDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public DirectSalesOrder_StoreApprovalStateDTO() { }
        public DirectSalesOrder_StoreApprovalStateDTO(StoreApprovalState StoreApprovalState)
        {

            this.Id = StoreApprovalState.Id;

            this.Code = StoreApprovalState.Code;

            this.Name = StoreApprovalState.Name;

            this.Errors = StoreApprovalState.Errors;
        }
    }

    public class DirectSalesOrder_StoreApprovalStateFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StoreApprovalStateOrder OrderBy { get; set; }
    }
}