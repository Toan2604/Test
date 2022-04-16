using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreStatisticDTO : DataDTO
    {
        public decimal Revenue { get; set; }
    }

    public class GeneralMobile_StoreStatisticFilterDTO : FilterDTO
    {
        public IdFilter SalesOrderTypeId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter Time { get; set; }
    }
}
