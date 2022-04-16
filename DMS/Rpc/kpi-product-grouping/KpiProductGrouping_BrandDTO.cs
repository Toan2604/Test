using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_BrandDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public long StatusId { get; set; }
        public DateTime UpdateTime { get; set; }

        public KpiProductGrouping_BrandDTO() { }
        public KpiProductGrouping_BrandDTO(Brand Brand)
        {

            this.Id = Brand.Id;

            this.Code = Brand.Code;

            this.Name = Brand.Name;
            this.Description = Brand.Description;

            this.StatusId = Brand.StatusId;
            this.UpdateTime = Brand.UpdateTime;
        }
    }

    public class KpiProductGrouping_BrandFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }

        public IdFilter StatusId { get; set; }
        public DateFilter UpdateTime { get; set; }
        public BrandOrder OrderBy { get; set; }
    }
}