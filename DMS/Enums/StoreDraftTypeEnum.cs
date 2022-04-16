using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class StoreDraftTypeEnum
    {
        public static GenericEnum MINE = new GenericEnum { Id = 1, Code = "MINE", Name = "Dự thảo của tôi" };
        public static GenericEnum ORGANIZATION = new GenericEnum { Id = 2, Code = "ORGANIZATION", Name = "Dự thảo của đơn vị" };
        public static List<GenericEnum> StoreDraftTypeEnumList = new List<GenericEnum>()
        {
            MINE, ORGANIZATION
        };
    }
}
