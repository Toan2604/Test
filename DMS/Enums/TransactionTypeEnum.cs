using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class TransactionTypeEnum
    {
        public static GenericEnum SALES_CONTENT = new GenericEnum { Id = 1, Code = "SALE", Name = "Sản phẩm bán" };
        public static GenericEnum PROMOTION = new GenericEnum { Id = 2, Code = "PROMOTION", Name = "Sản phẩm khuyến mại" };
        public static List<GenericEnum> TransactionTypeEnumList = new List<GenericEnum>
        {
            SALES_CONTENT, PROMOTION,
        };
    }
}
