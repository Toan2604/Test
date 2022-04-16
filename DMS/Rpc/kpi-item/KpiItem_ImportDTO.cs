using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public class KpiItem_ImportDTO : DataDTO
    {
        public long Stt { get; set; }
        public string UsernameValue { get; set; }
        public string ItemCodeValue { get; set; }

        public List<KpiItem_CriteriaContentDTO> CriteriaContents { get; set; }

        public bool IsNew { get; set; }
        public long EmployeeId { get; set; }
        public long ItemId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiItemTypeId { get; set; }
        public long OrganizationId { get; set; }

        public bool HasValue
        {
            get
            {
                if (this.CriteriaContents.Any(x => !string.IsNullOrWhiteSpace(x.Value)))
                    return true;
                return false;
            }
        }
    }

    public class KpiItem_CriteriaContentDTO : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public string Value { get; set; }

    }

    public class KpiItem_RowDTO : IEquatable<KpiItem_RowDTO>
    {
        public long AppUserId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiItemTypeId { get; set; }
        public bool Equals(KpiItem_RowDTO other)
        {
            return other != null && this.AppUserId == other.AppUserId && this.KpiYearId == other.KpiYearId && this.KpiPeriodId == other.KpiPeriodId && this.KpiItemTypeId == other.KpiItemTypeId;
        }
        public override int GetHashCode()
        {
            return AppUserId.GetHashCode() ^ KpiYearId.GetHashCode() ^ KpiPeriodId.GetHashCode() ^ KpiItemTypeId.GetHashCode();
        }
    }
}
