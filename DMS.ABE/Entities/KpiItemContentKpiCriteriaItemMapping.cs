using DMS.ABE.Common; using TrueSight.Common;
using System;

namespace DMS.ABE.Entities
{
    public class KpiItemContentKpiCriteriaItemMapping : DataEntity, IEquatable<KpiItemContentKpiCriteriaItemMapping>
    {
        public long KpiItemContentId { get; set; }
        public long KpiCriteriaItemId { get; set; }
        public long? Value { get; set; }
        public KpiCriteriaItem KpiItemCriteria { get; set; }
        public KpiItemContent KpiItemContent { get; set; }

        public bool Equals(KpiItemContentKpiCriteriaItemMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
