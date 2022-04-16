using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_InventoryCheckStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileIndirectSalesOrder_InventoryCheckStateDTO() {}
        public MobileIndirectSalesOrder_InventoryCheckStateDTO(CheckState CheckState)
        {
            this.Id = CheckState.Id;
            this.Code = CheckState.Code;
            this.Name = CheckState.Name;
            this.Informations = CheckState.Informations;
            this.Warnings = CheckState.Warnings;
            this.Errors = CheckState.Errors;
        }
    }

    public class MobileIndirectSalesOrder_InventoryCheckStateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public CheckStateOrder OrderBy { get; set; }
    }
}
