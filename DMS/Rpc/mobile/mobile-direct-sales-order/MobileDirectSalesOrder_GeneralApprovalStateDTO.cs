using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_GeneralApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileDirectSalesOrder_GeneralApprovalStateDTO() { }
        public MobileDirectSalesOrder_GeneralApprovalStateDTO(GeneralApprovalState GeneralApprovalState)
        {
            this.Id = GeneralApprovalState.Id;
            this.Code = GeneralApprovalState.Code;
            this.Name = GeneralApprovalState.Name;
            this.Errors = GeneralApprovalState.Errors;
        }
    }

    public class MobileDirectSalesOrder_GeneralApprovalStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public GeneralApprovalStateOrder OrderBy { get; set; }
    }
}
