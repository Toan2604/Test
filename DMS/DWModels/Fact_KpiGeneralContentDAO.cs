using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Fact_KpiGeneralContentDAO
    {
        public long KpiGeneralContentId { get; set; }
        public long KpiGeneralId { get; set; }
        public long OrganizationId { get; set; }
        public long EmployeeId { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public long KpiCriteriaGeneralId { get; set; }
        public long? YearId { get; set; }
        public long? QuarterId { get; set; }
        public long? MonthId { get; set; }
        public decimal? Value { get; set; }
    }
}
