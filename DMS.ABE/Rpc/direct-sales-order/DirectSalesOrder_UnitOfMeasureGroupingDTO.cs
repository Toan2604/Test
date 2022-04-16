using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_UnitOfMeasureGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UnitOfMeasureId { get; set; }
        public DirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public List<DirectSalesOrder_UnitOfMeasureGroupingContentDTO> UnitOfMeasureGroupingContents { get; set; }
        public DirectSalesOrder_UnitOfMeasureGroupingDTO() { }
        public DirectSalesOrder_UnitOfMeasureGroupingDTO(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            this.Id = UnitOfMeasureGrouping.Id;
            this.Code = UnitOfMeasureGrouping.Code;
            this.Name = UnitOfMeasureGrouping.Name;
            this.Description = UnitOfMeasureGrouping.Description;
            this.UnitOfMeasureId = UnitOfMeasureGrouping.UnitOfMeasureId;
            this.UnitOfMeasure = UnitOfMeasureGrouping.UnitOfMeasure == null ? null : new DirectSalesOrder_UnitOfMeasureDTO(UnitOfMeasureGrouping.UnitOfMeasure);
            this.UnitOfMeasureGroupingContents = UnitOfMeasureGrouping.UnitOfMeasureGroupingContents?.Select(x => new DirectSalesOrder_UnitOfMeasureGroupingContentDTO(x)).ToList();
            this.Errors = UnitOfMeasureGrouping.Errors;
        }
    }

    public class DirectSalesOrder_UnitOfMeasureGroupingFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public IdFilter StatusId { get; set; }
        public UnitOfMeasureGroupingOrder OrderBy { get; set; }
    }
}
