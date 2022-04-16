using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.product
{
    public class Product_UnitOfMeasureGroupingContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? Factor { get; set; }
        public Product_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public Product_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public Product_UnitOfMeasureGroupingContentDTO()
        {

        }
        public Product_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            this.Id = UnitOfMeasureGroupingContent.Id;
            this.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            this.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            this.Factor = UnitOfMeasureGroupingContent.Factor;
            this.UnitOfMeasure = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? null : new Product_UnitOfMeasureDTO(UnitOfMeasureGroupingContent.UnitOfMeasure);
        }
    }

    public class Product_UnitOfMeasureGroupingContentFilterDTO : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public LongFilter Factor { get; set; }
        public UnitOfMeasureGroupingContentOrder OrderBy { get; set; }
    }
}
