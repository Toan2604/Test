using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_KpiProductGroupingDTO : DataDTO
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiProductGroupingTypeId { get; set; }
        public long StatusId { get; set; }
        public long EmployeeId { get; set; }
        public long CreatorId { get; set; }
        public Guid RowId { get; set; }
        public bool ReadOnly { get; set; }
        public GenericEnum CurrentMonth { get; set; }
        public GenericEnum CurrentQuarter { get; set; }
        public GenericEnum CurrentYear { get; set; }
        public KpiProductGrouping_AppUserDTO Creator { get; set; }
        public KpiProductGrouping_AppUserDTO Employee { get; set; }
        public KpiProductGrouping_KpiPeriodDTO KpiPeriod { get; set; }
        public KpiProductGrouping_KpiYearDTO KpiYear { get; set; }
        public KpiProductGrouping_OrganizationDTO Organization { get; set; }
        public KpiProductGrouping_KpiProductGroupingTypeDTO KpiProductGroupingType { get; set; }
        public KpiProductGrouping_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<KpiProductGrouping_AppUserDTO> Employees { get; set; }
        public List<KpiProductGrouping_KpiProductGroupingContentDTO> KpiProductGroupingContents { get; set; }
        public List<KpiProductGrouping_KpiProductGroupingCriteriaDTO> KpiProductGroupingCriterias { get; set; }

        public KpiProductGrouping_KpiProductGroupingDTO() { }
        public KpiProductGrouping_KpiProductGroupingDTO(KpiProductGrouping KpiProductGrouping)
        {
            this.Id = KpiProductGrouping.Id;
            this.OrganizationId = KpiProductGrouping.OrganizationId;
            this.KpiYearId = KpiProductGrouping.KpiYearId;
            this.KpiPeriodId = KpiProductGrouping.KpiPeriodId;
            this.KpiProductGroupingTypeId = KpiProductGrouping.KpiProductGroupingTypeId;
            this.StatusId = KpiProductGrouping.StatusId;
            this.EmployeeId = KpiProductGrouping.EmployeeId;
            this.CreatorId = KpiProductGrouping.CreatorId;
            this.RowId = KpiProductGrouping.RowId;
            this.Creator = KpiProductGrouping.Creator == null ? null : new KpiProductGrouping_AppUserDTO(KpiProductGrouping.Creator);
            this.Employee = KpiProductGrouping.Employee == null ? null : new KpiProductGrouping_AppUserDTO(KpiProductGrouping.Employee);
            this.KpiPeriod = KpiProductGrouping.KpiPeriod == null ? null : new KpiProductGrouping_KpiPeriodDTO(KpiProductGrouping.KpiPeriod);
            this.KpiYear = KpiProductGrouping.KpiYear == null ? null : new KpiProductGrouping_KpiYearDTO(KpiProductGrouping.KpiYear);
            this.Organization = KpiProductGrouping.Organization == null ? null : new KpiProductGrouping_OrganizationDTO(KpiProductGrouping.Organization);
            this.KpiProductGroupingType = KpiProductGrouping.KpiProductGroupingType == null ? null : new KpiProductGrouping_KpiProductGroupingTypeDTO(KpiProductGrouping.KpiProductGroupingType);
            this.Status = KpiProductGrouping.Status == null ? null : new KpiProductGrouping_StatusDTO(KpiProductGrouping.Status);
            this.KpiProductGroupingContents = KpiProductGrouping.KpiProductGroupingContents == null ? null : KpiProductGrouping.KpiProductGroupingContents
                .Select(x => new KpiProductGrouping_KpiProductGroupingContentDTO(x))
                .ToList();
            this.CreatedAt = KpiProductGrouping.CreatedAt;
            this.UpdatedAt = KpiProductGrouping.UpdatedAt;
            this.Errors = KpiProductGrouping.Errors;
        }
    }

    public class KpiProductGrouping_KpiProductGroupingFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiProductGroupingTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter EmployeeId { get; set; }
        public IdFilter CreatorId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public KpiProductGroupingOrder OrderBy { get; set; }
    }
}
