using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_KpiGeneralContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long KpiGeneralId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }
        public long StatusId { get; set; }
        public KpiGeneral_KpiCriteriaGeneralDTO KpiCriteriaGeneral { get; set; }
        public KpiGeneral_StatusDTO Status { get; set; }
        public Dictionary<long, decimal?> KpiGeneralContentKpiPeriodMappings { get; set; }
        public Dictionary<long, bool> KpiGeneralContentKpiPeriodMappingEnables { get; set; }


        public KpiGeneral_KpiGeneralContentDTO() { }
        public KpiGeneral_KpiGeneralContentDTO(KpiGeneralContent KpiGeneralContent)
        {
            this.Id = KpiGeneralContent.Id;
            this.KpiGeneralId = KpiGeneralContent.KpiGeneralId;
            this.KpiCriteriaGeneralId = KpiGeneralContent.KpiCriteriaGeneralId;
            this.StatusId = KpiGeneralContent.StatusId;
            this.KpiCriteriaGeneral = KpiGeneralContent.KpiCriteriaGeneral == null ? null : new KpiGeneral_KpiCriteriaGeneralDTO(KpiGeneralContent.KpiCriteriaGeneral);
            this.Status = KpiGeneralContent.Status == null ? null : new KpiGeneral_StatusDTO(KpiGeneralContent.Status);
            this.KpiGeneralContentKpiPeriodMappings = KpiGeneralContent.KpiGeneralContentKpiPeriodMappings?.ToDictionary(x => x.KpiPeriodId, y => y.Value);
        }
    }

    public class KpiGeneral_KpiGeneralContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter KpiGeneralId { get; set; }

        public IdFilter KpiCriteriaGeneralId { get; set; }

        public IdFilter StatusId { get; set; }

        public KpiGeneralContentOrder OrderBy { get; set; }
    }
}