using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class SexEnum
    {
        public static GenericEnum Male => new GenericEnum { Id = 1, Name = "Nam", Code = "Male" };
        public static GenericEnum Female => new GenericEnum { Id = 2, Name = "Nữ", Code = "Female" };
        public static List<GenericEnum> SexEnumList = new List<GenericEnum>()
        {
            Male, Female
        };
    }
}
