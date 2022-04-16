using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class UsedVariationEnum
    {
        public static GenericEnum USED = new GenericEnum { Id = 1, Code = "USED", Name = "Có" };
        public static GenericEnum NOTUSED = new GenericEnum { Id = 0, Code = "NOTUSED", Name = "Không" };

        public static List<GenericEnum> UsedVariationEnumList = new List<GenericEnum>()
        {
            USED, NOTUSED
        };
    }
}
