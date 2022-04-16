using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.kpi_product_grouping
{
    public class KpiProductGrouping_ImportDTO : DataDTO
    {
        public long Stt { get; set; }
        public string Username { get; set; }
        public string ProductGroupingCode { get; set; }
        public bool SelectAllCurrentItem { get; set; }
        public List<KpiProductGrouping_CriteriaContent> CriteriaContents { get; set; }

        public bool HasValue
        {
            get
            {
                if (this.CriteriaContents.Any(x => !string.IsNullOrWhiteSpace(x.Value)))
                    return true;
                return false;
            }
        }

        public bool IsNew { get; set; }
        public long EmployeeId { get; set; }
        public long ProductGroupingId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiProductGroupingTypeId { get; set; }
        public long OrganizationId { get; set; }
        public List<KpiProductGrouping_ItemImportDTO> Items { get; set; }
    }

    public class KpiProductGrouping_CriteriaContent : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public string Value { get; set; }
    }

    public class KpiProductGrouping_RowDTO : IEquatable<KpiProductGrouping_RowDTO>
    {
        public long EmployeeId { get; set; }
        public long KpiYearId { get; set; }
        public long KpiPeriodId { get; set; }
        public long KpiProductGroupingTypeId { get; set; }
        public bool Equals(KpiProductGrouping_RowDTO other)
        {
            return other != null && this.EmployeeId == other.EmployeeId && this.KpiYearId == other.KpiYearId && this.KpiPeriodId == other.KpiPeriodId && this.KpiProductGroupingTypeId == other.KpiProductGroupingTypeId;
        }
        public override int GetHashCode()
        {
            return EmployeeId.GetHashCode() ^ KpiYearId.GetHashCode() ^ KpiPeriodId.GetHashCode() ^ KpiProductGroupingTypeId.GetHashCode();
        }
    }

    public class KpiProductGrouping_ItemImportDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsNew { get; set; }
        public long ProductGroupingId { get; set; }
    }
}
