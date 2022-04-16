using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_KpiItemContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long KpiItemId { get; set; }
        public long ItemId { get; set; }
        public PermissionMobile_ItemDTO Item { get; set; }
        public Dictionary<long, long?> KpiItemContentKpiCriteriaItemMappings { get; set; }


        public PermissionMobile_KpiItemContentDTO() { }
        public PermissionMobile_KpiItemContentDTO(KpiItemContent KpiItemContent)
        {
            this.Id = KpiItemContent.Id;
            this.KpiItemId = KpiItemContent.KpiItemId;
            this.ItemId = KpiItemContent.ItemId;
            this.Item = KpiItemContent.Item == null ? null : new PermissionMobile_ItemDTO(KpiItemContent.Item);
            this.KpiItemContentKpiCriteriaItemMappings = KpiItemContent.KpiItemContentKpiCriteriaItemMappings?.ToDictionary(x => x.KpiCriteriaItemId, y => y.Value);
            this.Errors = KpiItemContent.Errors;
        }
    }
}