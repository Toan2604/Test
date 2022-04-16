using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class CurrentUserEnum
    {
        public static GenericEnum ISNT = new GenericEnum { Id = 0, Code = "ISNT", Name = "Không phải tài khoản hiện tại" };
        public static GenericEnum IS = new GenericEnum { Id = 1, Code = "IS", Name = "Là tài khoản hiện tại" };
        public static List<GenericEnum> CurrentUserEnumList = new List<GenericEnum>
        {
            IS,ISNT,
        };
    }
}
