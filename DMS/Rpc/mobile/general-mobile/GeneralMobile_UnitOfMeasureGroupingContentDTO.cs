using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_UnitOfMeasureGroupingContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? Factor { get; set; }
        public GeneralMobile_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public GeneralMobile_UnitOfMeasureGroupingDTO UnitOfMeasureGrouping { get; set; }
        public GeneralMobile_UnitOfMeasureGroupingContentDTO() { }
        public GeneralMobile_UnitOfMeasureGroupingContentDTO(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {
            this.Id = UnitOfMeasureGroupingContent.Id;
            this.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId;
            this.UnitOfMeasureId = UnitOfMeasureGroupingContent.UnitOfMeasureId;
            this.Factor = UnitOfMeasureGroupingContent.Factor;
            this.UnitOfMeasure = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? null : new GeneralMobile_UnitOfMeasureDTO(UnitOfMeasureGroupingContent.UnitOfMeasure);
            this.UnitOfMeasureGrouping = UnitOfMeasureGroupingContent.UnitOfMeasureGrouping == null ? null : new GeneralMobile_UnitOfMeasureGroupingDTO(UnitOfMeasureGroupingContent.UnitOfMeasureGrouping);
            this.Errors = UnitOfMeasureGroupingContent.Errors;
        }
    }

    public class GeneralMobile_UnitOfMeasureGroupingContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter UnitOfMeasureGroupingId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public LongFilter Factor { get; set; }

        public UnitOfMeasureGroupingContentOrder OrderBy { get; set; }
    }
}
