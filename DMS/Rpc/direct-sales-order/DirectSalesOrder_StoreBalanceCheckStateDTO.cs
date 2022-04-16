using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_StoreBalanceCheckStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DirectSalesOrder_StoreBalanceCheckStateDTO() {}
        public DirectSalesOrder_StoreBalanceCheckStateDTO(CheckState CheckState)
        {
            this.Id = CheckState.Id;
            this.Code = CheckState.Code;
            this.Name = CheckState.Name;
            this.Informations = CheckState.Informations;
            this.Warnings = CheckState.Warnings;
            this.Errors = CheckState.Errors;
        }
    }

    public class DirectSalesOrder_StoreBalanceCheckStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public CheckStateOrder OrderBy { get; set; }
    }
}
