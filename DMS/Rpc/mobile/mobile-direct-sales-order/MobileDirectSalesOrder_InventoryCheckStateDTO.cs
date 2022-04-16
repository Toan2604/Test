using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_InventoryCheckStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileDirectSalesOrder_InventoryCheckStateDTO() {}
        public MobileDirectSalesOrder_InventoryCheckStateDTO(CheckState CheckState)
        {
            this.Id = CheckState.Id;
            this.Code = CheckState.Code;
            this.Name = CheckState.Name;
            this.Informations = CheckState.Informations;
            this.Warnings = CheckState.Warnings;
            this.Errors = CheckState.Errors;
        }
    }

    public class MobileDirectSalesOrder_InventoryCheckStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public CheckStateOrder OrderBy { get; set; }
    }
}
