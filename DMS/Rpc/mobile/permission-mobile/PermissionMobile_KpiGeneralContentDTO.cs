using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_KpiGeneralContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long KpiGeneralId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }
        public long StatusId { get; set; }
        public PermissionMobile_KpiCriteriaGeneralDTO KpiCriteriaGeneral { get; set; }
        public PermissionMobile_StatusDTO Status { get; set; }
        public Dictionary<long, decimal?> KpiGeneralContentKpiPeriodMappings { get; set; }
        public Dictionary<long, bool> KpiGeneralContentKpiPeriodMappingEnables { get; set; }


        public PermissionMobile_KpiGeneralContentDTO() { }
        public PermissionMobile_KpiGeneralContentDTO(KpiGeneralContent KpiGeneralContent)
        {
            this.Id = KpiGeneralContent.Id;
            this.KpiGeneralId = KpiGeneralContent.KpiGeneralId;
            this.KpiCriteriaGeneralId = KpiGeneralContent.KpiCriteriaGeneralId;
            this.StatusId = KpiGeneralContent.StatusId;
            this.KpiCriteriaGeneral = KpiGeneralContent.KpiCriteriaGeneral == null ? null : new PermissionMobile_KpiCriteriaGeneralDTO(KpiGeneralContent.KpiCriteriaGeneral);
            this.Status = KpiGeneralContent.Status == null ? null : new PermissionMobile_StatusDTO(KpiGeneralContent.Status);
            this.KpiGeneralContentKpiPeriodMappings = KpiGeneralContent.KpiGeneralContentKpiPeriodMappings?.ToDictionary(x => x.KpiPeriodId, y => y.Value);
        }
    }

    public class PermissionMobile_KpiGeneralContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter KpiGeneralId { get; set; }

        public IdFilter KpiCriteriaGeneralId { get; set; }

        public IdFilter StatusId { get; set; }

        public KpiGeneralContentOrder OrderBy { get; set; }
    }
}