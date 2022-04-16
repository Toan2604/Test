using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class EditedPriceStatusEnum
    {
        public static GenericEnum ACTIVE = new GenericEnum { Id = 1, Code = "ACTIVE", Name = "Sửa giá" };
        public static GenericEnum INACTIVE = new GenericEnum { Id = 0, Code = "INACTIVE", Name = "Không sửa giá" };
        public static List<GenericEnum> EditedPriceStatusEnumList = new List<GenericEnum>()
        {
            ACTIVE, INACTIVE
        };
    }
}
