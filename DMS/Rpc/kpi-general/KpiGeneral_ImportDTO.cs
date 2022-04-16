using System;
using TrueSight.Common;

namespace DMS.Rpc.kpi_general
{
    public class KpiGeneral_ImportDTO : DataDTO
    {
        public long Stt { get; set; }
        public string UsernameValue { get; set; }
        public string CriterialValue { get; set; }
        public string M1Value { get; set; }
        public string M2Value { get; set; }
        public string M3Value { get; set; }
        public string M4Value { get; set; }
        public string M5Value { get; set; }
        public string M6Value { get; set; }
        public string M7Value { get; set; }
        public string M8Value { get; set; }
        public string M9Value { get; set; }
        public string M10Value { get; set; }
        public string M11Value { get; set; }
        public string M12Value { get; set; }
        public string Q1Value { get; set; }
        public string Q2Value { get; set; }
        public string Q3Value { get; set; }
        public string Q4Value { get; set; }
        public string YValue { get; set; }
        public bool IsNew { get; set; }
        public long EmployeeId { get; set; }
        public long KpiYearId { get; set; }
        public long OrganizationId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }

        public bool HasValue
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(M1Value) ||
                    !string.IsNullOrWhiteSpace(M2Value) ||
                    !string.IsNullOrWhiteSpace(M3Value) ||
                    !string.IsNullOrWhiteSpace(M4Value) ||
                    !string.IsNullOrWhiteSpace(M5Value) ||
                    !string.IsNullOrWhiteSpace(M6Value) ||
                    !string.IsNullOrWhiteSpace(M7Value) ||
                    !string.IsNullOrWhiteSpace(M8Value) ||
                    !string.IsNullOrWhiteSpace(M9Value) ||
                    !string.IsNullOrWhiteSpace(M10Value) ||
                    !string.IsNullOrWhiteSpace(M11Value) ||
                    !string.IsNullOrWhiteSpace(M12Value) ||
                    !string.IsNullOrWhiteSpace(Q1Value) ||
                    !string.IsNullOrWhiteSpace(Q2Value) ||
                    !string.IsNullOrWhiteSpace(Q3Value) ||
                    !string.IsNullOrWhiteSpace(Q4Value) ||
                    !string.IsNullOrWhiteSpace(YValue))
                    return true;
                return false;
            }
        }
    }

    public class KpiGeneral_RowDTO : IEquatable<KpiGeneral_RowDTO>
    {
        public long AppUserId { get; set; }
        public long KpiYearId { get; set; }
        public bool Equals(KpiGeneral_RowDTO other)
        {
            return other != null && this.AppUserId == other.AppUserId && this.KpiYearId == other.KpiYearId;
        }
        public override int GetHashCode()
        {
            return AppUserId.GetHashCode() ^ KpiYearId.GetHashCode();
        }
    }
}
