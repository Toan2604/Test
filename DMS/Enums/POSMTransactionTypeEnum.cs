using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class POSMTransactionTypeEnum
    {
        public static GenericEnum ORDER = new GenericEnum { Id = 1, Code = "SHOWING_ORDER", Name = "Cấp mới" };
        public static GenericEnum ORDER_WITHDRAW = new GenericEnum { Id = 2, Code = "SHOWING_ORDER_WITH_DRAW", Name = "Thu hồi" };

        public static List<GenericEnum> POSMTransactionTypeEnumList = new List<GenericEnum> {
            ORDER, ORDER_WITHDRAW
        };
    }
}
