using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System.Collections.Generic;
using System.Linq;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_UnitOfMeasureGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UnitOfMeasureId { get; set; }
        public WebDirectSalesOrder_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public List<WebDirectSalesOrder_UnitOfMeasureGroupingContentDTO> UnitOfMeasureGroupingContents { get; set; }
        public WebDirectSalesOrder_UnitOfMeasureGroupingDTO() { }
        public WebDirectSalesOrder_UnitOfMeasureGroupingDTO(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            this.Id = UnitOfMeasureGrouping.Id;
            this.Code = UnitOfMeasureGrouping.Code;
            this.Name = UnitOfMeasureGrouping.Name;
            this.Description = UnitOfMeasureGrouping.Description;
            this.UnitOfMeasureId = UnitOfMeasureGrouping.UnitOfMeasureId;
            this.UnitOfMeasure = UnitOfMeasureGrouping.UnitOfMeasure == null ? null : new WebDirectSalesOrder_UnitOfMeasureDTO(UnitOfMeasureGrouping.UnitOfMeasure);
            this.UnitOfMeasureGroupingContents = UnitOfMeasureGrouping.UnitOfMeasureGroupingContents?.Select(x => new WebDirectSalesOrder_UnitOfMeasureGroupingContentDTO(x)).ToList();
            this.Errors = UnitOfMeasureGrouping.Errors;
        }
    }
    public class WebDirectSalesOrder_UnitOfMeasureGroupingFilterDTO : FilterDTO
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
