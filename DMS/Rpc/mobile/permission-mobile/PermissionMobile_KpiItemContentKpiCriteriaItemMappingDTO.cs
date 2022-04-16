using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_KpiItemContentKpiCriteriaItemMappingDTO : DataDTO
    {
        public long KpiItemContentId { get; set; }
        public long KpiCriteriaItemId { get; set; }
        public long? Value { get; set; }

        public PermissionMobile_KpiItemContentKpiCriteriaItemMappingDTO() { }
        public PermissionMobile_KpiItemContentKpiCriteriaItemMappingDTO(KpiItemContentKpiCriteriaItemMapping KpiItemContentKpiCriteriaItemMapping)
        {
            this.KpiItemContentId = KpiItemContentKpiCriteriaItemMapping.KpiItemContentId;
            this.KpiCriteriaItemId = KpiItemContentKpiCriteriaItemMapping.KpiCriteriaItemId;
            this.Value = KpiItemContentKpiCriteriaItemMapping.Value;
        }
    }
}
