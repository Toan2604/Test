using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class ExportTemplateEnum
    {
        public static GenericEnum PRINT_DIRECT = new GenericEnum { Id = 1, Code = "PRINT_DIRECT", Name = "Mẫu in đơn hàng trực tiếp" };
        public static GenericEnum PRINT_INDIRECT = new GenericEnum { Id = 2, Code = "PRINT_INDIRECT", Name = "Mẫu in đơn hàng gián tiếp" };
        public static GenericEnum PRINT_INDIRECT_MOBILE = new GenericEnum { Id = 3, Code = "PRINT_INDIRECT_MOBILE", Name = "Mẫu in đơn hàng mobile" };
        public static GenericEnum PRINT_DIRECT_MOBILE = new GenericEnum { Id = 4, Code = "PRINT_DIRECT_MOBILE", Name = "Mẫu in đơn hàng trực tiếp mobile" };

        public static List<GenericEnum> ExportTemplateEnumList = new List<GenericEnum>
        {
            PRINT_DIRECT, PRINT_INDIRECT, PRINT_INDIRECT_MOBILE, PRINT_DIRECT_MOBILE
        };
    }
}
