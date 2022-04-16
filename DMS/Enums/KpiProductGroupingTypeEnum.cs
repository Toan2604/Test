using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class KpiProductGroupingTypeEnum
    {
        public static GenericEnum NEW_PRODUCT_GROUPING = new GenericEnum { Id = 1, Code = "NewProductGrouping", Name = "KPI nhóm sản phẩm mới" };
        public static GenericEnum ALL_NEW_PRODUCT = new GenericEnum { Id = 2, Code = "AllProductGrouping", Name = "KPI nhóm sản phẩm trọng tâm" };
        public static List<GenericEnum> KpiProductGroupingTypeEnumList = new List<GenericEnum>
        {
            NEW_PRODUCT_GROUPING, ALL_NEW_PRODUCT
        };
    }
}
