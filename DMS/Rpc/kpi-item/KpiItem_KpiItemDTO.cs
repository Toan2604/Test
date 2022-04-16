using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public class KpiItem_KpiItemDTO : DataDTO
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiItemTypeId { get; set; }
        public long StatusId { get; set; }
        public long EmployeeId { get; set; }
        public long CreatorId { get; set; }
        public bool ReadOnly { get; set; }
        public GenericEnum CurrentMonth { get; set; }
        public GenericEnum CurrentQuarter { get; set; }
        public GenericEnum CurrentYear { get; set; }
        public KpiItem_AppUserDTO Creator { get; set; }
        public KpiItem_AppUserDTO Employee { get; set; }
        public KpiItem_KpiYearDTO KpiYear { get; set; }
        public KpiItem_KpiPeriodDTO KpiPeriod { get; set; }
        public KpiItem_KpiItemTypeDTO KpiItemType { get; set; }
        public KpiItem_OrganizationDTO Organization { get; set; }
        public KpiItem_StatusDTO Status { get; set; }
        public List<KpiItem_KpiItemContentDTO> KpiItemContents { get; set; }
        public List<KpiItem_KpiCriteriaItemDTO> KpiCriteriaItems { get; set; }
        public List<KpiItem_AppUserDTO> Employees { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public KpiItem_KpiItemDTO() { }
        public KpiItem_KpiItemDTO(KpiItem KpiItem)
        {
            this.Id = KpiItem.Id;
            this.OrganizationId = KpiItem.OrganizationId;
            this.KpiPeriodId = KpiItem.KpiPeriodId;
            this.KpiYearId = KpiItem.KpiYearId;
            this.KpiItemTypeId = KpiItem.KpiItemTypeId;
            this.StatusId = KpiItem.StatusId;
            this.EmployeeId = KpiItem.EmployeeId;
            this.CreatorId = KpiItem.CreatorId;
            this.ReadOnly = KpiItem.ReadOnly;
            this.Creator = KpiItem.Creator == null ? null : new KpiItem_AppUserDTO(KpiItem.Creator);
            this.Employee = KpiItem.Employee == null ? null : new KpiItem_AppUserDTO(KpiItem.Employee);
            this.KpiPeriod = KpiItem.KpiPeriod == null ? null : new KpiItem_KpiPeriodDTO(KpiItem.KpiPeriod);
            this.KpiYear = KpiItem.KpiYear == null ? null : new KpiItem_KpiYearDTO(KpiItem.KpiYear);
            this.KpiItemType = KpiItem.KpiItemType == null ? null : new KpiItem_KpiItemTypeDTO(KpiItem.KpiItemType);
            this.Organization = KpiItem.Organization == null ? null : new KpiItem_OrganizationDTO(KpiItem.Organization);
            this.Status = KpiItem.Status == null ? null : new KpiItem_StatusDTO(KpiItem.Status);
            this.KpiItemContents = KpiItem.KpiItemContents?.Select(x => new KpiItem_KpiItemContentDTO(x)).ToList();
            this.Employees = KpiItem.Employees?.Select(x => new KpiItem_AppUserDTO(x)).ToList();
            this.CreatedAt = KpiItem.CreatedAt;
            this.UpdatedAt = KpiItem.UpdatedAt;
            this.Errors = KpiItem.Errors;
        }
    }

    public class KpiItem_KpiItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiItemTypeId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public KpiItemOrder OrderBy { get; set; }
    }
}
