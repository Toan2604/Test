using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_ProductTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DirectSalesOrder_ProductTypeDTO() { }
        public DirectSalesOrder_ProductTypeDTO(ProductType ProductType)
        {

            this.Id = ProductType.Id;

            this.Code = ProductType.Code;

            this.Name = ProductType.Name;

            this.Description = ProductType.Description;

            this.StatusId = ProductType.StatusId;
            this.UpdatedTime = ProductType.UpdatedAt;
        }
    }

    public class DirectSalesOrder_ProductTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Description { get; set; }

        public IdFilter StatusId { get; set; }
        public DateFilter UpdatedTime { get; set; }
        public ProductTypeOrder OrderBy { get; set; }
    }
}
