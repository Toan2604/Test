using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class ERouteTypeEnum
    {
        public static GenericEnum PERMANENT = new GenericEnum { Id = 1, Code = "PERMANENT", Name = "Cố định" };
        public static GenericEnum INCURRED = new GenericEnum { Id = 2, Code = "INCURRED", Name = "Phát sinh" };
        public static List<GenericEnum> ERouteTypeEnumList = new List<GenericEnum>()
        {
            PERMANENT, INCURRED
        };
    }
}
