using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class ExportTemplateEnum
    {
        public static GenericEnum PRINT_DIRECT = new GenericEnum { Id = 1, Code = "PRINT_DIRECT", Name = "Mẫu in đơn hàng trực tiếp" };
        public static GenericEnum PRINT_INDIRECT = new GenericEnum { Id = 2, Code = "PRINT_INDIRECT", Name = "Mẫu in đơn hàng gián tiếp" };
        public static GenericEnum PRINT_INDIRECT_MOBILE = new GenericEnum { Id = 3, Code = "PRINT_INDIRECT_MOBILE", Name = "Mẫu in đơn hàng mobile" };
        public static GenericEnum PRINT_DIRECT_MOBILE = new GenericEnum { Id = 4, Code = "PRINT_DIRECT_MOBILE", Name = "Mẫu in đơn hàng trực tiếp mobile" };
        public static GenericEnum PRINT_DELIVERY_NOTE = new GenericEnum { Id = 5, Code = "PRINT_DELIVERY_NOTE", Name = "Mẫu in phiếu xuất kho" };

        public static List<GenericEnum> ExportTemplateEnumList = new List<GenericEnum>
        {
            PRINT_DIRECT, PRINT_INDIRECT, PRINT_INDIRECT_MOBILE, PRINT_DIRECT_MOBILE, PRINT_DELIVERY_NOTE
        };
    }
}
