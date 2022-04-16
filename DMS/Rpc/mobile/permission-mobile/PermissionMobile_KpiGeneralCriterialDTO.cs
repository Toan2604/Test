using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_KpiGeneralCriterialDTO : DataDTO
    {
        public long KpiCriterialId { get; set; }
        public string KpiCriterialName { get; set; }
        internal decimal? Plan { get; set; }
        public decimal Value { get; set; }
        public decimal Rate => Plan == null || Plan == 0 ? 0 : Math.Round(Value / Plan.Value * 100, 0);
    }

    public class PermissionMobile_KpiGeneralCriterialFilterDTO : FilterDTO
    {
        public IdFilter Period { get; set; }
    }
}
