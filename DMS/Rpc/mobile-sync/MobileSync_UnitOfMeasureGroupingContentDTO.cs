using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_UnitOfMeasureGroupingContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? Factor { get; set; }
        public MobileSync_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public MobileSync_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public MobileSync_UnitOfMeasureGroupingContentDTO() { }
        public MobileSync_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            this.Id = UnitOfMeasureGroupingContent.Id;
            this.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            this.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            this.Factor = UnitOfMeasureGroupingContent.Factor;
            this.UnitOfMeasure = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? null : new MobileSync_UnitOfMeasureDTO(UnitOfMeasureGroupingContent.UnitOfMeasure);
            this.UnitOfMeasureGrouping = UnitOfMeasureGroupingContent.UnitOfMeasureGrouping == null ? null : new MobileSync_UnitOfMeasureGroupingDTO(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping);
        }
    }
}
