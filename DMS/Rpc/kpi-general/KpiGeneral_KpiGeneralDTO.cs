using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_KpiGeneralDTO : DataDTO
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long EmployeeId { get; set; }
        public long KpiYearId { get; set; }
        public GenericEnum CurrentMonth { get; set; }
        public GenericEnum CurrentQuarter { get; set; }
        public GenericEnum CurrentYear { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public KpiGeneral_AppUserDTO Creator { get; set; }
        public KpiGeneral_AppUserDTO Employee { get; set; }
        public KpiGeneral_KpiYearDTO KpiYear { get; set; }
        public KpiGeneral_OrganizationDTO Organization { get; set; }
        public KpiGeneral_StatusDTO Status { get; set; }
        public List<KpiGeneral_KpiGeneralContentDTO> KpiGeneralContents { get; set; }
        public List<KpiGeneral_KpiPeriodDTO> KpiPeriods { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<long> EmployeeIds { get; set; }
        public KpiGeneral_KpiGeneralDTO() { }
        public KpiGeneral_KpiGeneralDTO(KpiGeneral KpiGeneral)
        {
            this.Id = KpiGeneral.Id;
            this.OrganizationId = KpiGeneral.OrganizationId;
            this.EmployeeId = KpiGeneral.EmployeeId;
            this.EmployeeIds = KpiGeneral.EmployeeIds;
            this.KpiYearId = KpiGeneral.KpiYearId;
            this.StatusId = KpiGeneral.StatusId;
            this.CreatorId = KpiGeneral.CreatorId;
            this.Creator = KpiGeneral.Creator == null ? null : new KpiGeneral_AppUserDTO(KpiGeneral.Creator);
            this.Employee = KpiGeneral.Employee == null ? null : new KpiGeneral_AppUserDTO(KpiGeneral.Employee);
            this.KpiYear = KpiGeneral.KpiYear == null ? null : new KpiGeneral_KpiYearDTO(KpiGeneral.KpiYear);
            this.Organization = KpiGeneral.Organization == null ? null : new KpiGeneral_OrganizationDTO(KpiGeneral.Organization);
            this.Status = KpiGeneral.Status == null ? null : new KpiGeneral_StatusDTO(KpiGeneral.Status);

            this.KpiGeneralContents = KpiGeneral.KpiGeneralContents?.Select(x => new KpiGeneral_KpiGeneralContentDTO(x)).ToList();
            this.CreatedAt = KpiGeneral.CreatedAt;
            this.UpdatedAt = KpiGeneral.UpdatedAt;
            this.Errors = KpiGeneral.Errors;
        }
    }

    public class KpiGeneral_KpiGeneralFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public KpiGeneralOrder OrderBy { get; set; }
    }
}
