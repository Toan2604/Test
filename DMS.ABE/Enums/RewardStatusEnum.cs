using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class RewardStatusEnum
    {
        public static GenericEnum INACTIVE = new GenericEnum { Id = 0, Code = "INACTIVE", Name = "Đã quay" };
        public static GenericEnum ACTIVE = new GenericEnum { Id = 1, Code = "ACTIVE", Name = "Chưa quay" };
        public static List<GenericEnum> RewardStatusEnumList = new List<GenericEnum>()
        {
            INACTIVE, ACTIVE
        };
    }
}
