using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_UnitOfMeasureGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UnitOfMeasureId { get; set; }
        public GeneralMobile_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public List<GeneralMobile_UnitOfMeasureGroupingContentDTO> UnitOfMeasureGroupingContents { get; set; }
        public GeneralMobile_UnitOfMeasureGroupingDTO() { }
        public GeneralMobile_UnitOfMeasureGroupingDTO(UnitOfMeasureGrouping UnitOfMeasureGrouping)
        {
            this.Id = UnitOfMeasureGrouping.Id;
            this.Code = UnitOfMeasureGrouping.Code;
            this.Name = UnitOfMeasureGrouping.Name;
            this.Description = UnitOfMeasureGrouping.Description;
            this.UnitOfMeasureId = UnitOfMeasureGrouping.UnitOfMeasureId;
            this.UnitOfMeasure = UnitOfMeasureGrouping.UnitOfMeasure == null ? null : new GeneralMobile_UnitOfMeasureDTO(UnitOfMeasureGrouping.UnitOfMeasure);
            this.UnitOfMeasureGroupingContents = UnitOfMeasureGrouping.UnitOfMeasureGroupingContents?.Select(x => new GeneralMobile_UnitOfMeasureGroupingContentDTO(x)).ToList();
            this.Errors = UnitOfMeasureGrouping.Errors;
        }
    }

    public class GeneralMobile_UnitOfMeasureGroupingFilterDTO : FilterDTO
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
