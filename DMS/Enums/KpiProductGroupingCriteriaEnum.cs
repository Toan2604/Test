using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class KpiProductGroupingCriteriaEnum
    {
        public static GenericEnum INDIRECT_REVENUE = new GenericEnum { Id = 2, Code = "IndirectRevenue", Name = "Doanh thu ĐGT" };
        public static GenericEnum INDIRECT_STORE = new GenericEnum { Id = 4, Code = "IndirectStore", Name = "Số đại lý hiện diện ĐGT" };
        public static GenericEnum DIRECT_REVENUE = new GenericEnum { Id = 1, Code = "DirectRevenue", Name = "Doanh thu ĐTT" };
        public static GenericEnum DIRECT_STORE = new GenericEnum { Id = 3, Code = "DirectStore", Name = "Số đại lý hiện diện ĐTT" };
        public static List<GenericEnum> KpiProductGroupingCriteriaEnumList = new List<GenericEnum>
        {
            INDIRECT_REVENUE, INDIRECT_STORE, DIRECT_REVENUE, DIRECT_STORE
        };
    }
}
